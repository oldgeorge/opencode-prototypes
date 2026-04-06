using System;
using System.Collections.Generic;
namespace AdminSystem.Core.DTOs;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; } = false;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
    public int ExpireIn { get; set; }
}

public class CurrentUserResponse
{
    public UserDto User { get; set; } = null!;
    public List<string> Permissions { get; set; } = new();
    public List<string> Roles { get; set; } = new();
}
