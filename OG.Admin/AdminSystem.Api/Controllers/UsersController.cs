using System;
using System.Threading.Tasks;
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
    public async Task<ApiResponse<PageResult<UserDto>>> GetPageList([FromQuery] UserQueryRequest request)
    {
        var result = await _userService.GetPageListAsync(request);
        return ApiResponse<PageResult<UserDto>>.Success(result);
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
    public async Task<ApiResponse<long>> Create([FromBody] CreateUserRequest request)
    {
        var id = await _userService.CreateAsync(request);
        return ApiResponse<long>.Success(id, "创建成功");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Update(long id, [FromBody] UpdateUserRequest request)
    {
        await _userService.UpdateAsync(id, request);
        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        await _userService.DeleteAsync(id);
        return ApiResponse.Success("删除成功");
    }

    [HttpPut("{id}/status/{status}")]
    public async Task<ApiResponse> UpdateStatus(long id, int status)
    {
        await _userService.UpdateStatusAsync(id, status);
        return ApiResponse.Success("状态更新成功");
    }

    [HttpPut("{id}/password")]
    public async Task<ApiResponse> ChangePassword(long id, [FromBody] ChangePasswordRequest request)
    {
        await _userService.ChangePasswordAsync(id, request.OldPassword, request.NewPassword);
        return ApiResponse.Success("密码修改成功");
    }
}
