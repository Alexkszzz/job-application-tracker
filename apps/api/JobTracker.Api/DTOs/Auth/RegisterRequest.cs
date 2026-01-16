using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    public required string Email { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
    public required string Password { get; set; }
    
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string? FirstName { get; set; }
    
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string? LastName { get; set; }
}
