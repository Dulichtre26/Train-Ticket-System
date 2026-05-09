using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Carriages")]
    public class Carriage
    {
        [Key]
        public int CarriageID { get; set; }

        public int TrainID { get; set; }

        [Required, MaxLength(10)]
        public string CarriageCode { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string CarriageType { get; set; } = string.Empty;

        public int TotalSeats { get; set; }
        public bool IsActive { get; set; } = true;

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";

        // Navigation
        public Train Train { get; set; } = null!;
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
