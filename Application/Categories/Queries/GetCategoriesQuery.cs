using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.DTOs;

namespace MyPortfolio.Application.Categories.Queries
{
    public class GetCategoriesQuery : IQuery<CategoriesListResponseDto>
    {
        public bool IncludeInactive { get; set; } = false;
        public string? OrderBy { get; set; } = "SortOrder"; // SortOrder, Name, CreatedAt
        public bool OrderDescending { get; set; } = false;
    }
}
