using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Seats")]
    public class Seat
    {
        [Key]
        public int SeatID { get; set; }

        public int CarriageID { get; set; }

        [Required, MaxLength(10)]
        public string SeatNumber { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string SeatType { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";

        // Navigation
        public Carriage Carriage { get; set; } = null!;
    }
}
