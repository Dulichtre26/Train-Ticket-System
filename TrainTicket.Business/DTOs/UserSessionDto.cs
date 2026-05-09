namespace TrainTicket.Business.DTOs
{
    // DTO ??i di?n cho thông tin ng??i dùng sau khi ??ng nh?p thành công.
    // Object này s? ???c l?u trong SessionManager ?? dùng gi?a các Form.
    public class UserSessionDto
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
