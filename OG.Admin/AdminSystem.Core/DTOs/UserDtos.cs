namespace AdminSystem.Core.DTOs;

public class UserDto
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public int Status { get; set; }
    public long? OrgId { get; set; }
    public string? OrgName { get; set; }
    public DateTime? CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public string? Remark { get; set; }
    public List<RoleSimpleDto>? Roles { get; set; }
}

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public int Status { get; set; } = 1;
    public long? OrgId { get; set; }
    public string? Remark { get; set; }
    public List<long>? RoleIds { get; set; }
}

public class UpdateUserRequest
{
    public string? Nickname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Avatar { get; set; }
    public int Status { get; set; } = 1;
    public long? OrgId { get; set; }
    public string? Remark { get; set; }
    public List<long>? RoleIds { get; set; }
}

public class ChangePasswordRequest
{
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

public class UserQueryRequest
{
    public string? Keyword { get; set; }
    public long? OrgId { get; set; }
    public int? Status { get; set; }
    public int PageNum { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public long Total { get; set; }
    public int PageNum { get; set; }
    public int PageSize { get; set; }
}
