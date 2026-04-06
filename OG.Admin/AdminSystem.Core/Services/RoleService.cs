using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.Core.DTOs;
using AdminSystem.Core.Entities;
using AdminSystem.Core.Interfaces;
using SqlSugar;

namespace AdminSystem.Core.Services;

public class RoleService : IRoleService
{
    private readonly ISqlSugarClient _db;

    public RoleService(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<PageResult<RoleDto>> GetPageListAsync(string? keyword, int page, int size)
    {
        var query = _db.Queryable<SysRole>()
            .WhereIF(!string.IsNullOrEmpty(keyword), x => 
                x.RoleName.Contains(keyword!) || 
                (x.RoleCode != null && x.RoleCode.Contains(keyword!)))
            .OrderBy(x => x.Sort, OrderByType.Asc);

        var total = await query.CountAsync();
        var list = await query.ToPageListAsync(page, size);
        var items = list.Select(MapToDto).ToList();

        return new PageResult<RoleDto>
        {
            Items = items,
            Total = total,
            PageNum = page,
            PageSize = size
        };
    }

    public async Task<List<RoleDto>> GetAllAsync()
    {
        var list = await _db.Queryable<SysRole>()
            .OrderBy(x => x.Sort, OrderByType.Asc)
            .ToListAsync();

        return list.Select(MapToDto).ToList();
    }

    public async Task<RoleDto?> GetByIdAsync(long id)
    {
        var role = await _db.Queryable<SysRole>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (role == null)
        {
            return null;
        }

        var menuIds = await _db.Queryable<SysRoleMenu>()
            .Where(x => x.RoleId == id)
            .Select(x => x.MenuId)
            .ToListAsync();

        var menus = await _db.Queryable<SysMenu>()
            .Where(x => menuIds.Contains(x.Id))
            .ToListAsync();

        return new RoleDto
        {
            Id = role.Id,
            RoleName = role.RoleName,
            RoleCode = role.RoleCode,
            Sort = role.Sort,
            Status = role.Status,
            CreateTime = role.CreateTime,
            UpdateTime = role.UpdateTime,
            Remark = role.Remark,
            MenuIds = menuIds,
            Menus = menus.Select(m => new MenuSimpleDto
            {
                Id = m.Id,
                MenuName = m.MenuName
            }).ToList()
        };
    }

    public async Task<long> CreateAsync(CreateRoleRequest request)
    {
        if (!string.IsNullOrEmpty(request.RoleCode))
        {
            var exists = await _db.Queryable<SysRole>()
                .Where(x => x.RoleCode == request.RoleCode)
                .AnyAsync();

            if (exists)
            {
                throw new Exception("角色编码已存在");
            }
        }

        var role = new SysRole
        {
            RoleName = request.RoleName,
            RoleCode = request.RoleCode,
            Sort = request.Sort,
            Status = request.Status,
            CreateTime = DateTime.Now,
            Remark = request.Remark
        };

        var roleId = await _db.Insertable(role).ExecuteReturnSnowflakeIdAsync();

        if (request.MenuIds != null && request.MenuIds.Count > 0)
        {
            var roleMenus = request.MenuIds.Select(menuId => new SysRoleMenu
            {
                RoleId = roleId,
                MenuId = menuId
            }).ToList();

            await _db.Insertable(roleMenus).ExecuteCommandAsync();
        }

        return roleId;
    }

    public async Task UpdateAsync(long id, UpdateRoleRequest request)
    {
        var role = await _db.Queryable<SysRole>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (role == null)
        {
            throw new Exception("角色不存在");
        }

        if (!string.IsNullOrEmpty(request.RoleCode))
        {
            var exists = await _db.Queryable<SysRole>()
                .Where(x => x.RoleCode == request.RoleCode && x.Id != id)
                .AnyAsync();

            if (exists)
            {
                throw new Exception("角色编码已存在");
            }
        }

        role.RoleName = request.RoleName;
        role.RoleCode = request.RoleCode;
        role.Sort = request.Sort;
        role.Status = request.Status;
        role.UpdateTime = DateTime.Now;
        role.Remark = request.Remark;

        await _db.Updateable(role).ExecuteCommandAsync();

        await _db.Deleteable<SysRoleMenu>()
            .Where(x => x.RoleId == id)
            .ExecuteCommandAsync();

        if (request.MenuIds != null && request.MenuIds.Count > 0)
        {
            var roleMenus = request.MenuIds.Select(menuId => new SysRoleMenu
            {
                RoleId = id,
                MenuId = menuId
            }).ToList();

            await _db.Insertable(roleMenus).ExecuteCommandAsync();
        }
    }

    public async Task DeleteAsync(long id)
    {
        var role = await _db.Queryable<SysRole>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (role == null)
        {
            throw new Exception("角色不存在");
        }

        await _db.Deleteable<SysUserRole>()
            .Where(x => x.RoleId == id)
            .ExecuteCommandAsync();

        await _db.Deleteable<SysRoleMenu>()
            .Where(x => x.RoleId == id)
            .ExecuteCommandAsync();

        await _db.Deleteable<SysRole>()
            .Where(x => x.Id == id)
            .ExecuteCommandAsync();
    }

    public async Task AssignPermissionsAsync(long roleId, List<long> menuIds)
    {
        var role = await _db.Queryable<SysRole>()
            .Where(x => x.Id == roleId)
            .FirstAsync();

        if (role == null)
        {
            throw new Exception("角色不存在");
        }

        await _db.Deleteable<SysRoleMenu>()
            .Where(x => x.RoleId == roleId)
            .ExecuteCommandAsync();

        if (menuIds != null && menuIds.Count > 0)
        {
            var roleMenus = menuIds.Select(menuId => new SysRoleMenu
            {
                RoleId = roleId,
                MenuId = menuId
            }).ToList();

            await _db.Insertable(roleMenus).ExecuteCommandAsync();
        }
    }

    private RoleDto MapToDto(SysRole role)
    {
        return new RoleDto
        {
            Id = role.Id,
            RoleName = role.RoleName,
            RoleCode = role.RoleCode,
            Sort = role.Sort,
            Status = role.Status,
            CreateTime = role.CreateTime,
            UpdateTime = role.UpdateTime,
            Remark = role.Remark
        };
    }
}
