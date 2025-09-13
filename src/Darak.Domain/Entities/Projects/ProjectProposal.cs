using Darak.Domain.Enums;

namespace Darak.Domain.Entities.Projects;

public class ProjectProposal
{
    public Guid Id { get; set; } 
    public Guid ProjectRequestId { get; set; }
    public ProjectRequest? ProjectRequest { get; set; }

    public string Content { get; set; } = default!;
    public Guid CreatorId { get; set; }
    public AppUser? Creator { get; set; }
    public decimal ProposedAmount { get; set; }

    public DateTime CreatedAt { get; set; }
    public ProjectProposalStatus Status { get; set; }

}
