using System.Collections.Generic;
using System.Threading.Tasks;
using AdminSystem.Core.DTOs;
using AdminSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ApiResponse<PageResult<RoleDto>>> GetPageList(
        [FromQuery] string? keyword,
        [FromQuery] int pageNum = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _roleService.GetPageListAsync(keyword, pageNum, pageSize);
        return ApiResponse<PageResult<RoleDto>>.Success(result);
    }

    [HttpGet("all")]
    public async Task<ApiResponse<List<RoleDto>>> GetAll()
    {
        var result = await _roleService.GetAllAsync();
        return ApiResponse<List<RoleDto>>.Success(result);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<RoleDto>> GetById(long id)
    {
        var result = await _roleService.GetByIdAsync(id);
        if (result == null)
        {
            return ApiResponse<RoleDto>.Fail("角色不存在", 404);
        }
        return ApiResponse<RoleDto>.Success(result);
    }

    [HttpPost]
    public async Task<ApiResponse<long>> Create([FromBody] CreateRoleRequest request)
    {
        var id = await _roleService.CreateAsync(request);
        return ApiResponse<long>.Success(id, "创建成功");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Update(long id, [FromBody] UpdateRoleRequest request)
    {
        await _roleService.UpdateAsync(id, request);
        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        await _roleService.DeleteAsync(id);
        return ApiResponse.Success("删除成功");
    }

    [HttpPut("{id}/menus")]
    public async Task<ApiResponse> AssignPermissions(long id, [FromBody] AssignPermissionsRequest request)
    {
        await _roleService.AssignPermissionsAsync(id, request.MenuIds);
        return ApiResponse.Success("权限分配成功");
    }
}
