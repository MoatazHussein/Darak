using MediatR;

namespace Darak.Application.Features.ServiceCategories.Commands.UpdateServiceCategory;

public class UpdateServiceCategoryCommand : IRequest
{
    public Guid Id { get; set;}
    public string NameAr { get; set; } = default!;
    public string NameEn { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; } 

}