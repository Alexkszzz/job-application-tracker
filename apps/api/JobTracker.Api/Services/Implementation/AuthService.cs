using JobTracker.Api.DTOs.Auth;
using JobTracker.Api.Models;
using JobTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobTracker.Api.Services.Implementation;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = new List<string> { "User with this email already exists." }
            };
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        var token = await GenerateTokenAsync(user);
        return new AuthResult
        {
            Succeeded = true,
            Response = token
        };
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = new List<string> { "Invalid email or password." }
            };
        }

        // Check if account is locked out
        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            var remainingMinutes = lockoutEnd.HasValue ? (int)(lockoutEnd.Value - DateTimeOffset.UtcNow).TotalMinutes : 0;
            return new AuthResult
            {
                Succeeded = false,
                Errors = new List<string> { $"Account is locked due to multiple failed login attempts. Please try again in {remainingMinutes} minute(s)." }
            };
        }

        // Use SignInManager to properly track failed attempts
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent: false, lockoutOnFailure: true);
        
        if (result.IsLockedOut)
        {
            return new AuthResult
            {
                Succeeded = false,
                Errors = new List<string> { "Account locked due to multiple failed login attempts. Please try again in 15 minutes." }
            };
        }
        
        if (!result.Succeeded)
        {
            // Get remaining attempts before lockout
            var accessFailedCount = await _userManager.GetAccessFailedCountAsync(user);
            var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
            var remainingAttempts = maxAttempts - accessFailedCount;
            
            return new AuthResult
            {
                Succeeded = false,
                Errors = new List<string> { $"Invalid email or password. {remainingAttempts} attempt(s) remaining before account lockout." }
            };
        }

        var token = await GenerateTokenAsync(user);
        return new AuthResult
        {
            Succeeded = true,
            Response = token
        };
    }

    private async Task<AuthResponse> GenerateTokenAsync(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expiryInMinutes = int.Parse(jwtSettings["ExpiryInMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (!string.IsNullOrEmpty(user.FirstName))
        {
            claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
        }

        if (!string.IsNullOrEmpty(user.LastName))
        {
            claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(expiryInMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponse
        {
            Token = tokenString,
            Email = user.Email!,
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Expiration = expiration
        };
    }
}
