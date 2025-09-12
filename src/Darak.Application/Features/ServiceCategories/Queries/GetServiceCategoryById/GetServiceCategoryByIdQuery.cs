using Darak.Domain.Entities;
using MediatR;

namespace Darak.Application.Features.ServiceCategories.Queries.GetServiceCategoryById;

public record GetServiceCategoryByIdQuery(Guid id) : IRequest<GetServiceCategoryByIdQueryResponse?>
{
    public Guid Id { get; init; } = id;
}

