using System.Security.Claims;
using AdminSystem.Core.DTOs;
using AdminSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ApiResponse<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            return ApiResponse<LoginResponse>.Success(result, "登录成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginResponse>.Fail(ex.Message, 400);
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public ApiResponse Logout()
    {
        return ApiResponse.Success("退出登录成功");
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<ApiResponse<CurrentUserResponse>> GetCurrentUser()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _authService.GetCurrentUserAsync(userId);

        if (result == null)
        {
            return ApiResponse<CurrentUserResponse>.Fail("用户不存在", 404);
        }

        return ApiResponse<CurrentUserResponse>.Success(result);
    }
}
