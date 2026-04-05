using SqlSugar;

namespace AdminSystem.Core.Entities;

[SugarTable("sys_user_role")]
public class SysUserRole
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    public long UserId { get; set; }

    public long RoleId { get; set; }
}
