using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminSystem.Core.DTOs;

namespace AdminSystem.Core.Interfaces;

public interface IOrgService
{
    Task<List<OrgDto>> GetAllAsync();
    Task<List<OrgDto>> GetTreeAsync();
    Task<OrgDto?> GetByIdAsync(long id);
    Task<long> CreateAsync(CreateOrgRequest request);
    Task UpdateAsync(long id, UpdateOrgRequest request);
    Task DeleteAsync(long id);
}
