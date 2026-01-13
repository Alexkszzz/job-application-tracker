using JobTracker.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.DTOs.Applications
{
    public class ApplicationUpdateDto
    {
        public string? CompanyName { get; set; }
        public string? JobTitle { get; set; }
        [EnumDataType(typeof(ApplicationStatus))]
        public ApplicationStatus Status { get; set; }
        public string? JobDescription { get; set; }
        public string? Location { get; set; }
        public decimal? Salary { get; set; }
        public required DateTimeOffset AppliedDate { get; set; }
        public DateTimeOffset? InterviewDate { get; set; }
        public string? Notes { get; set; }
        public string? JobUrl { get; set; }
    }
}