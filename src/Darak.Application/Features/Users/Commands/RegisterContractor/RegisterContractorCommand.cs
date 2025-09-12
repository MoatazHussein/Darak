using MediatR;

namespace Darak.Application.Features.Users.Commands.RegisterContractor;

public class RegisterContractorCommand : IRequest<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid ServiceCategoryId { get; set; }
    public string? PhoneNumber { get; set; } 

    public string? Location { get; set; }
}
