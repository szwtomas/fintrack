using fintrack.Models;
using Fintrack.Services;
using Fintrack.Services.Errors;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Tests.Services;

public class UserCreationServiceTest : IAsyncLifetime
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
    public async Task CreateUser_ShouldInsertUserInDb()
    {
        var unit = new UserCreationService(_db, new AuthService(_db));
        var user = await unit.CreateUser("john.doe@mail.com", "12345678");

        Assert.NotNull(user);
        var foundUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == "john.doe@mail.com");
        Assert.NotNull(foundUser);
        Assert.Equal(user.HashedPassword, foundUser.HashedPassword);
    }

    [Fact]
    public async Task CreateUser_GivenItAlreadyExists_ItShouldThrowUserAlreadyExistsException()
    {
        var unit = new UserCreationService(_db, new AuthService(_db));
        var user = await unit.CreateUser("john.doe@mail.com", "12345678");

        Assert.NotNull(user);
        await Assert.ThrowsAsync<UserAlreadyExistsException>(() =>
            unit.CreateUser("john.doe@mail.com", "other-password"));
    }
}