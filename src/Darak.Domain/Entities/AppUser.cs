using Microsoft.AspNetCore.Identity;
using Darak.Domain.Enums;

namespace Darak.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string? LastName { get; set; }
    public string? Location { get; set; } 
    public UserType UserType { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } 


    //Contractor related fields
    public Guid? ServiceCategoryId { get; set; }
    public ServiceCategory? ServiceCategory { get; set; }

}
