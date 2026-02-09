using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.DTOs
{
    public class PhotoResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AltText { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string? ThumbnailPath { get; set; }
        public string? MediumPath { get; set; }
        public string? LargePath { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? FileSizeFormatted { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public int? ImageWidth { get; set; }
        public int? ImageHeight { get; set; }
        public decimal? AspectRatio { get; set; }
        public string? CameraMake { get; set; }
        public string? CameraModel { get; set; }
        public string? Lens { get; set; }
        public string? FocalLength { get; set; }
        public string? Aperture { get; set; }
        public string? ShutterSpeed { get; set; }
        public string? Iso { get; set; }
        public DateTime? TakenAt { get; set; }
        public string? Location { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsPrivate { get; set; }
        public bool AllowDownload { get; set; }
        public int SortOrder { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int DownloadCount { get; set; }
        public string[]? Tags { get; set; }
        public string[]? Colors { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class PhotosListResponseDto
    {
        public List<PhotoResponseDto> Photos { get; set; } = new List<PhotoResponseDto>();
        public int TotalCount { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class CreatePhotoDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        [StringLength(255)]
        public string? AltText { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public IFormFile PhotoFile { get; set; } = null!;

        public string[]? Tags { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public bool IsPrivate { get; set; } = false;

        public bool AllowDownload { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        public DateTime? TakenAt { get; set; }

        public string? Location { get; set; }

        public string? CameraMake { get; set; }

        public string? CameraModel { get; set; }

        public string? Lens { get; set; }

        public string? FocalLength { get; set; }

        public string? Aperture { get; set; }

        public string? ShutterSpeed { get; set; }

        public string? Iso { get; set; }
    }

    public class UpdatePhotoDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        [StringLength(255)]
        public string? AltText { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string[]? Tags { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public bool IsPrivate { get; set; } = false;

        public bool AllowDownload { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        public DateTime? TakenAt { get; set; }

        public string? Location { get; set; }

        public string? CameraMake { get; set; }

        public string? CameraModel { get; set; }

        public string? Lens { get; set; }

        public string? FocalLength { get; set; }

        public string? Aperture { get; set; }

        public string? ShutterSpeed { get; set; }

        public string? Iso { get; set; }
    }
}
