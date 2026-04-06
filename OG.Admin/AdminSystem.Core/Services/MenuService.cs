using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.Core.DTOs;
using AdminSystem.Core.Entities;
using AdminSystem.Core.Interfaces;
using SqlSugar;

namespace AdminSystem.Core.Services;

public class MenuService : IMenuService
{
    private readonly ISqlSugarClient _db;

    public MenuService(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<List<MenuDto>> GetAllAsync()
    {
        var list = await _db.Queryable<SysMenu>()
            .OrderBy(x => x.Sort, OrderByType.Asc)
            .ToListAsync();

        return list.Select(MapToDto).ToList();
    }

    public async Task<List<MenuDto>> GetTreeAsync()
    {
        var list = await _db.Queryable<SysMenu>()
            .OrderBy(x => x.Sort, OrderByType.Asc)
            .ToListAsync();

        return BuildTree(list, 0);
    }

    public async Task<MenuDto?> GetByIdAsync(long id)
    {
        var menu = await _db.Queryable<SysMenu>()
            .Where(x => x.Id == id)
            .FirstAsync();

        return menu != null ? MapToDto(menu) : null;
    }

    public async Task<long> CreateAsync(CreateMenuRequest request)
    {
        var menu = new SysMenu
        {
            MenuName = request.MenuName,
            MenuCode = request.MenuCode,
            MenuType = request.MenuType,
            Path = request.Path,
            Component = request.Component,
            Icon = request.Icon,
            ParentId = request.ParentId,
            Sort = request.Sort,
            Status = request.Status,
            IsVisible = request.IsVisible,
            IsCache = request.IsCache,
            IsAffix = request.IsAffix,
            IsKeepAlive = request.IsKeepAlive,
            CreateTime = DateTime.Now,
            Remark = request.Remark
        };

        var menuId = await _db.Insertable(menu).ExecuteReturnSnowflakeIdAsync();
        return menuId;
    }

    public async Task UpdateAsync(long id, UpdateMenuRequest request)
    {
        var menu = await _db.Queryable<SysMenu>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (menu == null)
        {
            throw new Exception("菜单不存在");
        }

        menu.MenuName = request.MenuName;
        menu.MenuCode = request.MenuCode;
        menu.MenuType = request.MenuType;
        menu.Path = request.Path;
        menu.Component = request.Component;
        menu.Icon = request.Icon;
        menu.ParentId = request.ParentId;
        menu.Sort = request.Sort;
        menu.Status = request.Status;
        menu.IsVisible = request.IsVisible;
        menu.IsCache = request.IsCache;
        menu.IsAffix = request.IsAffix;
        menu.IsKeepAlive = request.IsKeepAlive;
        menu.UpdateTime = DateTime.Now;
        menu.Remark = request.Remark;

        await _db.Updateable(menu).ExecuteCommandAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var menu = await _db.Queryable<SysMenu>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (menu == null)
        {
            throw new Exception("菜单不存在");
        }

        var hasChildren = await _db.Queryable<SysMenu>()
            .Where(x => x.ParentId == id)
            .AnyAsync();

        if (hasChildren)
        {
            throw new Exception("请先删除子菜单");
        }

        await _db.Deleteable<SysRoleMenu>()
            .Where(x => x.MenuId == id)
            .ExecuteCommandAsync();

        await _db.Deleteable<SysMenu>()
            .Where(x => x.Id == id)
            .ExecuteCommandAsync();
    }

    public async Task<List<MenuDto>> GetMenusByRoleIdAsync(long roleId)
    {
        var menuIds = await _db.Queryable<SysRoleMenu>()
            .Where(x => x.RoleId == roleId)
            .Select(x => x.MenuId)
            .ToListAsync();

        var menus = await _db.Queryable<SysMenu>()
            .Where(x => menuIds.Contains(x.Id))
            .ToListAsync();

        return menus.Select(MapToDto).ToList();
    }

    private List<MenuDto> BuildTree(List<SysMenu> list, long parentId)
    {
        return list
            .Where(x => x.ParentId == parentId || (x.ParentId == null && parentId == 0))
            .OrderBy(x => x.Sort)
            .Select(x => new MenuDto
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
                Remark = x.Remark,
                Children = BuildTree(list, x.Id)
            })
            .ToList();
    }

    private MenuDto MapToDto(SysMenu menu)
    {
        return new MenuDto
        {
            Id = menu.Id,
            MenuName = menu.MenuName,
            MenuCode = menu.MenuCode,
            MenuType = menu.MenuType,
            Path = menu.Path,
            Component = menu.Component,
            Icon = menu.Icon,
            ParentId = menu.ParentId,
            Sort = menu.Sort,
            Status = menu.Status,
            IsVisible = menu.IsVisible,
            IsCache = menu.IsCache,
            IsAffix = menu.IsAffix,
            IsKeepAlive = menu.IsKeepAlive,
            CreateTime = menu.CreateTime,
            UpdateTime = menu.UpdateTime,
            Remark = menu.Remark
        };
    }
}
