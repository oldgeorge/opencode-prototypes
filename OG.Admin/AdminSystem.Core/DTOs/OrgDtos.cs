using System;
using System.Collections.Generic;
namespace AdminSystem.Core.DTOs;

public class OrgDto
{
    public long Id { get; set; }
    public string OrgName { get; set; } = string.Empty;
    public string? OrgCode { get; set; }
    public long? ParentId { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; }
    public DateTime? CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? Remark { get; set; }
    public List<OrgDto>? Children { get; set; }
}

public class CreateOrgRequest
{
    public string OrgName { get; set; } = string.Empty;
    public string? OrgCode { get; set; }
    public long? ParentId { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; } = 1;
    public string? Remark { get; set; }
}

public class UpdateOrgRequest
{
    public string OrgName { get; set; } = string.Empty;
    public string? OrgCode { get; set; }
    public long? ParentId { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; } = 1;
    public string? Remark { get; set; }
}
