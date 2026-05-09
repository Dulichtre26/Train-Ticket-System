using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        public int TicketID { get; set; }

        [Column(TypeName = "decimal(12,0)")]
        public decimal Amount { get; set; }

        [Required, MaxLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        [MaxLength(100)]
        public string? TransactionID { get; set; }

        public DateTime? PaidAt { get; set; }

        [MaxLength(200)]
        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Ticket Ticket { get; set; } = null!;
    }
}
