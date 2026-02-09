using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.Application.Users.Commands;
using MyPortfolio.DTOs;
using MyPortfolio.Infrastructure.Repositories;
using MyPortfolio.Models;
using MyPortfolio.Services;

namespace MyPortfolio.Application.Users.Handlers
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(
            IUserRepository userRepository,
            IJwtService jwtService,
            ILogger<LoginUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(request.Email);
                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                //var hasher = new PasswordHasher<User>();
                //var user1 = new User
                //{
                //    Id = "admin-user-id-001",
                //    Email = "admin@photography.com"
                //};

                //string password = "Admin123!";
                //string passwordHash = hasher.HashPassword(user, password);

                //Console.WriteLine(passwordHash);

                var hasher = new PasswordHasher<User>();

                var result = hasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    request.Password
                );

                if (result == PasswordVerificationResult.Failed)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                var role = await _userRepository.GetUserRoleAsync(user.Id) ?? "Client";

                var token = _jwtService.GenerateToken(user, role);
                var expiresAt = DateTime.UtcNow.AddMinutes(60);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    ExpiresAt = expiresAt,
                    User = new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Role = role,
                        CreatedAt = user.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "An error occurred during login"
                };
            }
        }
    }
}
