using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("RefreshTokens")]
    public class RefreshToken
    {
        [Key]
        public int TokenID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiresAt { get; set; }

        public bool IsRevoked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(255)]
        public string? DeviceInfo { get; set; }

        // Navigation
        public User? User { get; set; }
    }
}