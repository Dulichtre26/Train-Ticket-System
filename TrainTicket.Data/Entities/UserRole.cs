using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainTicket.Data.Entities
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;

        // Navigation
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
