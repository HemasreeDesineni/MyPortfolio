using MyPortfolio.Models;

namespace MyPortfolio.Infrastructure.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(bool includeInactive = false, string orderBy = "SortOrder", bool orderDescending = false);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryBySlugAsync(string slug);
        Task<int> GetCategoriesCountAsync(bool includeInactive = false);
    }
}
