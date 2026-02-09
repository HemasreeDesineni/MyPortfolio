using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Photo
    {
        public int Id { get; set; }

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
        [StringLength(500)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ThumbnailPath { get; set; }

        [StringLength(500)]
        public string? MediumPath { get; set; }

        [StringLength(500)]
        public string? LargePath { get; set; }

        [Required]
        [StringLength(500)]
        public string OriginalFileName { get; set; } = string.Empty;

        public long FileSize { get; set; } = 0;

        [StringLength(20)]
        public string? FileSizeFormatted { get; set; }

        [Required]
        [StringLength(50)]
        public string ContentType { get; set; } = string.Empty;

        public int? ImageWidth { get; set; }

        public int? ImageHeight { get; set; }

        public decimal? AspectRatio { get; set; }

        [StringLength(100)]
        public string? CameraMake { get; set; }

        [StringLength(100)]
        public string? CameraModel { get; set; }

        [StringLength(100)]
        public string? Lens { get; set; }

        [StringLength(20)]
        public string? FocalLength { get; set; }

        [StringLength(10)]
        public string? Aperture { get; set; }

        [StringLength(20)]
        public string? ShutterSpeed { get; set; }

        [StringLength(10)]
        public string? Iso { get; set; }

        public DateTime? TakenAt { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public bool IsPrivate { get; set; } = false;

        public bool AllowDownload { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        public int ViewCount { get; set; } = 0;

        public int LikeCount { get; set; } = 0;

        public int DownloadCount { get; set; } = 0;

        public string[]? Tags { get; set; }

        public string[]? Colors { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? PublishedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; } = null!;
    }
}
