using Microsoft.AspNetCore.Mvc;
using services;

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

