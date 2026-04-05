using SqlSugar;

namespace AdminSystem.Core.Entities;

[SugarTable("sys_org")]
public class SysOrg
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    [SugarColumn(Length = 50)]
    public string OrgName { get; set; } = string.Empty;

    [SugarColumn(Length = 50)]
    public string? OrgCode { get; set; }

    public long? ParentId { get; set; }

    public int Sort { get; set; }

    public int Status { get; set; } = 1;

    public DateTime? CreateTime { get; set; } = DateTime.Now;

    public DateTime? UpdateTime { get; set; }

    [SugarColumn(Length = 500)]
    public string? Remark { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<SysOrg>? Children { get; set; }
}
