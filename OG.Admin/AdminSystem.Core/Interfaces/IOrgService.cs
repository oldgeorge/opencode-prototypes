using AdminSystem.Core.DTOs;

namespace AdminSystem.Core.Interfaces;

public interface IOrgService
{
    Task<List<OrgDto>> GetAllAsync();
    Task<List<OrgDto>> GetTreeAsync();
    Task<OrgDto?> GetByIdAsync(long id);
    Task<OrgDto> CreateAsync(CreateOrgRequest request);
    Task<OrgDto> UpdateAsync(long id, UpdateOrgRequest request);
    Task DeleteAsync(long id);
}
