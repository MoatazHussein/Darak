using Darak.Domain.Constants;

namespace Darak.Application.Common.Dtos.Users
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public int UserTypeValue { get; set; } 
        public string UserTypeName { get; set; } 
        public IReadOnlyList<string> Roles { get; set; } 
    }

}
