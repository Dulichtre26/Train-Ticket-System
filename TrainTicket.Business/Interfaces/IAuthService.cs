using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    // Service nghi?p v? xác th?c ng??i dùng.
    public interface IAuthService
    {
        // Trả về thông tin phiên đăg nhập nếu hợp lý; ngc lại trả null. Trả về thông tin phiên đăg nhập nếu khó null.
        Task<UserSessionDto?> LoginAsync(LoginRequestDto request);

        // đăng ký tài khoản mới.
        Task<bool> RegisterAsync(RegisterRequestDto request);

        // ??i m?t kh?u
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);

        // Mã khóa tài khoản (Admin)
        Task<bool> UnlockAccountAsync(int userId);
    }
}
