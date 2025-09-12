using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Darak.Domain.Entities;

namespace Darak.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }


    public DbSet<ServiceCategory> ServiceCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); 
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


    }
}


