namespace JobTracker.Api.DTOs.Auth;

public class AuthResponse
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required string UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime Expiration { get; set; }
}
