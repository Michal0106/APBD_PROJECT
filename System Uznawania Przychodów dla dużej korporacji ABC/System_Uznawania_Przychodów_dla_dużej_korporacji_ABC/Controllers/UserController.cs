using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.DTOs;
using System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Services;

namespace System_Uznawania_Przychodów_dla_dużej_korporacji_ABC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        var user = await _userService.RegisterAsync(request.Username, request.Password, request.Role);
        if (user == null)
        {
            return BadRequest("Registration failed");
        }
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var token = await _userService.LoginAsync(request.Username, request.Password);
        if (token == null)
        {
            return Unauthorized("Invalid username or password");
        }
        return Ok(new { Token = token });
    }
}
