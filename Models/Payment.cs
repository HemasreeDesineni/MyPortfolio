using System.ComponentModel.DataAnnotations;

namespace MyPortfolio.Models
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(10)]
        public string Currency { get; set; } = "USD";

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [StringLength(200)]
        public string PaymentMethod { get; set; } = string.Empty;

        [StringLength(200)]
        public string TransactionId { get; set; } = string.Empty;

        [StringLength(200)]
        public string PaymentIntentId { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(1000)]
        public string PaymentMetadata { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? PaidAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Appointment Appointment { get; set; } = null!;
    }

    public enum PaymentStatus
    {
        Pending,
        Processing,
        Succeeded,
        Failed,
        Cancelled,
        Refunded
    }
}
