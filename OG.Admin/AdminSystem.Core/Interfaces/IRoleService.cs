using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminSystem.Core.DTOs;

namespace AdminSystem.Core.Interfaces;

public interface IRoleService
{
    Task<PageResult<RoleDto>> GetPageListAsync(string? keyword, int page, int size);
    Task<List<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(long id);
    Task<long> CreateAsync(CreateRoleRequest request);
    Task UpdateAsync(long id, UpdateRoleRequest request);
    Task DeleteAsync(long id);
    Task AssignPermissionsAsync(long roleId, List<long> menuIds);
}
