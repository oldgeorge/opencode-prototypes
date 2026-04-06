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
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<ApiResponse<List<MenuDto>>> GetAll()
    {
        var result = await _menuService.GetAllAsync();
        return ApiResponse<List<MenuDto>>.Success(result);
    }

    [HttpGet("tree")]
    public async Task<ApiResponse<List<MenuDto>>> GetTree()
    {
        var result = await _menuService.GetTreeAsync();
        return ApiResponse<List<MenuDto>>.Success(result);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<MenuDto>> GetById(long id)
    {
        var result = await _menuService.GetByIdAsync(id);
        if (result == null)
        {
            return ApiResponse<MenuDto>.Fail("菜单不存在", 404);
        }
        return ApiResponse<MenuDto>.Success(result);
    }

    [HttpPost]
    public async Task<ApiResponse<long>> Create([FromBody] CreateMenuRequest request)
    {
        var id = await _menuService.CreateAsync(request);
        return ApiResponse<long>.Success(id, "创建成功");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Update(long id, [FromBody] UpdateMenuRequest request)
    {
        await _menuService.UpdateAsync(id, request);
        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        await _menuService.DeleteAsync(id);
        return ApiResponse.Success("删除成功");
    }

    [HttpGet("role/{roleId}")]
    public async Task<ApiResponse<List<MenuDto>>> GetMenusByRoleId(long roleId)
    {
        var result = await _menuService.GetMenusByRoleIdAsync(roleId);
        return ApiResponse<List<MenuDto>>.Success(result);
    }
}
