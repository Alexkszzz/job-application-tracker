using JobTracker.Api.Models;

namespace JobTracker.Api.Services.Interfaces;

public interface IApplicationService
{
    Task<IEnumerable<Application>> GetAllApplicationsAsync();
    Task<Application?> GetApplicationByIdAsync(Guid id);
    Task<Application> CreateApplicationAsync(Application application);
    Task<bool> UpdateApplicationAsync(Guid id, Application application);
    Task<bool> DeleteApplicationAsync(Guid id);
}
