using fintrack.Models;
using Fintrack.Services;
using Fintrack.Services.Errors;
using Testcontainers.PostgreSql;

namespace Tests.Services;

public class AuthServiceTest : IAsyncLifetime
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
    public async Task HashPassword_ShouldGenerateDifferentAndVerifyWhenAuthenticating()
    {
        var unit = new AuthService(_db);
        var hashedPassword = unit.HashPassword("12345678");
        var anotherHashedPassword = unit.HashPassword("12345678");
        var yetAnotherDifferent = unit.HashPassword("password");

        Assert.NotEqual(hashedPassword, anotherHashedPassword);
        Assert.NotEqual(hashedPassword, yetAnotherDifferent);
        Assert.NotEqual(anotherHashedPassword, yetAnotherDifferent);

        var user = new User
        {
            Email = "john.doe@mail.com",
            HashedPassword = hashedPassword
        };
        var createdUser = _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var authenticated = await unit.AuthenticateCredentials("john.doe@mail.com", "12345678");
        Assert.NotNull(authenticated);
        Assert.Equal(authenticated.UserId, createdUser.Entity.UserId);

        var notAuthenticated = await unit.AuthenticateCredentials("john.doe@mail.com", "incorrect");
        Assert.Null(notAuthenticated);
    }

    [Fact]
    public async Task AuthenticateUser_GivenUserDoesNotExist_ShouldThrowUserNotFoundException()
    {
        var unit = new AuthService(_db);
        await Assert.ThrowsAsync<UserNotFoundException>(() =>
            unit.AuthenticateCredentials("john.doe@mail.com", "12345678"));
    }
}