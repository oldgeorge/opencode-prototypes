namespace AdminSystem.Core.DTOs;

public class JwtSettings
{
    public string SecretKey { get; set; } = "YourSecretKeyForJwtTokenMustBeAtLeast32CharactersLong!";
    public string Issuer { get; set; } = "AdminSystem";
    public string Audience { get; set; } = "AdminSystem";
    public int ExpireDays { get; set; } = 7;
}
