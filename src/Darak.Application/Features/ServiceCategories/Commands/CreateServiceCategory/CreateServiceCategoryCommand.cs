using MediatR;

namespace Darak.Application.Features.ServiceCategories.Commands.CreateServiceCategory;

public class CreateServiceCategoryCommand : IRequest<Guid>
{
    public string NameAr { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string? Description { get; set; } 
    public string? ImageUrl { get; set; } 

}
