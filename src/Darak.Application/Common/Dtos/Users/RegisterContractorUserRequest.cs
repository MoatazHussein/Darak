namespace Darak.Application.Common.Dtos.Users;
public class RegisterContractorUserRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? Location { get; set; }
    public Guid ServiceCategoryId { get; set; }


}
