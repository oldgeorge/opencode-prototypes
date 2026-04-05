using AdminSystem.Core.DTOs;

namespace AdminSystem.Core.Interfaces;

public interface IRoleService
{
    Task<PagedResult<RoleDto>> GetPageListAsync(string? keyword, int pageNum, int pageSize);
    Task<List<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(long id);
    Task<RoleDto> CreateAsync(CreateRoleRequest request);
    Task<RoleDto> UpdateAsync(long id, UpdateRoleRequest request);
    Task DeleteAsync(long id);
    Task<List<MenuDto>> GetRoleMenusAsync(long roleId);
    Task<bool> AssignPermissionsAsync(long roleId, List<long> menuIds);
}
