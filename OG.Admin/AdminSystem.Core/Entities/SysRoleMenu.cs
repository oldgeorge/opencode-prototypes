using System;
using System.Collections.Generic;
using SqlSugar;

namespace AdminSystem.Core.Entities;

[SugarTable("sys_role_menu")]
public class SysRoleMenu
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long MenuId { get; set; }
}
