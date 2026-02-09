using MyPortfolio.Models;

namespace MyPortfolio.Infrastructure.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(string id);
        Task<bool> ExistsAsync(string email);
        Task<string?> GetUserRoleAsync(string userId);
        Task AddUserToRoleAsync(string userId, string role);
    }
}
