using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Slug { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(160)]
        public string? MetaTitle { get; set; }

        [StringLength(320)]
        public string? MetaDescription { get; set; }

        [StringLength(500)]
        public string? CoverImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public int SortOrder { get; set; } = 0;

        public int? ParentId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        // Navigation properties
        public virtual Category? Parent { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}
