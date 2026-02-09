using Dapper;
using MyPortfolio.Infrastructure.Database;
using MyPortfolio.Models;

namespace MyPortfolio.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UserRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                SELECT u.*, ur.role_name as Role
                FROM users u
                LEFT JOIN user_roles ur ON u.id = ur.user_id
                WHERE u.id = @Id";

            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                SELECT u.*, ur.role_name as Role
                FROM users u
                LEFT JOIN user_roles ur ON u.id = ur.user_id
                WHERE LOWER(u.email) = LOWER(@Email)";

            var result =  await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
            return result;
        }

        public async Task<User> CreateAsync(User user)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                INSERT INTO users (id, first_name, last_name, email, phone_number, password_hash, created_at, updated_at)
                VALUES (@Id, @FirstName, @LastName, @Email, @PhoneNumber, @PasswordHash, @CreatedAt, @UpdatedAt)
                RETURNING *";

            user.Id = Guid.NewGuid().ToString();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            return await connection.QuerySingleAsync<User>(sql, user);
        }

        public async Task<User> UpdateAsync(User user)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                UPDATE users 
                SET first_name = @FirstName, 
                    last_name = @LastName, 
                    email = @Email, 
                    phone_number = @PhoneNumber,
                    password_hash = @PasswordHash,
                    updated_at = @UpdatedAt
                WHERE id = @Id
                RETURNING *";

            user.UpdatedAt = DateTime.UtcNow;

            return await connection.QuerySingleAsync<User>(sql, user);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = "DELETE FROM users WHERE id = @Id";
            
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> ExistsAsync(string email)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = "SELECT COUNT(1) FROM users WHERE LOWER(email) = LOWER(@Email)";
            
            var count = await connection.QuerySingleAsync<int>(sql, new { Email = email });
            return count > 0;
        }

        public async Task<string?> GetUserRoleAsync(string userId)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = "SELECT role_name FROM user_roles WHERE user_id = @UserId";
            
            return await connection.QueryFirstOrDefaultAsync<string>(sql, new { UserId = userId });
        }

        public async Task AddUserToRoleAsync(string userId, string role)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                INSERT INTO user_roles (user_id, role_name, created_at)
                VALUES (@UserId, @Role, @CreatedAt)
                ON CONFLICT (user_id) DO UPDATE SET role_name = @Role";

            await connection.ExecuteAsync(sql, new 
            { 
                UserId = userId, 
                Role = role, 
                CreatedAt = DateTime.UtcNow 
            });
        }
    }
}
