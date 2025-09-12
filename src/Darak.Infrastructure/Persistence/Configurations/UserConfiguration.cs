using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Darak.Domain.Entities;

namespace Darak.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PhoneNumber).HasMaxLength(15);
        builder.Property(u => u.Location).HasMaxLength(200);
        builder.Property(u => u.ServiceCategoryId).HasColumnType("uniqueidentifier");

        builder.HasOne(u => u.ServiceCategory)
            .WithMany()
            .HasForeignKey(u => u.ServiceCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
