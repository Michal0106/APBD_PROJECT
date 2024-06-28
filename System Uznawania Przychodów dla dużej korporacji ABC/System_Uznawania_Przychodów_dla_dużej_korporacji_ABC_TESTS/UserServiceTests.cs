using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Data;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Models;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

namespace ContractServiceTests;

public class UserServiceTests : IDisposable
{
    private readonly UserService _userService;
    private readonly DatabaseContext _context;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("Jwt:Key", "a very very very very secret key")
            })
            .Build();

        _userService = new UserService(_context, configuration);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task RegisterAsync_Should_Register_User()
    {
        // Arrange
        var username = "testuser";
        var password = "testpassword";
        var role = "user";

        // Act
        var result = await _userService.RegisterAsync(username, password, role);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(username, result.Username);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, result.Password));
        Assert.Equal(role, result.Role);

        var userInDb = await _context.Users.FindAsync(result.Id);
        Assert.NotNull(userInDb);
    }

    [Fact]
    public async Task LoginAsync_Should_Return_JwtToken_On_Valid_Credentials()
    {
        // Arrange
        var username = "testuser";
        var password = "testpassword";
        var role = "user";

        var user = new User { Username = username, Password = BCrypt.Net.BCrypt.HashPassword(password), Role = role };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.NotNull(result);
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadToken(result) as JwtSecurityToken;
        Assert.NotNull(token);
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Null_On_Invalid_Credentials()
    {
        // Arrange
        var username = "testuser";
        var password = "wrongpassword";

        var user = new User { Username = username, Password = BCrypt.Net.BCrypt.HashPassword("correctpassword"), Role = "user" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Null_When_User_Not_Found()
    {
        // Arrange
        var username = "nonexistentuser";
        var password = "password";

        // Act
        var result = await _userService.LoginAsync(username, password);

        // Assert
        Assert.Null(result);
    }
}