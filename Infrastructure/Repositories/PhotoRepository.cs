using Dapper;
using MyPortfolio.Infrastructure.Database;
using MyPortfolio.Models;

namespace MyPortfolio.Infrastructure.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PhotoRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Photo>> GetPhotosByCategoryAsync(
            int categoryId, 
            bool includeInactive = false, 
            bool includePrivate = false,
            string orderBy = "SortOrder", 
            bool orderDescending = false,
            int? limit = null,
            int? offset = null)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            var whereConditions = new List<string> { "category_id = @CategoryId" };
            
            if (!includeInactive)
                whereConditions.Add("is_active = true");
                
            if (!includePrivate)
                whereConditions.Add("is_private = false");
            
            var whereClause = "WHERE " + string.Join(" AND ", whereConditions);
            var orderClause = GetOrderClause(orderBy, orderDescending);
            var limitClause = limit.HasValue ? $"LIMIT {limit}" : "";
            var offsetClause = offset.HasValue ? $"OFFSET {offset}" : "";
            
            var sql = $@"
                SELECT id, title, slug, description, alt_text, file_name, file_path, 
                       thumbnail_path, medium_path, large_path, original_file_name, 
                       file_size, file_size_formatted, content_type, image_width, 
                       image_height, aspect_ratio, camera_make, camera_model, lens, 
                       focal_length, aperture, shutter_speed, iso, taken_at, location, 
                       latitude, longitude, category_id, is_active, is_featured, 
                       is_private, allow_download, sort_order, view_count, like_count, 
                       download_count, tags, colors, created_at, updated_at, 
                       published_at, created_by, updated_by
                FROM photos 
                {whereClause}
                {orderClause}
                {limitClause}
                {offsetClause}";

            var photos = await connection.QueryAsync<dynamic>(sql, new { CategoryId = categoryId });
            return photos.Select(MapDatabaseToModel);
        }

        public async Task<int> GetPhotosByCategoryCountAsync(int categoryId, bool includeInactive = false, bool includePrivate = false)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            var whereConditions = new List<string> { "category_id = @CategoryId" };
            
            if (!includeInactive)
                whereConditions.Add("is_active = true");
                
            if (!includePrivate)
                whereConditions.Add("is_private = false");
            
            var whereClause = "WHERE " + string.Join(" AND ", whereConditions);
            
            var sql = $@"
                SELECT COUNT(*)
                FROM photos 
                {whereClause}";

            return await connection.QuerySingleAsync<int>(sql, new { CategoryId = categoryId });
        }

        public async Task<Photo?> GetPhotoByIdAsync(int id)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                SELECT id, title, slug, description, alt_text, file_name, file_path, 
                       thumbnail_path, medium_path, large_path, original_file_name, 
                       file_size, file_size_formatted, content_type, image_width, 
                       image_height, aspect_ratio, camera_make, camera_model, lens, 
                       focal_length, aperture, shutter_speed, iso, taken_at, location, 
                       latitude, longitude, category_id, is_active, is_featured, 
                       is_private, allow_download, sort_order, view_count, like_count, 
                       download_count, tags, colors, created_at, updated_at, 
                       published_at, created_by, updated_by
                FROM photos 
                WHERE id = @Id";

            var photo = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Id = id });
            return photo != null ? MapDatabaseToModel(photo) : null;
        }

        public async Task<Photo?> GetPhotoBySlugAsync(string slug)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = @"
                SELECT id, title, slug, description, alt_text, file_name, file_path, 
                       thumbnail_path, medium_path, large_path, original_file_name, 
                       file_size, file_size_formatted, content_type, image_width, 
                       image_height, aspect_ratio, camera_make, camera_model, lens, 
                       focal_length, aperture, shutter_speed, iso, taken_at, location, 
                       latitude, longitude, category_id, is_active, is_featured, 
                       is_private, allow_download, sort_order, view_count, like_count, 
                       download_count, tags, colors, created_at, updated_at, 
                       published_at, created_by, updated_by
                FROM photos 
                WHERE slug = @Slug AND is_active = true";

            var photo = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { Slug = slug });
            return photo != null ? MapDatabaseToModel(photo) : null;
        }

        public async Task<IEnumerable<Photo>> GetFeaturedPhotosAsync(int? limit = null)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            var limitClause = limit.HasValue ? $"LIMIT {limit}" : "";
            
            var sql = $@"
                SELECT id, title, slug, description, alt_text, file_name, file_path, 
                       thumbnail_path, medium_path, large_path, original_file_name, 
                       file_size, file_size_formatted, content_type, image_width, 
                       image_height, aspect_ratio, camera_make, camera_model, lens, 
                       focal_length, aperture, shutter_speed, iso, taken_at, location, 
                       latitude, longitude, category_id, is_active, is_featured, 
                       is_private, allow_download, sort_order, view_count, like_count, 
                       download_count, tags, colors, created_at, updated_at, 
                       published_at, created_by, updated_by
                FROM photos 
                WHERE is_featured = true AND is_active = true AND is_private = false
                ORDER BY sort_order ASC, created_at DESC
                {limitClause}";

            var photos = await connection.QueryAsync<dynamic>(sql);
            return photos.Select(MapDatabaseToModel);
        }

        public async Task<IEnumerable<Photo>> GetRecentPhotosAsync(int? limit = null)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            var limitClause = limit.HasValue ? $"LIMIT {limit}" : "";
            
            var sql = $@"
                SELECT id, title, slug, description, alt_text, file_name, file_path, 
                       thumbnail_path, medium_path, large_path, original_file_name, 
                       file_size, file_size_formatted, content_type, image_width, 
                       image_height, aspect_ratio, camera_make, camera_model, lens, 
                       focal_length, aperture, shutter_speed, iso, taken_at, location, 
                       latitude, longitude, category_id, is_active, is_featured, 
                       is_private, allow_download, sort_order, view_count, like_count, 
                       download_count, tags, colors, created_at, updated_at, 
                       published_at, created_by, updated_by
                FROM photos 
                WHERE is_active = true AND is_private = false
                ORDER BY created_at DESC
                {limitClause}";

            var photos = await connection.QueryAsync<dynamic>(sql);
            return photos.Select(MapDatabaseToModel);
        }

        public async Task<string?> GetCategoryNameAsync(int categoryId)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();
            
            const string sql = "SELECT name FROM categories WHERE id = @CategoryId";
            
            return await connection.QueryFirstOrDefaultAsync<string>(sql, new { CategoryId = categoryId });
        }

        private static Photo MapDatabaseToModel(dynamic dbPhoto)
        {
            return new Photo
            {
                Id = dbPhoto.id,
                Title = dbPhoto.title ?? string.Empty,
                Slug = dbPhoto.slug ?? string.Empty,
                Description = dbPhoto.description,
                AltText = dbPhoto.alt_text,
                FileName = dbPhoto.file_name ?? string.Empty,
                FilePath = dbPhoto.file_path ?? string.Empty,
                ThumbnailPath = dbPhoto.thumbnail_path,
                MediumPath = dbPhoto.medium_path,
                LargePath = dbPhoto.large_path,
                OriginalFileName = dbPhoto.original_file_name ?? string.Empty,
                FileSize = dbPhoto.file_size,
                FileSizeFormatted = dbPhoto.file_size_formatted,
                ContentType = dbPhoto.content_type ?? string.Empty,
                ImageWidth = dbPhoto.image_width,
                ImageHeight = dbPhoto.image_height,
                AspectRatio = dbPhoto.aspect_ratio,
                CameraMake = dbPhoto.camera_make,
                CameraModel = dbPhoto.camera_model,
                Lens = dbPhoto.lens,
                FocalLength = dbPhoto.focal_length,
                Aperture = dbPhoto.aperture,
                ShutterSpeed = dbPhoto.shutter_speed,
                Iso = dbPhoto.iso,
                TakenAt = dbPhoto.taken_at,
                Location = dbPhoto.location,
                Latitude = dbPhoto.latitude,
                Longitude = dbPhoto.longitude,
                CategoryId = dbPhoto.category_id,
                IsActive = dbPhoto.is_active,
                IsFeatured = dbPhoto.is_featured,
                IsPrivate = dbPhoto.is_private,
                AllowDownload = dbPhoto.allow_download,
                SortOrder = dbPhoto.sort_order,
                ViewCount = dbPhoto.view_count,
                LikeCount = dbPhoto.like_count,
                DownloadCount = dbPhoto.download_count,
                Tags = dbPhoto.tags,
                Colors = dbPhoto.colors,
                CreatedAt = dbPhoto.created_at,
                UpdatedAt = dbPhoto.updated_at,
                PublishedAt = dbPhoto.published_at,
                CreatedBy = dbPhoto.created_by,
                UpdatedBy = dbPhoto.updated_by
            };
        }

        private static string GetOrderClause(string orderBy, bool orderDescending)
        {
            var direction = orderDescending ? "DESC" : "ASC";
            
            return orderBy?.ToLower() switch
            {
                "title" => $"ORDER BY title {direction}",
                "createdat" => $"ORDER BY created_at {direction}",
                "takenat" => $"ORDER BY taken_at {direction}",
                "viewcount" => $"ORDER BY view_count {direction}",
                "likecount" => $"ORDER BY like_count {direction}",
                "sortorder" => $"ORDER BY sort_order {direction}",
                _ => $"ORDER BY sort_order {direction}"
            };
        }
    }
}
