using System;
using System.Data;
using System.Threading.Tasks;
using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    public interface ITicketService
    {
        Task<BookTicketResultDto?> BookTicketAsync(BookTicketRequestDto request);
        Task<CancelTicketResultDto> CancelTicketAsync(int ticketId, int userId, string? cancelReason = null);
        Task<bool> ConfirmPaymentAsync(int ticketId, string? transactionId = null);
        Task<DataTable> GetTicketsAsync(int? userId = null, string? status = null, DateTime? from = null, DateTime? to = null, string? ticketCode = null);
        Task<bool> CheckInAsync(string ticketCode);
        Task<decimal> CalculatePriceAsync(int scheduleId, string seatType, string? discountCode);

        // ĐỊNH NGHĨA CHUẨN: Đồng bộ trả về BookTicketResultDto giống lớp Service
        Task<BookTicketResultDto?> GetTicketByIdAsync(int ticketId);
    }
}