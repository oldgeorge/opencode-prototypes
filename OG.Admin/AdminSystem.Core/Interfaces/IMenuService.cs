using AdminSystem.Core.DTOs;

namespace AdminSystem.Core.Interfaces;

public interface IMenuService
{
    Task<List<MenuDto>> GetAllAsync();
    Task<List<MenuDto>> GetTreeAsync();
    Task<MenuDto?> GetByIdAsync(long id);
    Task<MenuDto> CreateAsync(CreateMenuRequest request);
    Task<MenuDto> UpdateAsync(long id, UpdateMenuRequest request);
    Task DeleteAsync(long id);
    Task<List<MenuDto>> GetMenusByRoleIdAsync(long roleId);
}
