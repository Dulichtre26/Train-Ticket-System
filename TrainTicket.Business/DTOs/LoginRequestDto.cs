namespace TrainTicket.Business.DTOs
{
    // DTO dùng cho màn hình ??ng nh?p.
    // Form g?i Email + Password sang Business Layer qua object này.
    public class LoginRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
