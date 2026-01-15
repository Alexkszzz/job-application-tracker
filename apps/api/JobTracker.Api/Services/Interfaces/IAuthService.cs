using JobTracker.Api.DTOs.Auth;

namespace JobTracker.Api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task<AuthResult> LoginAsync(LoginRequest request);
}
