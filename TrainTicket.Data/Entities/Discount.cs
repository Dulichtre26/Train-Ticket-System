using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Discounts")]
    public class Discount
    {
        [Key]
        public int DiscountID { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        [Required, MaxLength(20)]
        public string DiscountType { get; set; } = string.Empty; // e.g., "Percentage", "Fixed"

        [Required]
        public decimal Amount { get; set; }

        public decimal MinPrice { get; set; } = 0;

        public int? MaxUses { get; set; }

        public int UsedCount { get; set; } = 0;

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}