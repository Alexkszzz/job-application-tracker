using JobTracker.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.DTOs.Applications
{
    public class ApplicationUpdateDto
    {
        [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string? CompanyName { get; set; }
        
        [StringLength(200, ErrorMessage = "Job title cannot exceed 200 characters")]
        public string? JobTitle { get; set; }
        
        [EnumDataType(typeof(ApplicationStatus))]
        public ApplicationStatus Status { get; set; }
        
        [StringLength(5000, ErrorMessage = "Job description cannot exceed 5000 characters")]
        public string? JobDescription { get; set; }
        
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string? Location { get; set; }
        
        [Range(0, 10000000, ErrorMessage = "Salary must be between 0 and 10,000,000")]
        public decimal? Salary { get; set; }
        
        public required DateTimeOffset AppliedDate { get; set; }
        public DateTimeOffset? InterviewDate { get; set; }
        
        [StringLength(2000, ErrorMessage = "Notes cannot exceed 2000 characters")]
        public string? Notes { get; set; }
        
        [Url(ErrorMessage = "Job URL must be a valid URL")]
        [StringLength(500, ErrorMessage = "Job URL cannot exceed 500 characters")]
        public string? JobUrl { get; set; }
        
        [StringLength(500, ErrorMessage = "Resume cannot exceed 500 characters")]
        public string? Resume { get; set; }
        
        [StringLength(500, ErrorMessage = "Cover letter cannot exceed 500 characters")]
        public string? CoverLetter { get; set; }
    }
}