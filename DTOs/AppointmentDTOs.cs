using System.ComponentModel.DataAnnotations;
using MyPortfolio.Models;

namespace MyPortfolio.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public DateTime RequestedDate { get; set; }
        public string RequestedTime { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;
        public string ClientPhone { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; }
        public string AdminNotes { get; set; } = string.Empty;
        public decimal? EstimatedCost { get; set; }
        public bool PaymentRequired { get; set; }
        public PaymentDto? Payment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }

    public class CreateAppointmentDto
    {
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
        [EmailAddress]
        [StringLength(200)]
        public string ClientEmail { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string ClientPhone { get; set; } = string.Empty;
    }

    public class UpdateAppointmentStatusDto
    {
        [Required]
        public AppointmentStatus Status { get; set; }

        [StringLength(500)]
        public string AdminNotes { get; set; } = string.Empty;

        public decimal? EstimatedCost { get; set; }

        public bool PaymentRequired { get; set; } = false;
    }

    public class AppointmentFilterDto
    {
        public AppointmentStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? ClientEmail { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class AppointmentSummaryDto
    {
        public int TotalAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int ApprovedAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int RejectedAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<AppointmentDto> RecentAppointments { get; set; } = new List<AppointmentDto>();
    }

    public class PaymentDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }

    public class CreatePaymentDto
    {
        [Required]
        public int AppointmentId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [StringLength(10)]
        public string Currency { get; set; } = "USD";

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class ProcessPaymentDto
    {
        [Required]
        public string PaymentIntentId { get; set; } = string.Empty;

        [Required]
        public string PaymentMethodId { get; set; } = string.Empty;
    }

    public class EventTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? BasePrice { get; set; }
        public int EstimatedDurationHours { get; set; }
    }

    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
