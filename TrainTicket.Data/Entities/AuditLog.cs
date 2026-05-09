using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        public int LogID { get; set; }

        public int? UserID { get; set; }

        [Required, MaxLength(100)]
        public string Action { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? TableName { get; set; }

        public int? RecordID { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        [MaxLength(45)]
        public string? IPAddress { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public User? User { get; set; }
    }
}