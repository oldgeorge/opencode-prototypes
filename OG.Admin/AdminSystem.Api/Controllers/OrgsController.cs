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
    public async Task<ApiResponse<long>> Create([FromBody] CreateOrgRequest request)
    {
        var id = await _orgService.CreateAsync(request);
        return ApiResponse<long>.Success(id, "创建成功");
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse> Update(long id, [FromBody] UpdateOrgRequest request)
    {
        await _orgService.UpdateAsync(id, request);
        return ApiResponse.Success("更新成功");
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse> Delete(long id)
    {
        await _orgService.DeleteAsync(id);
        return ApiResponse.Success("删除成功");
    }
}
