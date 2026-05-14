// ============================================================
// FILE: TrainTicket.Business/DTOs/UserSessionDto.cs
// NÂNG C?P: Thêm Avatar, Permissions cache
// ============================================================
namespace TrainTicket.Business.DTOs
{
    public class UserSessionDto
    {
        public int          UserID      { get; set; }
        public string       FullName    { get; set; } = string.Empty;
        public string       Email       { get; set; } = string.Empty;
        public bool         IsActive    { get; set; }
        public List<string> Roles       { get; set; } = new();
        public DateTime     LoginAt     { get; set; } = DateTime.Now;  // [M?I]
        public string       AvatarLetter => FullName.Length > 0 ? FullName[0].ToString().ToUpper() : "?";
        public object       Permissions  { get; set; } = null!; // [M?I]
        // Helpers phân quy?n
        public bool IsAdmin   => Roles.Contains("Admin");
        public bool IsStaff   => Roles.Contains("Staff") || IsAdmin;
        public bool IsCustomer => Roles.Contains("Customer");
    }
}
