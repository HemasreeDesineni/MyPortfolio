using MyPortfolio.Models;

namespace MyPortfolio.Infrastructure.Repositories
{
    public interface IPhotoRepository
    {
        Task<IEnumerable<Photo>> GetPhotosByCategoryAsync(
            int categoryId, 
            bool includeInactive = false, 
            bool includePrivate = false,
            string orderBy = "SortOrder", 
            bool orderDescending = false,
            int? limit = null,
            int? offset = null);
        Task<int> GetPhotosByCategoryCountAsync(int categoryId, bool includeInactive = false, bool includePrivate = false);
        Task<Photo?> GetPhotoByIdAsync(int id);
        Task<Photo?> GetPhotoBySlugAsync(string slug);
        Task<IEnumerable<Photo>> GetFeaturedPhotosAsync(int? limit = null);
        Task<IEnumerable<Photo>> GetRecentPhotosAsync(int? limit = null);
        Task<string?> GetCategoryNameAsync(int categoryId);
    }
}
