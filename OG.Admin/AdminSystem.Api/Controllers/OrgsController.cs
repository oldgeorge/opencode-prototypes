using AdminSystem.Core.DTOs;
using AdminSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrgsController : ControllerBase
{
    private readonly IOrgService _orgService;

    public OrgsController(IOrgService orgService)
    {
        _orgService = orgService;
    }

    [HttpGet]
    public async Task<ApiResponse<List<OrgDto>>> GetAll()
    {
        var result = await _orgService.GetAllAsync();
        return ApiResponse<List<OrgDto>>.Success(result);
    }

    [HttpGet("tree")]
    public async Task<ApiResponse<List<OrgDto>>> GetTree()
    {
        var result = await _orgService.GetTreeAsync();
        return ApiResponse<List<OrgDto>>.Success(result);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<OrgDto>> GetById(long id)
    {
        var result = await _orgService.GetByIdAsync(id);

        if (result == null)
        {
            return ApiResponse<OrgDto>.Fail("组织不存在", 404);
        }

        return ApiResponse<OrgDto>.Success(result);
    }

    [HttpPost]
    public async Task<ApiResponse<OrgDto>> Create([FromBody] CreateOrgRequest request)
    {
        try
        {
            var result = await _orgService.CreateAsync(request);
            return ApiResponse<OrgDto>.Success(result, "创建成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<OrgDto>.Fail(ex.Message, 400);
        }
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<OrgDto>> Update(long id, [FromBody] UpdateOrgRequest request)
    {
        try
        {
            var result = await _orgService.UpdateAsync(id, request);
            return ApiResponse<OrgDto>.Success(result, "更新成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<OrgDto>.Fail(ex.Message, 400);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        try
        {
            await _orgService.DeleteAsync(id);
            return ApiResponse.Success("删除成功");
        }
        catch (Exception ex)
        {
            return ApiResponse.Fail(ex.Message, 400);
        }
    }
}
