using JobTracker.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Data;

public class JobTrackerDbContext : IdentityDbContext<ApplicationUser>
{
    public JobTrackerDbContext(DbContextOptions<JobTrackerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Application> Applications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JobTrackerDbContext).Assembly);
    }
}
