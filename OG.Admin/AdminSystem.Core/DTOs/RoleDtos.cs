using System;
using System.Collections.Generic;
namespace AdminSystem.Core.DTOs;

public class RoleDto
{
    public long Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? RoleCode { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; }
    public DateTime? CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? Remark { get; set; }
    public List<MenuSimpleDto>? Menus { get; set; }
    public List<long>? MenuIds { get; set; }
}

public class RoleSimpleDto
{
    public long Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? RoleCode { get; set; }
}

public class MenuSimpleDto
{
    public long Id { get; set; }
    public string MenuName { get; set; } = string.Empty;
}

public class CreateRoleRequest
{
    public string RoleName { get; set; } = string.Empty;
    public string? RoleCode { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; } = 1;
    public string? Remark { get; set; }
    public List<long>? MenuIds { get; set; }
}

public class UpdateRoleRequest
{
    public string RoleName { get; set; } = string.Empty;
    public string? RoleCode { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; } = 1;
    public string? Remark { get; set; }
    public List<long>? MenuIds { get; set; }
}

public class AssignPermissionsRequest
{
    public List<long> MenuIds { get; set; } = new();
}
