using SqlSugar;

namespace AdminSystem.Core.Entities;

[SugarTable("sys_menu")]
public class SysMenu
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    [SugarColumn(Length = 50)]
    public string MenuName { get; set; } = string.Empty;

    [SugarColumn(Length = 50)]
    public string? MenuCode { get; set; }

    public int MenuType { get; set; }

    [SugarColumn(Length = 200)]
    public string? Path { get; set; }

    [SugarColumn(Length = 200)]
    public string? Component { get; set; }

    [SugarColumn(Length = 50)]
    public string? Icon { get; set; }

    public long? ParentId { get; set; }

    public int Sort { get; set; }

    public int Status { get; set; } = 1;

    public bool IsVisible { get; set; } = true;

    public bool IsCache { get; set; } = false;

    public bool IsAffix { get; set; } = false;

    public bool IsKeepAlive { get; set; } = true;

    public DateTime? CreateTime { get; set; } = DateTime.Now;

    public DateTime? UpdateTime { get; set; }

    [SugarColumn(Length = 500)]
    public string? Remark { get; set; }

    [SugarColumn(IsIgnore = true)]
    public List<SysMenu>? Children { get; set; }
}
