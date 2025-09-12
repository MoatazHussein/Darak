using Darak.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Darak.Infrastructure.Persistence.Configurations;

public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.Property(u => u.NameAr).IsRequired().HasMaxLength(50);
        builder.Property(u => u.NameEn).IsRequired().HasMaxLength(50);

        builder.HasIndex(p => p.NameAr).IsUnique(); 
        builder.HasIndex(p => p.NameEn).IsUnique(); 
    }
}
