using InterviewTracker.Api.Auth;
using InterviewTracker.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        if (request.Username != "admin" || request.Password != "password")
            return Unauthorized("Invalid username or password.");

        var token = _jwtTokenService.GenerateToken(request.Username);

        return Ok(new
        {
            accessToken = token
        });
    }
}