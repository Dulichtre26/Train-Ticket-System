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
        Task<CancelTicketResultDto> CancelTicketAsync(int ticketId, int userId, string? cancelReason = null);

        // Xác nh?n thanh toán cho vé ?ang Pending.
        Task<bool> ConfirmPaymentAsync(int ticketId, string? transactionId = null);

        // L?y danh sách vé (?? hi?n th? trên form qu?n lý vé / thanh toán)
        Task<DataTable> GetTicketsAsync(int? userId = null, string? status = null,
                                        DateTime? from = null, DateTime? to = null,
                                        string? ticketCode = null);

        // Th?c hi?n Check-in cho vé
        Task<bool> CheckInAsync(string ticketCode);              // [M?I]

        // Tính giá vé t?p h?p
        Task<decimal> CalculatePriceAsync(int scheduleId, string seatType, string? discountCode); // [M?I]
    }
}
