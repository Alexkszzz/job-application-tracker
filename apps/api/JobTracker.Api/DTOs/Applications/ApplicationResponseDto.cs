using JobTracker.Api.Enums;

namespace JobTracker.Api.DTOs.Applications 
{
    public record ApplicationResponseDto
    {
        public Guid Id { get; init; }
        public string CompanyName { get; init; }
        public string JobTitle { get; init; }
        public ApplicationStatus Status { get; init; }
        public string JobDescription { get; init; }
        public string? Location { get; init; }
        public decimal? Salary { get; init; }
        public DateTimeOffset AppliedDate { get; init; }
        public DateTimeOffset? InterviewDate { get; init; }
        public string? Notes { get; init; }
        public string? JobUrl { get; init; }
    }
}