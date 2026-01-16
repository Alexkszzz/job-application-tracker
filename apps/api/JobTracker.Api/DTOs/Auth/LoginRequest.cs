using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.DTOs.Auth;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    public required string Email { get; set; }
    
    [Required]
    [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
    public required string Password { get; set; }
}
