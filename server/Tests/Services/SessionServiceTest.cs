using fintrack.Models;
using Fintrack.Services;
using Testcontainers.PostgreSql;

namespace Tests.Services;

public class SessionServiceTest : IAsyncLifetime
{
    private PostgreSqlContainer _testContainer = null!;
    private AppDbContext _db = null!;

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
    public async Task CreateSession_ShouldInsertSessionInDb()
    {
        var user = await CreateUser();
        var unit = new SessionService(_db);
        var sessionToken = await unit.CreateSession(user.UserId);
        var session = _db.UserSessions.FirstOrDefault(us => us.SessionToken == sessionToken);
        Assert.NotNull(session);
        Assert.Equal(1, session.UserId);
    }

    [Fact]
    public async Task GetSession_GivenItExists_ShouldReturnExistingSession()
    {
        var user = await CreateUser();
        var unit = new SessionService(_db);
        var sessionToken = await unit.CreateSession(user.UserId);

        var session = await unit.GetSession(sessionToken);
        Assert.NotNull(session);
        Assert.Equal(sessionToken, session.SessionToken);
        Assert.Equal(user.UserId, session.UserId);
    }

    [Fact]
    public async Task GetSession_GivenItDoesNotExist_ShouldReturnNull()
    {
        var unit = new SessionService(_db);
        var session = await unit.GetSession("does-not-exist");
        Assert.Null(session);
    }

    [Fact]
    public async Task DeleteSession_GivenItExists_ShouldDeleteSession()
    {
        var user = await CreateUser();
        var unit = new SessionService(_db);
        var sessionToken = await unit.CreateSession(user.UserId);

        var isDeleted = await unit.DeleteSession(sessionToken);
        Assert.True(isDeleted);

        var session = _db.UserSessions.FirstOrDefault(us => us.SessionToken == sessionToken);
        Assert.Null(session);
    }

    [Fact]
    public async Task DeleteSession_GivenItDoesNotExist_DeleteShouldReturnFalse()
    {
        var unit = new SessionService(_db);
        var isDeleted = await unit.DeleteSession("does-not-exist");
        Assert.False(isDeleted);
    }

    private async Task<User> CreateUser()
    {
        var user = new User
        {
            Email = "john.doe@mail.com",
            HashedPassword = "12345678"
        };

        var createdUser = _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return createdUser.Entity;
    }
}