using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    // Service nghi?p v? xác th?c ng??i dùng.
    public interface IAuthService
    {
        // Tr? v? thông tin phiên ??ng nh?p n?u h?p l?; ng??c l?i tr? null.
        Task<UserSessionDto?> LoginAsync(LoginRequestDto request);
    }
}
