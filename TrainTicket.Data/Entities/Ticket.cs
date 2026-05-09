using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        [Required, MaxLength(20)]
        public string TicketCode { get; set; } = string.Empty;

        public int UserID { get; set; }
        public int ScheduleID { get; set; }
        public int SeatID { get; set; }

        [Required, MaxLength(100)]
        public string PassengerName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string PassengerID { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? PassengerPhone { get; set; }

        [Required, MaxLength(50)]
        public string SeatType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(12,0)")]
        public decimal OriginalPrice { get; set; }

        [Column(TypeName = "decimal(12,0)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(12,0)")]
        public decimal FinalPrice { get; set; }

        [MaxLength(50)]
        public string? DiscountCode { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public bool CheckedIn { get; set; } = false;
        public DateTime? CheckInAt { get; set; }
        public DateTime BookedAt { get; set; } = DateTime.Now;
        public DateTime? CancelledAt { get; set; }

        [MaxLength(255)]
        public string? CancelReason { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";

        // Navigation
        public User User { get; set; } = null!;
        public Schedule Schedule { get; set; } = null!;
        public Seat Seat { get; set; } = null!;
        public Payment? Payment { get; set; }
    }
}
