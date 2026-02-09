using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.DTOs;

namespace MyPortfolio.Application.Users.Commands
{
    public class RegisterUserCommand : ICommand<AuthResponseDto>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
