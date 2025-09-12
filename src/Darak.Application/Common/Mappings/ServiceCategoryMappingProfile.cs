using AutoMapper;
using Darak.Application.Features.ServiceCategories.Commands.CreateServiceCategory;
using Darak.Application.Features.ServiceCategories.Commands.UpdateServiceCategory;
using Darak.Application.Features.ServiceCategories.Queries.GetAllServiceCategories;
using Darak.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;
using Darak.Domain.Entities;

namespace Darak.Application.Common.Mappings;

public class ServiceCategoryMappingProfile : Profile
{
    public ServiceCategoryMappingProfile()
    {

        // Command -> Entity mappings
        CreateMap<CreateServiceCategoryCommand, ServiceCategory>();
        CreateMap<UpdateServiceCategoryCommand, ServiceCategory>();

        // Entity -> Response mappings
        CreateMap<ServiceCategory, GetAllServiceCategoriesQueryResponse>();
        CreateMap<ServiceCategory, GetServiceCategoryByIdQueryResponse>();

      
    }
}
