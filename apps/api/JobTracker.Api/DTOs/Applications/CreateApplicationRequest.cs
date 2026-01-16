using JobTracker.Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobTracker.Api.DTOs.Applications
{
    public class CreateApplicationRequest
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string CompanyName { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public required string JobTitle { get; set; }
        
        public ApplicationStatus Status { get; set; }
        
        [StringLength(5000)]
        public string JobDescription { get; set; }
        
        [StringLength(200)]
        public string? Location { get; set; }
        
        [Range(0, 10000000)]
        public decimal? Salary { get; set; }
        
        public DateTimeOffset? AppliedDate { get; set; }
        public DateTimeOffset? InterviewDate { get; set; }
        
        [StringLength(2000)]
        public string? Notes { get; set; }
        
        [Url]
        [StringLength(500)]
        public string? JobUrl { get; set; }
        
        [StringLength(200)]
        public string? Resume { get; set; }
        
        [StringLength(200)]
        public string? CoverLetter { get; set; }
    }
}