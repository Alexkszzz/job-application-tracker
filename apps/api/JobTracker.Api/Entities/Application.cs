namespace JobTracker.Api.Models;
using JobTracker.Api.Enums;

public class Application
{
    private Application()
    {
    }

    private Application(string companyName, string jobTitle, ApplicationStatus status, string jobDescription, string? location, decimal? salary, DateTimeOffset? appliedDate, DateTimeOffset? interviewDate, string? notes, string? jobUrl)
    {
        Id = Guid.NewGuid();
        CompanyName = companyName;
        JobTitle = jobTitle;
        Status = status;
        JobDescription = jobDescription;
        Location = location;
        Salary = salary;
        AppliedDate = appliedDate;
        InterviewDate = interviewDate;
        Notes = notes;
        JobUrl = jobUrl;
    }

    public static Application Create(
        string companyName,
        string jobTitle,
        ApplicationStatus status,
        DateTimeOffset? appliedDate,
        string jobDescription = null,
        string? location = null,
        decimal? salary = null,
        DateTimeOffset? interviewDate = null,
        string? notes = null,
        string? jobUrl = null)
    {
        var now = DateTimeOffset.UtcNow;
        return new Application
        {
            CompanyName = companyName,
            JobTitle = jobTitle,
            Status = status,
            AppliedDate = appliedDate,
            JobDescription = jobDescription,
            Location = location,
            Salary = salary,
            InterviewDate = interviewDate,
            Notes = notes,
            JobUrl = jobUrl,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public Guid Id { get; set; }
    
    public required string CompanyName { get; set; } = string.Empty;
    
    public required string JobTitle { get; set; } = string.Empty;
    
    public string? JobDescription { get; set; } = string.Empty;
    
    public string? Location { get; set; }
    
    public decimal? Salary { get; set; }
    
    public required ApplicationStatus Status { get; set; } // e.g., "Applied", "Interview", "Offer", "Rejected"
    
    public DateTimeOffset? AppliedDate { get; set; }
    
    public DateTimeOffset? InterviewDate { get; set; }
    
    public string? Notes { get; set; }
    
    public string? JobUrl { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
}
