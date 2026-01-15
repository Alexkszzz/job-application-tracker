namespace JobTracker.Api.DTOs.Auth;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public AuthResponse? Response { get; set; }
    public List<string> Errors { get; set; } = new();
}
