using SqlSugar;

namespace AdminSystem.Core.Entities;

[SugarTable("sys_role")]
public class SysRole
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    [SugarColumn(Length = 50)]
    public string RoleName { get; set; } = string.Empty;

    [SugarColumn(Length = 50)]
    public string? RoleCode { get; set; }

    public int Sort { get; set; }

    public int Status { get; set; } = 1;

    public DateTime? CreateTime { get; set; } = DateTime.Now;

    public DateTime? UpdateTime { get; set; }

    [SugarColumn(Length = 500)]
    public string? Remark { get; set; }

    [Navigate(NavigateType.ManyToMany, typeof(SysRoleMenu))]
    public List<SysMenu>? Menus { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<long>? MenuIds { get; set; }
}
