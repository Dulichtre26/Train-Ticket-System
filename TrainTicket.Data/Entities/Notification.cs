using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        public int NotiID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Type { get; set; } = "Info";

        public bool IsRead { get; set; } = false;

        public int? RelatedID { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public User? User { get; set; }
    }
}