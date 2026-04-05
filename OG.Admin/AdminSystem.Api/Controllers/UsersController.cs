using AdminSystem.Core.DTOs;
using AdminSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ApiResponse<PagedResult<UserDto>>> GetPageList([FromQuery] UserQueryRequest request)
    {
        var result = await _userService.GetPageListAsync(request);
        return ApiResponse<PagedResult<UserDto>>.Success(result);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<UserDto>> GetById(long id)
    {
        var result = await _userService.GetByIdAsync(id);

        if (result == null)
        {
            return ApiResponse<UserDto>.Fail("用户不存在", 404);
        }

        return ApiResponse<UserDto>.Success(result);
    }

    [HttpPost]
    public async Task<ApiResponse<UserDto>> Create([FromBody] CreateUserRequest request)
    {
        try
        {
            var result = await _userService.CreateAsync(request);
            return ApiResponse<UserDto>.Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail(ex.Message, 400);
        }
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<UserDto>> Update(long id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var result = await _userService.UpdateAsync(id, request);
            return ApiResponse<UserDto>.Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<UserDto>.Fail(ex.Message, 400);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        try
        {
            await _userService.DeleteAsync(id);
            return ApiResponse.Success("删除成功");
        }
        catch (Exception ex)
        {
            return ApiResponse.Fail(ex.Message, 400);
        }
    }

    [HttpPut("{id}/status/{status}")]
    public async Task<ApiResponse> UpdateStatus(long id, int status)
    {
        var result = await _userService.UpdateStatusAsync(id, status);

        if (!result)
        {
            return ApiResponse.Fail("更新失败", 400);
        }

        return ApiResponse.Success("更新成功");
    }

    [HttpPut("{id}/password")]
    public async Task<ApiResponse> ChangePassword(long id, [FromBody] ChangePasswordRequest request)
    {
        try
        {
            var result = await _userService.ChangePasswordAsync(id, request.OldPassword, request.NewPassword);

            if (!result)
            {
                return ApiResponse.Fail("修改失败", 400);
            }

            return ApiResponse.Success("密码修改成功");
        }
        catch (Exception ex)
        {
            return ApiResponse.Fail(ex.Message, 400);
        }
    }
}
