using AutoMapper;
using Darak.Application.Features.Projects.ProjectRequests.Commands.CreateProjectRequest;
using Darak.Application.Features.Projects.ProjectRequests.Commands.UpdateProjectRequest;
using Darak.Application.Features.Projects.ProjectRequests.Queries.GetAllProjectRequests;
using Darak.Application.Features.Projects.ProjectRequests.Queries.GetProjectRequestById;
using Darak.Domain.Entities.Projects;

namespace Darak.Application.Common.Mappings;

public class ProjectRequestMappingProfile : Profile
{
    public ProjectRequestMappingProfile()
    {

        // Command -> Entity mappings
        CreateMap<CreateProjectRequestCommand, ProjectRequest>();
        CreateMap<UpdateProjectRequestCommand, ProjectRequest>();

        // Entity -> Response mappings
        CreateMap<ProjectRequest, GetAllProjectRequestsQueryResponse>()
            .ForMember(dest => dest.StatusValue, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<ProjectRequest, GetProjectRequestByIdQueryResponse>()
             .ForMember(dest => dest.StatusValue, opt => opt.MapFrom(src => (int)src.Status))
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));

    }
}
