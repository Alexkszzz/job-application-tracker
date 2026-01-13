using JobTracker.Api.Enums;

namespace JobTracker.Api.DTOs.Applications
{
    public class CreateApplicationRequest
    {
        public required string CompanyName { get; set; }
        public required string JobTitle { get; set; }
        public ApplicationStatus Status { get; set; }
        public string JobDescription { get; set; }
        public string? Location { get; set; }
        public decimal? Salary { get; set; }
        public required DateTimeOffset AppliedDate { get; set; }
        public DateTimeOffset? InterviewDate { get; set; }
        public string? Notes { get; set; }
        public string? JobUrl { get; set; }
    }
}