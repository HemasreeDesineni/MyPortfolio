using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.DTOs;

namespace MyPortfolio.Application.Users.Queries
{
    public class GetCurrentUserQuery : IQuery<UserDto>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
