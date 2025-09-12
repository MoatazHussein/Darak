using MediatR;

namespace Darak.Application.Features.ServiceCategories.Commands.DeleteServiceCategory;

public class DeleteServiceCategoryCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}
