namespace Darak.Application.Common.Dtos.Users;

public class UserDto
{
    public Guid Id { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Location { get; set; }
    public int UserTypeValue { get; set; }
    public string UserTypeName { get; set; } = default!;
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 
    public bool EmailConfirmed { get; set; }
    public List<string> Roles { get; set; } = [];

    public Guid? ServiceCategoryId { get; set; }
    public string? ServiceCategoryNameEn { get; set; }
    public string? ServiceCategoryNameAr { get; set; }

}

