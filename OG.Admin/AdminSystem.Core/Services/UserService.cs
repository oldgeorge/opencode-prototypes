using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.Core.DTOs;
using AdminSystem.Core.Entities;
using AdminSystem.Core.Interfaces;
using SqlSugar;

namespace AdminSystem.Core.Services;

public class UserService : IUserService
{
    private readonly ISqlSugarClient _db;

    public UserService(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<PageResult<UserDto>> GetPageListAsync(UserQueryRequest request)
    {
        var query = _db.Queryable<SysUser>()
            .Includes(x => x.Roles)
            .WhereIF(!string.IsNullOrEmpty(request.Keyword), x => 
                x.Username.Contains(request.Keyword!) || 
                (x.Nickname != null && x.Nickname.Contains(request.Keyword!)))
            .WhereIF(request.OrgId.HasValue, x => x.OrgId == request.OrgId)
            .WhereIF(request.Status.HasValue, x => x.Status == request.Status)
            .OrderBy(x => x.CreateTime, OrderByType.Desc);

        var total = await query.CountAsync();
        var list = await query.ToPageListAsync(request.PageNum, request.PageSize);
        var items = list.Select(MapToDto).ToList();

        return new PageResult<UserDto>
        {
            Items = items,
            Total = total,
            PageNum = request.PageNum,
            PageSize = request.PageSize
        };
    }

    public async Task<UserDto?> GetByIdAsync(long id)
    {
        var user = await _db.Queryable<SysUser>()
            .Includes(x => x.Roles)
            .Where(x => x.Id == id)
            .FirstAsync();

        return user != null ? MapToDto(user) : null;
    }

    public async Task<long> CreateAsync(CreateUserRequest request)
    {
        var exists = await _db.Queryable<SysUser>()
            .Where(x => x.Username == request.Username)
            .AnyAsync();

        if (exists)
        {
            throw new Exception("用户名已存在");
        }

        var user = new SysUser
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Nickname = request.Nickname,
            Email = request.Email,
            Phone = request.Phone,
            Avatar = request.Avatar,
            Status = request.Status,
            OrgId = request.OrgId,
            CreateTime = DateTime.Now,
            Remark = request.Remark
        };

        var userId = await _db.Insertable(user).ExecuteReturnSnowflakeIdAsync();

        if (request.RoleIds != null && request.RoleIds.Count > 0)
        {
            var userRoles = request.RoleIds.Select(roleId => new SysUserRole
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();

            await _db.Insertable(userRoles).ExecuteCommandAsync();
        }

        return userId;
    }

    public async Task UpdateAsync(long id, UpdateUserRequest request)
    {
        var user = await _db.Queryable<SysUser>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (user == null)
        {
            throw new Exception("用户不存在");
        }

        user.Nickname = request.Nickname;
        user.Email = request.Email;
        user.Phone = request.Phone;
        user.Avatar = request.Avatar;
        user.Status = request.Status;
        user.OrgId = request.OrgId;
        user.UpdateTime = DateTime.Now;
        user.Remark = request.Remark;

        await _db.Updateable(user).ExecuteCommandAsync();

        await _db.Deleteable<SysUserRole>()
            .Where(x => x.UserId == id)
            .ExecuteCommandAsync();

        if (request.RoleIds != null && request.RoleIds.Count > 0)
        {
            var userRoles = request.RoleIds.Select(roleId => new SysUserRole
            {
                UserId = id,
                RoleId = roleId
            }).ToList();

            await _db.Insertable(userRoles).ExecuteCommandAsync();
        }
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _db.Queryable<SysUser>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (user == null)
        {
            throw new Exception("用户不存在");
        }

        await _db.Deleteable<SysUserRole>()
            .Where(x => x.UserId == id)
            .ExecuteCommandAsync();

        await _db.Deleteable<SysUser>()
            .Where(x => x.Id == id)
            .ExecuteCommandAsync();
    }

    public async Task UpdateStatusAsync(long id, int status)
    {
        var user = await _db.Queryable<SysUser>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (user == null)
        {
            throw new Exception("用户不存在");
        }

        user.Status = status;
        user.UpdateTime = DateTime.Now;

        await _db.Updateable(user)
            .UpdateColumns(x => new { x.Status, x.UpdateTime })
            .ExecuteCommandAsync();
    }

    public async Task ChangePasswordAsync(long id, string oldPassword, string newPassword)
    {
        var user = await _db.Queryable<SysUser>()
            .Where(x => x.Id == id)
            .FirstAsync();

        if (user == null)
        {
            throw new Exception("用户不存在");
        }

        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
        {
            throw new Exception("原密码错误");
        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.UpdateTime = DateTime.Now;

        await _db.Updateable(user)
            .UpdateColumns(x => new { x.Password, x.UpdateTime })
            .ExecuteCommandAsync();
    }

    private UserDto MapToDto(SysUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Nickname = user.Nickname,
            Email = user.Email,
            Phone = user.Phone,
            Avatar = user.Avatar,
            Status = user.Status,
            OrgId = user.OrgId,
            CreateTime = user.CreateTime,
            UpdateTime = user.UpdateTime,
            Remark = user.Remark,
            Roles = user.Roles?.Select(r => new RoleSimpleDto
            {
                Id = r.Id,
                RoleName = r.RoleName,
                RoleCode = r.RoleCode
            }).ToList()
        };
    }
}
