using Fintrack.Controllers;
using Fintrack.Dto;
using fintrack.Models;
using Fintrack.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using Testcontainers.PostgreSql;

namespace Tests.Controllers;

public class AuthControllerTest : IAsyncLifetime
{
    private PostgreSqlContainer _testContainer = null!;
    private AppDbContext _db = null!;
    private readonly ILogger<AuthController> _logger = new Mock<ILogger<AuthController>>().Object;

    public async Task InitializeAsync()
    {
        var container = new PostgreSqlBuilder()
            .WithDatabase("test_db")
            .WithUsername("postgres:latest")
            .WithPassword("password")
            .Build();
        _testContainer = container;

        await _testContainer.StartAsync();

        _db = new AppDbContext(container.GetConnectionString());
        await _db.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _testContainer.StopAsync();
        await _db.DisposeAsync();
    }

    [Fact]
    public async Task SignUp_GivenUserDoesNotExist_ShouldCreateUser()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var request = new SignUpRequest { Email = "john.doe@mail.com", Password = "12345678" };
        var response = await unit.SignUp(request);
        var result = Assert.IsType<ObjectResult>(response);

        Assert.Equal(201, result.StatusCode);
        var responseDto = Assert.IsType<UserDto>(result.Value);

        Assert.Equal("john.doe@mail.com", responseDto.Email);
        Assert.NotNull(authService.AuthenticateCredentials("john.doe@mail.com", "12345678"));
    }

    [Fact]
    public async Task SignUp_GivenUserAlreadyExists_ShouldReturnConflictStatus()
    {
        _db.Users.Add(new User { Email = "john.doe@mail.com", HashedPassword = "some.password" });
        await _db.SaveChangesAsync();
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var request = new SignUpRequest { Email = "john.doe@mail.com", Password = "12345678" };
        var response = await unit.SignUp(request);
        var result = Assert.IsType<ConflictObjectResult>(response);

        Assert.Equal(409, result.StatusCode);
    }

    [Fact]
    public async Task Login_GivenUserExistsAndValidCredentials_ShouldReturnOkWithUserData_AndCookie()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);

        var user = await userCreationService.CreateUser("john.doe@mail.com", "12345678");

        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var request = new LogInRequest { Email = "john.doe@mail.com", Password = "12345678" };
        var response = await unit.LogIn(request);
        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);

        var sessionToken = SessionTokenFromHeaders(unit.HttpContext.Response.Headers.SetCookie);
        Assert.NotNull(sessionToken);
        var session = await _db.UserSessions.FirstOrDefaultAsync(us => us.SessionToken == sessionToken);
        Assert.NotNull(session);
        Assert.Equal(user.UserId, session.UserId);
    }

    [Fact]
    public async Task LogIn_GivenInvalidCredentials_ItShouldReturnUnauthorized()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        await userCreationService.CreateUser("john.doe@mail.com", "12345678");
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var request = new LogInRequest { Email = "john.doe@mail.com", Password = "incorrect.password" };
        var response = await unit.LogIn(request);
        var result = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(401, result.StatusCode);
        var cookie = SessionTokenFromHeaders(unit.HttpContext.Response.Headers.SetCookie);
        Assert.Null(cookie);
    }

    [Fact]
    public async Task LogIn_GivenUserDoesNotExist_ShouldReturnNotFoundStatus()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var request = new LogInRequest { Email = "does.not.exist@mail.com", Password = "12345678" };
        var response = await unit.LogIn(request);
        var result = Assert.IsType<NotFoundResult>(response);
        Assert.Equal(404, result.StatusCode);
        var cookie = SessionTokenFromHeaders(unit.HttpContext.Response.Headers.SetCookie);
        Assert.Null(cookie);
    }

    [Fact]
    public async Task LogOut_GivenValidSessionToken_ShouldRemoveSession()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        await userCreationService.CreateUser("john.doe@mail.com", "12345678");
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        var logInRequest = new LogInRequest { Email = "john.doe@mail.com", Password = "12345678" };
        await unit.LogIn(logInRequest);

        var sessionToken = SessionTokenFromHeaders(unit.HttpContext.Response.Headers.SetCookie);
        Assert.NotNull(sessionToken);

        var session = await sessionService.GetSession(sessionToken);
        Assert.NotNull(session);

        unit.ControllerContext.HttpContext = new DefaultHttpContext();
        var headerCookie = new CookieHeaderValue("fintrack-session", sessionToken);
        unit.ControllerContext.HttpContext.Request.Headers.Add("cookie", headerCookie.ToString());

        var response = await unit.LogOut();
        var result = Assert.IsType<OkResult>(response);
        Assert.Equal(200, result.StatusCode);
        session = await sessionService.GetSession(sessionToken);
        Assert.Null(session);
    }

    [Fact]
    public async Task LogOut_GivenInvalidSessionCookie_ItShouldReturnUnauthorized()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        unit.ControllerContext.HttpContext = new DefaultHttpContext();
        var headerCookie = new CookieHeaderValue("fintrack-session", "does-not-exist");
        unit.ControllerContext.HttpContext.Request.Headers.Add("cookie", headerCookie.ToString());

        var response = await unit.LogOut();
        var result = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(401, result.StatusCode);
    }

    [Fact]
    public async Task IsAuthenticated_GivenNoCookie_ShouldReturnUnauthorized_ShouldNotAddCookie()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        unit.ControllerContext.HttpContext = new DefaultHttpContext();
        var response = await unit.IsAuthenticated();
        var result = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(401, result.StatusCode);
        var cookie = SessionTokenFromHeaders(unit.HttpContext.Response.Headers.SetCookie);
        Assert.Null(cookie);
    }

    [Fact]
    public async Task IsAuthenticated_GivenInvalidSessionCookie_ShouldReturnUnauthorized_ShouldNotAddCookie()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        unit.ControllerContext.HttpContext = new DefaultHttpContext();
        var headerCookie = new CookieHeaderValue("fintrack-session", "does-not-exist");
        unit.ControllerContext.HttpContext.Request.Headers.Add("cookie", headerCookie.ToString());

        var response = await unit.IsAuthenticated();
        var result = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(401, result.StatusCode);
        var cookie = SessionTokenFromHeaders(unit.HttpContext.Response.Headers.SetCookie);
        Assert.Null(cookie);
    }

    [Fact]
    public async Task IsAuthenticated_GivenExpiredSessionCookie_ShouldReturnUnauthorized_ShouldNotAddCookie()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        var user = await userCreationService.CreateUser("john.doe@mail.com", "password");
        var sessionToken = await sessionService.CreateSession(user.UserId);
        var session = await _db.UserSessions.FirstOrDefaultAsync(s => s.SessionToken == sessionToken);
        Assert.NotNull(session);
        session.ExpiresAt = DateTime.UtcNow.AddDays(-1);
        _db.UserSessions.Update(session);
        await _db.SaveChangesAsync();
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        unit.ControllerContext.HttpContext = new DefaultHttpContext();
        var headerCookie = new CookieHeaderValue("fintrack-session", sessionToken);
        unit.ControllerContext.HttpContext.Request.Headers.Add("cookie", headerCookie.ToString());

        var response = await unit.IsAuthenticated();
        var result = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(401, result.StatusCode);
        var cookie = SessionTokenFromHeaders(unit.HttpContext.Response.Headers.SetCookie);
        Assert.Null(cookie);
    }
    
    [Fact]
    public async Task IsAuthenticated_GivenValidSessionCookie_ShouldReturnOkWithUserDto()
    {
        var sessionService = new SessionService(_db);
        var authService = new AuthService(_db);
        var userCreationService = new UserCreationService(_db, authService);
        var user = await userCreationService.CreateUser("john.doe@mail.com", "password");
        var sessionToken = await sessionService.CreateSession(user.UserId);
        
        var unit = new AuthController(_logger, sessionService, authService, userCreationService)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
        };

        unit.ControllerContext.HttpContext = new DefaultHttpContext();
        var headerCookie = new CookieHeaderValue("fintrack-session", sessionToken);
        unit.ControllerContext.HttpContext.Request.Headers.Add("cookie", headerCookie.ToString());

        var response = await unit.IsAuthenticated();
        var result = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(200, result.StatusCode);
        var userDto = Assert.IsType<UserDto>(result.Value);
        Assert.Equal(user.UserId, userDto.UserId);
        Assert.Equal(user.Email, userDto.Email);
    }

    private static string? SessionTokenFromHeaders(string? headers)
    {
        if (string.IsNullOrEmpty(headers))
        {
            return null;
        }

        var cookieHeaderParts = headers.Split(";")[0].Split("=");
        var cookieName = cookieHeaderParts[0];
        var sessionToken = cookieHeaderParts[1];
        return cookieName == "fintrack-session" ? sessionToken : null;
    }
}