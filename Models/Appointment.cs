using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string EventType { get; set; } = string.Empty;

        [Required]
        public DateTime RequestedDate { get; set; }

        [StringLength(100)]
        public string RequestedTime { get; set; } = string.Empty;

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ClientEmail { get; set; } = string.Empty;

        [StringLength(20)]
        public string ClientPhone { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [StringLength(500)]
        public string AdminNotes { get; set; } = string.Empty;

        public decimal? EstimatedCost { get; set; }

        public bool PaymentRequired { get; set; } = false;

        public int? PaymentId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Payment? Payment { get; set; }
    }

    public enum AppointmentStatus
    {
        Pending,
        Approved,
        Rejected,
        Completed,
        Cancelled
    }
}
