using Fintrack.Dto;
using Fintrack.Services;
using Fintrack.Services.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Fintrack.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController(
    ILogger<AuthController> logger,
    SessionService sessionService,
    AuthService authService,
    UserCreationService userCreationService) : ControllerBase
{
    private const string SessionCookieName = "fintrack-session";

    [HttpGet]
    public async Task<IActionResult> IsAuthenticated()
    {
        try
        {
            var sessionToken = Request.Cookies[SessionCookieName];
            if (sessionToken == null)
            {
                return Unauthorized("unauthorized");
            }

            var session = await sessionService.GetSession(sessionToken);
            if (session == null)
            {
                return Unauthorized();
            }

            var dto = new UserDto { UserId = session.UserId, Email = session.User.Email };
            return Ok(dto);
        }
        catch (Exception e)
        {
            logger.LogError("Unexpected error checking authentication: {} {}", e.Message, e.InnerException?.Message);
            return StatusCode(500, "Unexpected error checking authentication");
        }
    }

    [HttpPost("logIn")]
    public async Task<IActionResult> LogIn(LogInRequest request)
    {
        try
        {
            var user = await authService.AuthenticateCredentials(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var sessionToken = await sessionService.CreateSession(user.UserId);
            Response.Cookies.Append(SessionCookieName, sessionToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            var dto = new UserDto { UserId = user.UserId, Email = user.Email };
            return Ok(dto);
        }
        catch (UserNotFoundException e)
        {
            logger.LogError("Failed logging in with user not found: {}", e.Message);
            return NotFound();
        }
        catch (Exception e)
        {
            logger.LogError("Unexpected error checking authentication: {} {}", e.Message, e.InnerException?.Message);
            return StatusCode(500, "Unexpected error checking authentication");
        }
    }

    [HttpPost("signUp")]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        try
        {
            var user = await userCreationService.CreateUser(request.Email, request.Password);

            var sessionToken = await sessionService.CreateSession(user.UserId);
            Response.Cookies.Append(SessionCookieName, sessionToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            var dto = new UserDto { UserId = user.UserId, Email = user.Email };
            return Ok(dto);
        }
        catch (UserAlreadyExistsException e)
        {
            logger.LogError(e.Message);
            return Conflict($"User with email {request.Email} already exists");
        }
        catch (Exception e)
        {
            logger.LogError("Unexpected error creating user: {} {}", e.Message, e.InnerException?.Message);
            return StatusCode(500, "Unexpected error creating user");
        }
    }

    [HttpPost("logOut")]
    public async Task<IActionResult> LogOut()
    {
        var sessionToken = Request.Cookies[SessionCookieName];
        if (sessionToken == null)
        {
            return Unauthorized();
        }

        var success = await sessionService.DeleteSession(sessionToken);
        if (!success)
        {
            return Unauthorized();
        }

        Response.Cookies.Delete(SessionCookieName);
        return Ok();
    }
}