using Darak.Domain.Entities;
using Darak.Domain.Entities.Projects;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Darak.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }


    public DbSet<ServiceCategory> ServiceCategories { get; set; }
    public DbSet<ProjectRequest> ProjectRequests { get; set; }
    public DbSet<ProjectProposal> ProjectProposals { get; set; }
    public DbSet<OtpChallenge> OtpChallenges { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


    }
}


