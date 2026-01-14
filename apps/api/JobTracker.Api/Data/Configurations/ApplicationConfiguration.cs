using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Api.Data.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("Applications");
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(a => a.CompanyName)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(a => a.JobTitle)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(a => a.JobDescription)
            .HasMaxLength(2000);
        
        builder.Property(a => a.Location)
            .HasMaxLength(200);
        
        builder.Property(a => a.Salary)
            .HasPrecision(18, 2);
        
        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(a => a.AppliedDate);
        
        builder.Property(a => a.InterviewDate);
        
        builder.Property(a => a.Notes)
            .HasMaxLength(1000);
        
        builder.Property(a => a.JobUrl)
            .HasMaxLength(500);
        
        builder.Property(a => a.Resume)
            .HasMaxLength(1000);
        
        builder.Property(a => a.CoverLetter)
            .HasMaxLength(1000);
        
        builder.Property(a => a.CreatedAt)
            .IsRequired();
        
        builder.Property(a => a.UpdatedAt);
    }
}
