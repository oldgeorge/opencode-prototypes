using AdminSystem.Core.DTOs;

namespace AdminSystem.Core.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserDto>> GetPageListAsync(UserQueryRequest request);
    Task<UserDto?> GetByIdAsync(long id);
    Task<UserDto> CreateAsync(CreateUserRequest request);
    Task<UserDto> UpdateAsync(long id, UpdateUserRequest request);
    Task DeleteAsync(long id);
    Task<bool> UpdateStatusAsync(long id, int status);
    Task<bool> ChangePasswordAsync(long id, string oldPassword, string newPassword);
}
