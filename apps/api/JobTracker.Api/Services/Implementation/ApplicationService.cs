using JobTracker.Api.Data;
using JobTracker.Api.Models;
using JobTracker.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Services.Implementation;

public class ApplicationService : IApplicationService
{
    private readonly JobTrackerDbContext _context;

    public ApplicationService(JobTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
    {
        return await _context.Applications.ToListAsync();
    }

    public async Task<Application?> GetApplicationByIdAsync(Guid id)
    {
        return await _context.Applications.FindAsync(id);
    }

    public async Task<Application> CreateApplicationAsync(Application application)
    {
        _context.Applications.Add(application);
        await _context.SaveChangesAsync();
        return application;
    }

    public async Task<bool> UpdateApplicationAsync(Guid id, Application application)
    {
        var existingApplication = await _context.Applications.FindAsync(id);
        if (existingApplication == null)
        {
            return false;
        }

        existingApplication.CompanyName = application.CompanyName;
        existingApplication.JobTitle = application.JobTitle;
        existingApplication.JobDescription = application.JobDescription;
        existingApplication.Location = application.Location;
        existingApplication.Salary = application.Salary;
        existingApplication.Status = application.Status;
        existingApplication.AppliedDate = application.AppliedDate;
        existingApplication.InterviewDate = application.InterviewDate;
        existingApplication.Notes = application.Notes;
        existingApplication.JobUrl = application.JobUrl;
        existingApplication.UpdatedAt = DateTimeOffset.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ApplicationExistsAsync(id))
            {
                return false;
            }
            throw;
        }
    }

    public async Task<bool> DeleteApplicationAsync(Guid id)
    {
        var application = await _context.Applications.FindAsync(id);
        if (application == null)
        {
            return false;
        }

        _context.Applications.Remove(application);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> ApplicationExistsAsync(Guid id)
    {
        return await _context.Applications.AnyAsync(e => e.Id == id);
    }
}
