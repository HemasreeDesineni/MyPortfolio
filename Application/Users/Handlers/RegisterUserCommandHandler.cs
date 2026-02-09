using BCrypt.Net;
using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.Application.Users.Commands;
using MyPortfolio.DTOs;
using MyPortfolio.Infrastructure.Repositories;
using MyPortfolio.Models;
using MyPortfolio.Services;

namespace MyPortfolio.Application.Users.Handlers
{
    public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    };
                }

                // Create new user
                var user = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    CreatedAt = DateTime.UtcNow
                };

                var createdUser = await _userRepository.CreateAsync(user);

                // Add user to Client role by default
                await _userRepository.AddUserToRoleAsync(createdUser.Id, "Client");

                var token = _jwtService.GenerateToken(createdUser, "Client");
                var expiresAt = DateTime.UtcNow.AddMinutes(60);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Registration successful",
                    Token = token,
                    ExpiresAt = expiresAt,
                    User = new UserDto
                    {
                        Id = createdUser.Id,
                        FirstName = createdUser.FirstName,
                        LastName = createdUser.LastName,
                        Email = createdUser.Email,
                        PhoneNumber = createdUser.PhoneNumber,
                        Role = "Client",
                        CreatedAt = createdUser.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "An error occurred during registration"
                };
            }
        }
    }
}
