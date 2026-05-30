namespace TrainTicket.Business.DTOs
{
    // DTO dùng cho màn hình đăng nhập
    // Form gửi Email + Password sang Business Layer qua object này.
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } 
        public string? Region { get; set; }   // HQ / North / Central / South
    }
}
