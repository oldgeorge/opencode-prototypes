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

    public async Task<PagedResult<RoleDto>> GetPageListAsync(string? keyword, int pageNum, int pageSize)
    {
        var query = _db.Queryable<SysRole>()
            .WhereIF(!string.IsNullOrEmpty(keyword), x => x.RoleName.Contains(keyword!) || x.RoleCode!.Contains(keyword!))
            .OrderBy(x => x.Sort);

        var total = await query.CountAsync();
        var list = await query.ToPageListAsync(pageNum, pageSize);

        var items = new List<RoleDto>();
        foreach (var role in list)
        {
            var menuIds = await _db.Queryable<SysRoleMenu>()
                .Where(x => x.RoleId == role.Id)
                .Select(x => x.MenuId)
                .ToListAsync();

            items.Add(new RoleDto
            {
                Id = role.Id,
                RoleName = role.RoleName,
                RoleCode = role.RoleCode,
                Sort = role.Sort,
                Status = role.Status,
                CreateTime = role.CreateTime,
                UpdateTime = role.UpdateTime,
                Remark = role.Remark,
                MenuIds = menuIds
            });
        }

        return new PagedResult<RoleDto>
        {
            Items = items,
            Total = total,
            PageNum = pageNum,
            PageSize = pageSize
        };
    }

    public async Task<List<RoleDto>> GetAllAsync()
    {
        var list = await _db.Queryable<SysRole>()
            .OrderBy(x => x.Sort)
            .ToListAsync();

        return list.Select(x => new RoleDto
        {
            Id = x.Id,
            RoleName = x.RoleName,
            RoleCode = x.RoleCode,
            Sort = x.Sort,
            Status = x.Status,
            CreateTime = x.CreateTime,
            UpdateTime = x.UpdateTime,
            Remark = x.Remark
        }).ToList();
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
            MenuIds = menuIds
        };
    }

    public async Task<RoleDto> CreateAsync(CreateRoleRequest request)
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

        await _db.Insertable(role).ExecuteCommandAsync();

        if (request.MenuIds != null && request.MenuIds.Count > 0)
        {
            var roleMenus = request.MenuIds.Select(menuId => new SysRoleMenu
            {
                RoleId = role.Id,
                MenuId = menuId
            }).ToList();

            await _db.Insertable(roleMenus).ExecuteCommandAsync();
        }

        return (await GetByIdAsync(role.Id))!;
    }

    public async Task<RoleDto> UpdateAsync(long id, UpdateRoleRequest request)
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

        return (await GetByIdAsync(id))!;
    }

    public async Task DeleteAsync(long id)
    {
        await _db.Deleteable<SysRoleMenu>()
            .Where(x => x.RoleId == id)
            .ExecuteCommandAsync();

        await _db.Deleteable<SysUserRole>()
            .Where(x => x.RoleId == id)
            .ExecuteCommandAsync();

        await _db.Deleteable<SysRole>()
            .Where(x => x.Id == id)
            .ExecuteCommandAsync();
    }

    public async Task<List<MenuDto>> GetRoleMenusAsync(long roleId)
    {
        var menuIds = await _db.Queryable<SysRoleMenu>()
            .Where(x => x.RoleId == roleId)
            .Select(x => x.MenuId)
            .ToListAsync();

        var menus = await _db.Queryable<SysMenu>()
            .Where(x => menuIds.Contains(x.Id))
            .OrderBy(x => x.Sort)
            .ToListAsync();

        return menus.Select(x => new MenuDto
        {
            Id = x.Id,
            MenuName = x.MenuName,
            MenuCode = x.MenuCode,
            MenuType = x.MenuType,
            Path = x.Path,
            Component = x.Component,
            Icon = x.Icon,
            ParentId = x.ParentId,
            Sort = x.Sort,
            Status = x.Status,
            IsVisible = x.IsVisible,
            IsCache = x.IsCache,
            IsAffix = x.IsAffix,
            IsKeepAlive = x.IsKeepAlive,
            CreateTime = x.CreateTime,
            UpdateTime = x.UpdateTime,
            Remark = x.Remark
        }).ToList();
    }

    public async Task<bool> AssignPermissionsAsync(long roleId, List<long> menuIds)
    {
        await _db.Deleteable<SysRoleMenu>()
            .Where(x => x.RoleId == roleId)
            .ExecuteCommandAsync();

        if (menuIds.Count > 0)
        {
            var roleMenus = menuIds.Select(menuId => new SysRoleMenu
            {
                RoleId = roleId,
                MenuId = menuId
            }).ToList();

            await _db.Insertable(roleMenus).ExecuteCommandAsync();
        }

        return true;
    }
}
