using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Darak.Application.Features.Users.Commands.UpdateContractor;

public record UpdateContractorCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Location,
    Guid ServiceCategoryId
) : IRequest<IdentityResult>;