using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminSystem.Core.DTOs;

namespace AdminSystem.Core.Interfaces;

public interface IUserService
{
    Task<PageResult<UserDto>> GetPageListAsync(UserQueryRequest request);
    Task<UserDto?> GetByIdAsync(long id);
    Task<long> CreateAsync(CreateUserRequest request);
    Task UpdateAsync(long id, UpdateUserRequest request);
    Task DeleteAsync(long id);
    Task UpdateStatusAsync(long id, int status);
    Task ChangePasswordAsync(long id, string oldPassword, string newPassword);
}
