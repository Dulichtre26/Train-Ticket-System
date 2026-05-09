using System.Data;
using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    // Service nghi?p v? liên quan vòng ??i vé: ??t/h?y/xác nh?n thanh toán.
    public interface ITicketService
    {
        // Th?c hi?n ??t vé, tr? thông tin vé v?a t?o.
        Task<BookTicketResultDto?> BookTicketAsync(BookTicketRequestDto request);

        // H?y vé theo quy?n và quy t?c nghi?p v? trong SP.
        Task<bool> CancelTicketAsync(int ticketId, int userId, string? cancelReason = null);

        // Xác nh?n thanh toán cho vé ?ang Pending.
        Task<bool> ConfirmPaymentAsync(int ticketId, string? transactionId = null);

        // L?y danh sách vé (?? hi?n th? trên form qu?n lý vé / thanh toán)
        Task<List<TicketHistoryDto>> GetTicketsAsync(int? userId = null, string? status = null);
    }
}
