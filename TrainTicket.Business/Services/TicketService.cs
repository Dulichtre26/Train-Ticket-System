using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.ADO;

namespace TrainTicket.Business.Services
{
    public class TicketService : ITicketService
    {
        private readonly AdoHelper _adoHelper;

        public TicketService(AdoHelper adoHelper) => _adoHelper = adoHelper;

        // CẢI TIẾN: Sử dụng await trực tiếp với ExecuteStoredProcedureAsync
        public async Task<BookTicketResultDto?> BookTicketAsync(BookTicketRequestDto request)
        {
            var p = new Dictionary<string, object?>
            {
                ["@UserID"] = request.UserID,
                ["@ScheduleID"] = request.ScheduleID,
                ["@SeatIDs"] = request.SeatID.ToString(),
                ["@PassengerNames"] = request.PassengerName,
                ["@PassengerIDs"] = request.PassengerID,
                ["@PassengerPhones"] = (object?)request.PassengerPhone ?? DBNull.Value,
                ["@PaymentMethod"] = request.PaymentMethod,
                ["@DiscountCode"] = (object?)request.DiscountCode ?? DBNull.Value,
            };

            // Tận dụng phương thức Async trong AdoHelper
            var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_DatVe", p);
            if (table.Rows.Count == 0) return null;

            var row = table.Rows[0];
            return new BookTicketResultDto
            {
                TicketID = row.Field<int>("TicketID"),
                TicketCode = row.Field<string>("TicketCode") ?? "",
                PassengerName = row.Field<string>("PassengerName") ?? "",
                GiaVe = row.Field<decimal>("FinalPrice"),
                OriginalPrice = row.Field<decimal>("FinalPrice"),
                DiscountAmount = 0,
                SoGhe = row.Field<string>("SeatNumber") ?? "",
                MaToa = row.Field<string>("CarriageCode") ?? "",
                GaDi = row.Field<string>("GaDi") ?? "",
                GaDen = row.Field<string>("GaDen") ?? "",
                SeatType = "",
                TrangThaiVe = "Pending",
                GioDi = DateTime.MinValue,
                GioDen = DateTime.MinValue,
                MaTau = "",
            };
        }

        public async Task<CancelTicketResultDto> CancelTicketAsync(int ticketId, int userId, string? cancelReason = null)
        {
            var p = new Dictionary<string, object?>
            {
                ["@TicketID"] = ticketId,
                ["@UserID"] = userId,
                ["@CancelReason"] = (object?)cancelReason ?? DBNull.Value
            };

            try
            {
                var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_HuyVe", p);
                if (table.Rows.Count == 0)
                    return new CancelTicketResultDto { Message = "Không thể hủy vé.", Success = false };

                var row = table.Rows[0];
                return new CancelTicketResultDto
                {
                    Success = row.Field<int>("Success") == 1,
                    RefundAmount = row.Field<decimal>("RefundAmount"),
                    RefundPercent = row.Field<decimal>("RefundPercent"),
                    Message = $"Hoàn tiền {row.Field<decimal>("RefundPercent"):F0}% = {row.Field<decimal>("RefundAmount"):N0} VNĐ"
                };
            }
            catch (Exception ex)
            {
                return new CancelTicketResultDto { Message = ex.Message, Success = false };
            }
        }

        public async Task<bool> ConfirmPaymentAsync(int ticketId, string? transactionId = null)
        {
            var p = new Dictionary<string, object?>
            {
                ["@TicketID"] = ticketId,
                ["@TransactionID"] = (object?)transactionId ?? DBNull.Value
            };

            var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_XacNhanThanhToan", p);
            return table.Rows.Count > 0;
        }

        public async Task<DataTable> GetTicketsAsync(
            int? userId = null, string? status = null,
            DateTime? from = null, DateTime? to = null, string? ticketCode = null)
        {
            var p = new Dictionary<string, object?>
            {
                ["@UserID"] = (object?)userId ?? DBNull.Value,
                ["@Status"] = (object?)status ?? DBNull.Value,
                ["@TuNgay"] = (object?)(from?.Date) ?? DBNull.Value,
                ["@DenNgay"] = (object?)(to?.Date) ?? DBNull.Value,
                ["@MaVe"] = (object?)ticketCode ?? DBNull.Value,
            };

            return await _adoHelper.ExecuteStoredProcedureAsync("sp_LayDanhSachVe", p);
        }

        public async Task<bool> CheckInAsync(string ticketCode)
        {
            var p = new Dictionary<string, object?> { ["@TicketCode"] = ticketCode };
            try
            {
                var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_CheckIn", p);
                return table.Rows.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<decimal> CalculatePriceAsync(int scheduleId, string seatType, string? discountCode)
        {
            var p = new Dictionary<string, object?>
            {
                ["@ScheduleID"] = scheduleId,
                ["@SeatType"] = seatType,
                ["@DiscountCode"] = (object?)discountCode ?? DBNull.Value
            };

            var table = await _adoHelper.ExecuteQueryAsync(
                @"SELECT dbo.fn_TinhGiaVe(
                    (SELECT Price FROM SchedulePrices WHERE ScheduleID=@ScheduleID AND SeatType=@SeatType),
                    @DiscountCode) AS FinalPrice", p);

            if (table.Rows.Count == 0 || table.Rows[0]["FinalPrice"] == DBNull.Value)
                return 0m;

            return Convert.ToDecimal(table.Rows[0]["FinalPrice"]);
        }

        /// <summary>
        /// XỬ LÝ TRUY VẤN: Lấy chi tiết vé phục vụ luồng hiển thị mã QR thanh toán
        /// </summary>
        public async Task<BookTicketResultDto?> GetTicketByIdAsync(int ticketId)
        {
            var p = new Dictionary<string, object?>
            {
                ["@TicketID"] = ticketId
            };

            // Truy vấn trực tiếp từ cơ sở dữ liệu bảng Tickets
            var table = await _adoHelper.ExecuteQueryAsync(
                @"SELECT TicketID, TicketCode, FinalPrice 
                  FROM Tickets 
                  WHERE TicketID = @TicketID", p);

            if (table.Rows.Count == 0) return null;

            var row = table.Rows[0];

            // Đã xóa thuộc tính 'FinalPrice' không hợp lệ tại đây để tránh lỗi biên dịch
            return new BookTicketResultDto
            {
                TicketID = row.Field<int>("TicketID"),
                TicketCode = row.Field<string>("TicketCode") ?? "",
                GiaVe = row.Field<decimal>("FinalPrice")
            };
        }
    }
}