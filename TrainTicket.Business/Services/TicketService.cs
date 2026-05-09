using System.Data;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.ADO;

namespace TrainTicket.Business.Services
{
    public class TicketService : ITicketService
    {
        private readonly AdoHelper _adoHelper;

        public TicketService(AdoHelper adoHelper)
        {
            _adoHelper = adoHelper;
        }

        public async Task<BookTicketResultDto?> BookTicketAsync(BookTicketRequestDto request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@UserID"] = request.UserID,
                ["@ScheduleID"] = request.ScheduleID,
                ["@SeatID"] = request.SeatID,
                ["@PassengerName"] = request.PassengerName,
                ["@PassengerID"] = request.PassengerID,
                ["@PassengerPhone"] = request.PassengerPhone,
                ["@SeatType"] = request.SeatType,
                ["@PaymentMethod"] = request.PaymentMethod
            };

            var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_DatVe", parameters);
            if (table.Rows.Count == 0)
            {
                return null;
            }

            var row = table.Rows[0];
            var result = new BookTicketResultDto
            {
                TicketID = row["TicketID"] == DBNull.Value ? 0 : Convert.ToInt32(row["TicketID"]),
                TicketCode = row["TicketCode"]?.ToString() ?? string.Empty,
                PassengerName = row["PassengerName"]?.ToString() ?? string.Empty,
                SeatType = row["SeatType"]?.ToString() ?? string.Empty,
                GiaVe = row["GiaVe"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaVe"]),
                TrangThaiVe = row["TrangThaiVe"]?.ToString() ?? string.Empty,
                GioDi = row["GioDi"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["GioDi"]),
                GioDen = row["GioDen"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["GioDen"]),
                GaDi = row["GaDi"]?.ToString() ?? string.Empty,
                GaDen = row["GaDen"]?.ToString() ?? string.Empty,
                MaTau = row["MaTau"]?.ToString() ?? string.Empty,
                MaToa = row["MaToa"]?.ToString() ?? string.Empty,
                SoGhe = row["SoGhe"]?.ToString() ?? string.Empty
            };

            return result;
        }

        public async Task<bool> CancelTicketAsync(int ticketId, int userId, string? cancelReason = null)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@TicketID"] = ticketId,
                ["@UserID"] = userId,
                ["@CancelReason"] = cancelReason
            };

            var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_HuyVe", parameters);
            return table.Rows.Count > 0;
        }

        public async Task<bool> ConfirmPaymentAsync(int ticketId, string? transactionId = null)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@TicketID"] = ticketId,
                ["@TransactionID"] = transactionId
            };

            var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_XacNhanThanhToan", parameters);
            return table.Rows.Count > 0;
        }

        public async Task<List<TicketHistoryDto>> GetTicketsAsync(int? userId = null, string? status = null)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@UserID"] = userId
            };

            var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_LichSuVeKhachHang", parameters);
            var result = new List<TicketHistoryDto>();

            foreach (DataRow row in table.Rows)
            {
                var dto = new TicketHistoryDto();
                if (table.Columns.Contains("TicketID") && row["TicketID"] != DBNull.Value) dto.TicketID = Convert.ToInt32(row["TicketID"]);
                if (table.Columns.Contains("TicketCode") && row["TicketCode"] != DBNull.Value) dto.TicketCode = row["TicketCode"].ToString() ?? "";
                if (table.Columns.Contains("PassengerName") && row["PassengerName"] != DBNull.Value) dto.PassengerName = row["PassengerName"].ToString() ?? "";
                if (table.Columns.Contains("SeatType") && row["SeatType"] != DBNull.Value) dto.SeatType = row["SeatType"].ToString() ?? "";
                if (table.Columns.Contains("GiaVe") && row["GiaVe"] != DBNull.Value) dto.GiaVe = Convert.ToDecimal(row["GiaVe"]);
                if (table.Columns.Contains("TrangThaiVe") && row["TrangThaiVe"] != DBNull.Value) dto.TrangThaiVe = row["TrangThaiVe"].ToString() ?? "";
                if (table.Columns.Contains("GioDi") && row["GioDi"] != DBNull.Value) dto.GioDi = Convert.ToDateTime(row["GioDi"]);
                if (table.Columns.Contains("GioDen") && row["GioDen"] != DBNull.Value) dto.GioDen = Convert.ToDateTime(row["GioDen"]);
                if (table.Columns.Contains("GaDi") && row["GaDi"] != DBNull.Value) dto.GaDi = row["GaDi"].ToString() ?? "";
                if (table.Columns.Contains("GaDen") && row["GaDen"] != DBNull.Value) dto.GaDen = row["GaDen"].ToString() ?? "";
                if (table.Columns.Contains("MaTau") && row["MaTau"] != DBNull.Value) dto.MaTau = row["MaTau"].ToString() ?? "";
                if (table.Columns.Contains("MaToa") && row["MaToa"] != DBNull.Value) dto.MaToa = row["MaToa"].ToString() ?? "";
                if (table.Columns.Contains("SoGhe") && row["SoGhe"] != DBNull.Value) dto.SoGhe = row["SoGhe"].ToString() ?? "";
                if (table.Columns.Contains("Status") && row["Status"] != DBNull.Value) dto.Status = row["Status"].ToString() ?? "";
                if (table.Columns.Contains("RegionCode") && row["RegionCode"] != DBNull.Value) dto.RegionCode = row["RegionCode"].ToString() ?? "";

                // Gi? tính t??ng thích: N?u không có Status nh?ng có TrangThaiVe thì map qua (ho?c ng??c l?i)
                if (string.IsNullOrEmpty(dto.Status) && !string.IsNullOrEmpty(dto.TrangThaiVe))
                {
                    dto.Status = dto.TrangThaiVe;
                }

                result.Add(dto);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                result = result.Where(x => x.TrangThaiVe == status || x.Status == status).ToList();
            }

            return result;
        }
    }
}
