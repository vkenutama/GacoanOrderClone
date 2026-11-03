using Gacoan.Backend.Dtos;
using Gacoan.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gacoan.Backend.Controllers;

public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return HandleResult(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        if (CurrentUserId == Guid.Empty)
        {
            return Unauthorized("User belum terautentikasi.");
        }

        var result = await _authService.GetCurrentUserAsync(CurrentUserId);
        return HandleResult(result);
    }
}
