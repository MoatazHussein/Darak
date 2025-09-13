using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Darak.Domain.Entities.Projects;
using Darak.Domain.Enums;

namespace Darak.Infrastructure.Persistence.Configurations;
public class ProjectRequestConfiguration : IEntityTypeConfiguration<ProjectRequest>
{
    public void Configure(EntityTypeBuilder<ProjectRequest> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
               .HasMaxLength(200);

        builder.Property(p => p.Description)
               .IsRequired()
               .HasMaxLength(2000);  

        builder.Property(p => p.ImageUrl)
               .HasMaxLength(500);   

        builder.Property(pr => pr.MinBudget)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.Property(pr => pr.MaxBudget)
               .IsRequired()
               .HasColumnType("decimal(18,2)");

        builder.HasOne(p => p.ServiceCategory)
               .WithMany()
               .HasForeignKey(p => p.ServiceCategoryId);

        builder.HasOne(p => p.Creator)
               .WithMany()
               .HasForeignKey(p => p.CreatorId);

        builder.Property(p => p.Status)
               .HasDefaultValue(ProjectRequestStatus.Open);
    }
}

