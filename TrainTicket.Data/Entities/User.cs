using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? IDNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        [MaxLength(10)]
        public string? Gender { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [MaxLength(20)]
        public string RegionCode { get; set; } = "HQ";

        // Navigation
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}
