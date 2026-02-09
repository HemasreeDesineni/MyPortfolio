using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.Application.Users.Queries;
using MyPortfolio.DTOs;
using MyPortfolio.Infrastructure.Repositories;

namespace MyPortfolio.Application.Users.Handlers
{
    public class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetCurrentUserQueryHandler> _logger;

        public GetCurrentUserQueryHandler(
            IUserRepository userRepository,
            ILogger<GetCurrentUserQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.UserId);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                var role = await _userRepository.GetUserRoleAsync(user.Id) ?? "Client";

                return new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = role,
                    CreatedAt = user.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current user");
                throw;
            }
        }
    }
}
