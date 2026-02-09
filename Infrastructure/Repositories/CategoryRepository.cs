using Dapper;
using MyPortfolio.Infrastructure.Database;
using MyPortfolio.Models;

namespace MyPortfolio.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public CategoryRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool includeInactive = false, string orderBy = "SortOrder", bool orderDescending = false)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            var whereClause = includeInactive ? "" : "WHERE is_active = true";
            var orderClause = GetOrderClause(orderBy, orderDescending);
            
            var sql = $@"
                SELECT id, name, slug, description, meta_title, meta_description, cover_image_url, 
                       is_active, is_featured, sort_order, parent_id, created_at, updated_at, 
                       created_by, updated_by
                FROM categories 
                {whereClause}
                {orderClause}";

            var categories = await connection.QueryAsync<dynamic>(sql);
            return categories.Select(MapDatabaseToModel);
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                SELECT id, name, slug, description, meta_title, meta_description, cover_image_url, 
                       is_active, is_featured, sort_order, parent_id, created_at, updated_at, 
                       created_by, updated_by
                FROM categories 
                WHERE id = @Id";

            var category = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
            return category != null ? MapDatabaseToModel(category) : null;
        }

        public async Task<Category?> GetCategoryBySlugAsync(string slug)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                SELECT id, name, slug, description, meta_title, meta_description, cover_image_url, 
                       is_active, is_featured, sort_order, parent_id, created_at, updated_at, 
                       created_by, updated_by
                FROM categories 
                WHERE slug = @Slug";

            var category = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Slug = slug });
            return category != null ? MapDatabaseToModel(category) : null;
        }

        public async Task<int> GetCategoriesCountAsync(bool includeInactive = false)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            var whereClause = includeInactive ? "" : "WHERE is_active = true";
            
            var sql = $@"
                SELECT COUNT(*)
                FROM categories 
                {whereClause}";

            return await connection.QuerySingleAsync<int>(sql);
        }

        private static Category MapDatabaseToModel(dynamic dbCategory)
        {
            return new Category
            {
                Id = dbCategory.id,
                Name = dbCategory.name ?? string.Empty,
                Slug = dbCategory.slug ?? string.Empty,
                Description = dbCategory.description,
                MetaTitle = dbCategory.meta_title,
                MetaDescription = dbCategory.meta_description,
                CoverImageUrl = dbCategory.cover_image_url,
                IsActive = dbCategory.is_active,
                IsFeatured = dbCategory.is_featured,
                SortOrder = dbCategory.sort_order,
                ParentId = dbCategory.parent_id,
                CreatedAt = dbCategory.created_at,
                UpdatedAt = dbCategory.updated_at,
                CreatedBy = dbCategory.created_by,
                UpdatedBy = dbCategory.updated_by
            };
        }

        private static string GetOrderClause(string orderBy, bool orderDescending)
        {
            var direction = orderDescending ? "DESC" : "ASC";
            
            return orderBy?.ToLower() switch
            {
                "name" => $"ORDER BY name {direction}",
                "createdat" => $"ORDER BY created_at {direction}",
                "sortorder" => $"ORDER BY sort_order {direction}",
                _ => $"ORDER BY sort_order {direction}"
            };
        }
    }
}
