using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.DTOs;

namespace MyPortfolio.Application.Users.Commands
{
    public class LoginUserCommand : ICommand<AuthResponseDto>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
