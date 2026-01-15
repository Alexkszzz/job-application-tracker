using JobTracker.Api.Models;

namespace JobTracker.Api.Services.Interfaces;

public interface IApplicationService
{
    Task<IEnumerable<Application>> GetAllApplicationsAsync(string userId);
    Task<Application?> GetApplicationByIdAsync(Guid id, string userId);
    Task<Application> CreateApplicationAsync(Application application);
    Task<bool> UpdateApplicationAsync(Guid id, Application application, string userId);
    Task<bool> DeleteApplicationAsync(Guid id, string userId);
}
