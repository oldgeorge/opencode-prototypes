using System;
using System.Collections.Generic;
using SqlSugar;

namespace AdminSystem.Core.Entities;

[SugarTable("sys_user")]
public class SysUser
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    [SugarColumn(Length = 50)]
    public string Username { get; set; } = string.Empty;

    [SugarColumn(Length = 255)]
    public string Password { get; set; } = string.Empty;

    [SugarColumn(Length = 50)]
    public string? Nickname { get; set; }

    [SugarColumn(Length = 100)]
    public string? Email { get; set; }

    [SugarColumn(Length = 20)]
    public string? Phone { get; set; }

    [SugarColumn(Length = 500)]
    public string? Avatar { get; set; }

    public int Status { get; set; } = 1;

    public long? OrgId { get; set; }

    public DateTime? CreateTime { get; set; } = DateTime.Now;

    public DateTime? UpdateTime { get; set; }

    public string? Remark { get; set; }

    [Navigate(NavigateType.ManyToMany, "SysUserRole")]
    public List<SysRole>? Roles { get; set; }
}
