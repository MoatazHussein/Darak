namespace Darak.Application.Common.Dtos.Projects;

public class ProjectProposalDto 
{
    public Guid Id { get; set; }
    public string Content { get; set; } = default!;
    public decimal ProposedAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StatusValue { get; set; }
    public string StatusName { get; set; } = default!;
    public string CreatorEmail { get; set; } = default!;
}
