using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] AdminLoginModel login)
    {
        string role;
        if (login.Username == "admin" && login.Password == "password")
        {
            role = "Admin";
        }
        else if (login.Username == "user" && login.Password == "password")
        {
            role = "User";
        }
        else
        {
            return Unauthorized();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("Your_Secret_Key_Which_Is_Long_Enough");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "yourdomain.com",
            Audience = "yourdomain.com"
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { Token = tokenString });
    }
}

public class AdminLoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}