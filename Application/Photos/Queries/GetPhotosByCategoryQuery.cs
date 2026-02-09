using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.DTOs;

namespace MyPortfolio.Application.Photos.Queries
{
    public class GetPhotosByCategoryQuery : IQuery<PhotosListResponseDto>
    {
        public int CategoryId { get; set; }
        public bool IncludeInactive { get; set; } = false;
        public bool IncludePrivate { get; set; } = false;
        public string? OrderBy { get; set; } = "SortOrder"; // SortOrder, Title, CreatedAt, TakenAt, ViewCount
        public bool OrderDescending { get; set; } = false;
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
