using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminSystem.Core.DTOs;
using AdminSystem.Core.Entities;
using AdminSystem.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;
using SqlSugar;

namespace AdminSystem.Core.Services;

public class AuthService : IAuthService
{
    private readonly ISqlSugarClient _db;

    public AuthService(ISqlSugarClient db)
    {
        _db = db;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Queryable<SysUser>()
            .Where(u => u.Username == request.Username)
            .FirstAsync();

        if (user == null)
        {
            throw new Exception("用户名或密码错误");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            throw new Exception("用户名或密码错误");
        }

        if (user.Status != 1)
        {
            throw new Exception("账号已被禁用");
        }

        var token = GenerateToken(user);
        var expireIn = request.RememberMe ? 7 * 24 * 60 * 60 : 2 * 60 * 60;

        return new LoginResponse
        {
            Token = token,
            User = MapToUserDto(user),
            ExpireIn = expireIn
        };
    }

    public async Task<CurrentUserResponse?> GetCurrentUserAsync(long userId)
    {
        var user = await _db.Queryable<SysUser>()
            .Includes(x => x.Roles)
            .Where(x => x.Id == userId)
            .FirstAsync();

        if (user == null)
        {
            return null;
        }

        var permissions = new List<string>();
        var roles = new List<string>();

        if (user.Roles != null)
        {
            foreach (var role in user.Roles)
            {
                roles.Add(role.RoleCode ?? role.RoleName);
            }
        }

        return new CurrentUserResponse
        {
            User = MapToUserDto(user),
            Permissions = permissions,
            Roles = roles
        };
    }

    public string GenerateToken(SysUser user)
    {
        var secretKey = "YourSecretKeyHere12345678901234567890";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("nickname", user.Nickname ?? user.Username)
        };
        var token = new JwtSecurityToken(
            issuer: "AdminSystem",
            audience: "AdminSystem",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private UserDto MapToUserDto(SysUser user)
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
