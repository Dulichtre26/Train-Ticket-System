using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Routes")]
    public class Route
    {
        [Key]
        public int RouteID { get; set; }

        [Required, MaxLength(200)]
        public string RouteName { get; set; } = string.Empty;

        public int DepartureStation { get; set; }
        public int ArrivalStation { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal? Distance { get; set; }

        [MaxLength(50)]
        public string RouteType { get; set; } = "Th??ng";

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";

        // Navigation
        public Station DepartureStationNav { get; set; } = null!;

        public Station ArrivalStationNav { get; set; } = null!;

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
