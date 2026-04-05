using AdminSystem.Core.DTOs;
using AdminSystem.Core.Entities;
using AdminSystem.Core.Interfaces;
using SqlSugar;

namespace AdminSystem.Core.Services;

public class OrgService : IOrgService
{
    private readonly ISqlSugarClient _db;

    public OrgService(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<List<OrgDto>> GetAllAsync()
    {
        var list = await _db.Queryable<SysOrg>()
            .OrderBy(x => x.Sort)
            .ToListAsync();

        return list.Select(MapToDto).ToList();
    }

    public async Task<List<OrgDto>> GetTreeAsync()
    {
        var list = await _db.Queryable<SysOrg>()
            .OrderBy(x => x.Sort)
            .ToListAsync();

        return BuildTree(list, 0);
    }

    public async Task<OrgDto?> GetByIdAsync(long id)
    {
        var org = await _db.Queryable<SysOrg>()
            .Where(x => x.Id == id)
            .FirstAsync();

        return org != null ? MapToDto(org) : null;
    }

    public async Task<OrgDto> CreateAsync(CreateOrgRequest request)
    {
        var org = new SysOrg
        {
            OrgName = request.OrgName,
            OrgCode = request.OrgCode,
            ParentId = request.ParentId,
            Sort = request.Sort,
            Status = request.Status,
            CreateTime = DateTime.Now,
            Remark = request.Remark
        };

        await _db.Insertable(org).ExecuteCommandAsync();
        return (await GetByIdAsync(org.Id))!;
    }

    public async Task<OrgDto> UpdateAsync(long id, UpdateOrgRequest request)
    {
        var org = await _db.Queryable<SysOrg>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (org == null)
        {
            throw new Exception("组织不存在");
        }

        org.OrgName = request.OrgName;
        org.OrgCode = request.OrgCode;
        org.ParentId = request.ParentId;
        org.Sort = request.Sort;
        org.Status = request.Status;
        org.UpdateTime = DateTime.Now;
        org.Remark = request.Remark;

        await _db.Updateable(org).ExecuteCommandAsync();
        return (await GetByIdAsync(id))!;
    }

    public async Task DeleteAsync(long id)
    {
        var hasChildren = await _db.Queryable<SysOrg>()
            .Where(x => x.ParentId == id)
            .AnyAsync();

        if (hasChildren)
        {
            throw new Exception("请先删除子组织");
        }

        var hasUsers = await _db.Queryable<SysUser>()
            .Where(x => x.OrgId == id)
            .AnyAsync();

        if (hasUsers)
        {
            throw new Exception("该组织下存在用户，无法删除");
        }

        await _db.Deleteable<SysOrg>()
            .Where(x => x.Id == id)
            .ExecuteCommandAsync();
    }

    private List<OrgDto> BuildTree(List<SysOrg> list, long parentId)
    {
        return list
            .Where(x => x.ParentId == parentId)
            .Select(x => new OrgDto
            {
                Id = x.Id,
                OrgName = x.OrgName,
                OrgCode = x.OrgCode,
                ParentId = x.ParentId,
                Sort = x.Sort,
                Status = x.Status,
                CreateTime = x.CreateTime,
                UpdateTime = x.UpdateTime,
                Remark = x.Remark,
                Children = BuildTree(list, x.Id)
            })
            .ToList();
    }

    private OrgDto MapToDto(SysOrg org)
    {
        return new OrgDto
        {
            Id = org.Id,
            OrgName = org.OrgName,
            OrgCode = org.OrgCode,
            ParentId = org.ParentId,
            Sort = org.Sort,
            Status = org.Status,
            CreateTime = org.CreateTime,
            UpdateTime = org.UpdateTime,
            Remark = org.Remark
        };
    }
}
