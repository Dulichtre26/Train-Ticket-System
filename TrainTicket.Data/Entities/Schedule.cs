using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Schedules")]
    public class Schedule
    {
        [Key]
        public int ScheduleID { get; set; }

        public int TrainID { get; set; }
        public int RouteID { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        [MaxLength(30)]
        public string Status { get; set; } = "Scheduled";

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";

        // Navigation
        public Train Train { get; set; } = null!;
        public Route Route { get; set; } = null!;
        public ICollection<SchedulePrice> SchedulePrices { get; set; } = new List<SchedulePrice>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
