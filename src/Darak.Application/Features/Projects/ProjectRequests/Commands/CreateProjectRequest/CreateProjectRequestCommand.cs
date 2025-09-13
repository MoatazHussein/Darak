using MediatR;

namespace Darak.Application.Features.Projects.ProjectRequests.Commands.CreateProjectRequest;

public record CreateProjectRequestCommand(
string Title,
string Description,
string ImageUrl,
decimal MinBudget,
decimal MaxBudget,
Guid ServiceCategoryId
) : IRequest<Guid>;
