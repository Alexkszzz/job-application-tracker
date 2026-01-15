using JobTracker.Api.DTOs.Auth;
using JobTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        if (!result.Succeeded)
        {
            return BadRequest(new { message = "User registration failed.", errors = result.Errors });
        }

        return Ok(result.Response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (!result.Succeeded)
        {
            return Unauthorized(new { message = "Login failed.", errors = result.Errors });
        }

        return Ok(result.Response);
    }
}
