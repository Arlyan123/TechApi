using Microsoft.AspNetCore.Mvc;
using service;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly jwtt _jwt;

    public AuthController(jwtt jwt)
    {
        _jwt = jwt;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        // Simulamos autenticación
        if (req.Email == "admin@test.com" && req.Password == "1234")
        {
            var token = _jwt.GenerateToken(req.Email);
            return Ok(new { token });
        }

        return Unauthorized();
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}
