using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("SchedulePrices")]
    public class SchedulePrice
    {
        [Key]
        public int PriceID { get; set; }

        public int ScheduleID { get; set; }

        [Required, MaxLength(50)]
        public string SeatType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(12,0)")]
        public decimal Price { get; set; }

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";

        // Navigation
        public Schedule Schedule { get; set; } = null!;
    }
}
