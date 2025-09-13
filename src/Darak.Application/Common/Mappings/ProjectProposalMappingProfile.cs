using AutoMapper;
using Darak.Application.Features.Projects.ProjectProposals.Commands.CreateProjectProposal;
using Darak.Application.Features.Projects.ProjectProposals.Commands.UpdateProjectProposal;
using Darak.Application.Features.Projects.ProjectProposals.Queries.GetAllProjectProposals;
using Darak.Domain.Entities.Projects;

namespace Darak.Application.Common.Mappings;

public class ProjectProposalMappingProfile : Profile
{
    public ProjectProposalMappingProfile()
    {

        // Command -> Entity mappings
        CreateMap<CreateProjectProposalCommand, ProjectProposal>();
        CreateMap<UpdateProjectProposalCommand, ProjectProposal>();

        // Entity -> Response mappings
        CreateMap<ProjectProposal, GetAllProjectProposalsQueryResponse>()
            .ForMember(dest => dest.StatusValue, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

    }
}
