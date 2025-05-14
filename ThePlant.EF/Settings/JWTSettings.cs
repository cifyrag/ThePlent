namespace ThePlant.EF.Settings;

public class JWTSettings
{
    public string ValidIssuer { get; set; } = string.Empty;
    public string IssuerSigningKey { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
}