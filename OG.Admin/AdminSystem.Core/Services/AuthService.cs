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
    private readonly JwtSettings _jwtSettings;

    public AuthService(ISqlSugarClient db, JwtSettings jwtSettings)
    {
        _db = db;
        _jwtSettings = jwtSettings;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Queryable<SysUser>()
            .Where(x => x.Username == request.Username)
            .FirstAsync();

        if (user == null)
        {
            throw new Exception("用户名或密码错误");
        }

        var passwordHash = HashPassword(request.Password);
        if (user.Password != passwordHash)
        {
            throw new Exception("用户名或密码错误");
        }

        if (user.Status != 1)
        {
            throw new Exception("账号已被禁用");
        }

        var token = GenerateToken(user);
        var expireIn = (int)TimeSpan.FromDays(_jwtSettings.ExpireDays).TotalSeconds;

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
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("nickname", user.Nickname ?? user.Username)
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpireDays),
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
            Remark = user.Remark
        };
    }

    private string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

public class JwtSettings
{
    public string SecretKey { get; set; } = "YourSecretKeyForJwtTokenMustBeAtLeast32CharactersLong!";
    public string Issuer { get; set; } = "AdminSystem";
    public string Audience { get; set; } = "AdminSystem";
    public int ExpireDays { get; set; } = 7;
}
