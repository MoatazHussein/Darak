using AutoMapper;
using Darak.Application.Common.Dtos.Users;
using Darak.Application.Features.Users.Commands.RegisterClient;
using Darak.Application.Features.Users.Commands.RegisterContractor;
using Darak.Application.Features.Users.Commands.UpdateClient;
using Darak.Application.Features.Users.Commands.UpdateContractor;
using Darak.Domain.Entities;

namespace Darak.Application.Common.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // Commands
        CreateMap<RegisterClientCommand, RegisterClientUserRequest>();
        CreateMap<RegisterContractorCommand, RegisterContractorUserRequest>();

        CreateMap<UpdateClientCommand, UpdateClientUserRequest>();
        CreateMap<UpdateContractorCommand, UpdateContractorUserRequest>();


        // Queries 
        CreateMap<AppUser, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ForMember(dest => dest.UserTypeValue, opt => opt.MapFrom(src => (int)src.UserType))
            .ForMember(dest => dest.UserTypeName, opt => opt.MapFrom(src => src.UserType.ToString()));

    }
}
