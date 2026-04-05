namespace AdminSystem.Core.DTOs;

public class MenuDto
{
    public long Id { get; set; }
    public string MenuName { get; set; } = string.Empty;
    public string? MenuCode { get; set; }
    public int MenuType { get; set; }
    public string? Path { get; set; }
    public string? Component { get; set; }
    public string? Icon { get; set; }
    public long? ParentId { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; }
    public bool IsVisible { get; set; }
    public bool IsCache { get; set; }
    public bool IsAffix { get; set; }
    public bool IsKeepAlive { get; set; }
    public DateTime? CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? Remark { get; set; }
    public List<MenuDto>? Children { get; set; }
}

public class CreateMenuRequest
{
    public string MenuName { get; set; } = string.Empty;
    public string? MenuCode { get; set; }
    public int MenuType { get; set; } = 1;
    public string? Path { get; set; }
    public string? Component { get; set; }
    public string? Icon { get; set; }
    public long? ParentId { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; } = 1;
    public bool IsVisible { get; set; } = true;
    public bool IsCache { get; set; } = false;
    public bool IsAffix { get; set; } = false;
    public bool IsKeepAlive { get; set; } = true;
    public string? Remark { get; set; }
}

public class UpdateMenuRequest
{
    public string MenuName { get; set; } = string.Empty;
    public string? MenuCode { get; set; }
    public int MenuType { get; set; } = 1;
    public string? Path { get; set; }
    public string? Component { get; set; }
    public string? Icon { get; set; }
    public long? ParentId { get; set; }
    public int Sort { get; set; }
    public int Status { get; set; } = 1;
    public bool IsVisible { get; set; } = true;
    public bool IsCache { get; set; } = false;
    public bool IsAffix { get; set; } = false;
    public bool IsKeepAlive { get; set; } = true;
    public string? Remark { get; set; }
}
