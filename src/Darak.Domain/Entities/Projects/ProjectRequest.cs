using Darak.Domain.Enums;

namespace Darak.Domain.Entities.Projects;

public class ProjectRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public decimal MinBudget { get; set; }
    public decimal MaxBudget { get; set; }
    public Guid ServiceCategoryId { get; set; }
    public ServiceCategory ServiceCategory { get; set; } = default!;

    public Guid CreatorId { get; set; }
    public AppUser? Creator { get; set; }

    public DateTime CreatedAt { get; set; }
    public ProjectRequestStatus Status { get; set; } 

    public ICollection<ProjectProposal> Proposals { get; set; } = new List<ProjectProposal>();
}
