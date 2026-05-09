using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Stations")]
    public class Station
    {
        [Key]
        public int StationID { get; set; }

        [Required, MaxLength(10)]
        public string StationCode { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string StationName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";
    }
}
