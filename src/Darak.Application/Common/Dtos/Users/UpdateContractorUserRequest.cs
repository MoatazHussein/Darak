namespace Darak.Application.Common.Dtos.Users;
public class UpdateContractorUserRequest
{
    public Guid UserId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Location { get; set; }
    public Guid ServiceCategoryId { get; set; }

}
