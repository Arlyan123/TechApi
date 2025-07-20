using Microsoft.AspNetCore.Mvc;
using services;

namespace Controllers;

/// <summary>
/// Generar Token
/// </summary>

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly jwtt _jwt;

    public AuthController(jwtt jwt)
    {
        _jwt = jwt;
    }


    /// <summary>
    /// Post de Login / Correo personal de Ing.Sebastian y password se envió en el correo
    /// </summary>
    /// <param name="req">Correo personal de Ing.Sebastian y password se envió en el correo</param>
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto req)
    {
        //simulacro de login
        if (req.Email == "sebastianmalambo9@gmail.com" && req.Password == "123")
        {
            var token = _jwt.GenerateToken(req.Email);
            return Ok(new { token });
        }

        return Unauthorized();
    }


}

