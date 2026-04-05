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
    public async Task<ApiResponse<MenuDto>> Create([FromBody] CreateMenuRequest request)
    {
        try
        {
            var result = await _menuService.CreateAsync(request);
            return ApiResponse<MenuDto>.Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<MenuDto>.Fail(ex.Message, 400);
        }
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<MenuDto>> Update(long id, [FromBody] UpdateMenuRequest request)
    {
        try
        {
            var result = await _menuService.UpdateAsync(id, request);
            return ApiResponse<MenuDto>.Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<MenuDto>.Fail(ex.Message, 400);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        try
        {
            await _menuService.DeleteAsync(id);
            return ApiResponse.Success("删除成功");
        }
        catch (Exception ex)
        {
            return ApiResponse.Fail(ex.Message, 400);
        }
    }

    [HttpGet("role/{roleId}")]
    public async Task<ApiResponse<List<MenuDto>>> GetMenusByRoleId(long roleId)
    {
        var result = await _menuService.GetMenusByRoleIdAsync(roleId);
        return ApiResponse<List<MenuDto>>.Success(result);
    }
}
