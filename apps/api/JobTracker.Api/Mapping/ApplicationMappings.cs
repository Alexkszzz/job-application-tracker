using JobTracker.Api.DTOs.Applications;
using JobTracker.Api.Models;

namespace JobTracker.Api.Mapping;

public static class ApplicationMappings
{
    public static ApplicationResponseDto ToResponseDto(this Application application)
    {
        return new ApplicationResponseDto
        {
            Id = application.Id,
            CompanyName = application.CompanyName,
            JobTitle = application.JobTitle,
            Status = application.Status,
            JobDescription = application.JobDescription ?? string.Empty,
            Location = application.Location,
            Salary = application.Salary,
            AppliedDate = application.AppliedDate,
            InterviewDate = application.InterviewDate,
            Notes = application.Notes,
            JobUrl = application.JobUrl,
            Resume = application.Resume,
            CoverLetter = application.CoverLetter
        };
    }

    public static Application ToEntity(this CreateApplicationRequest request, string userId)
    {
        return Application.Create(
            companyName: request.CompanyName,
            jobTitle: request.JobTitle,
            status: request.Status,
            userId: userId,
            appliedDate: request.AppliedDate,
            jobDescription: request.JobDescription,
            location: request.Location,
            salary: request.Salary,
            interviewDate: request.InterviewDate,
            notes: request.Notes,
            jobUrl: request.JobUrl,
            resume: request.Resume,
            coverLetter: request.CoverLetter
        );
    }

    public static void UpdateEntity(this Application application, ApplicationUpdateDto dto)
    {
        if (dto.CompanyName != null)
            application.CompanyName = dto.CompanyName;
        
        if (dto.JobTitle != null)
            application.JobTitle = dto.JobTitle;
        
        application.Status = dto.Status;
        
        if (dto.JobDescription != null)
            application.JobDescription = dto.JobDescription;
        
        application.Location = dto.Location;
        application.Salary = dto.Salary;
        application.AppliedDate = dto.AppliedDate;
        application.InterviewDate = dto.InterviewDate;
        application.Notes = dto.Notes;
        application.JobUrl = dto.JobUrl;
        application.Resume = dto.Resume;
        application.CoverLetter = dto.CoverLetter;
        application.UpdatedAt = DateTimeOffset.UtcNow;
    }
}
