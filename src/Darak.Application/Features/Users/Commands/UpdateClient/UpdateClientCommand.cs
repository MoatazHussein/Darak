using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Darak.Application.Features.Users.Commands.UpdateClient;

public record UpdateClientCommand(
    string UserId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Location
) : IRequest<IdentityResult>;