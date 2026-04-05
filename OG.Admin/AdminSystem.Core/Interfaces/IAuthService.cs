using AdminSystem.Core.DTOs;
using AdminSystem.Core.Entities;

namespace AdminSystem.Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<CurrentUserResponse?> GetCurrentUserAsync(long userId);
    string GenerateToken(SysUser user);
}
