using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Trains")]
    public class Train
    {
        [Key]
        public int TrainID { get; set; }

        [Required, MaxLength(20)]
        public string TrainCode { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string TrainName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string TrainType { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";

        // Navigation
        public ICollection<Carriage> Carriages { get; set; } = new List<Carriage>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
