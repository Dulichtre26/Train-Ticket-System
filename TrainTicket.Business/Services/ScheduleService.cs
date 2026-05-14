using System.Data;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.ADO;

namespace TrainTicket.Business.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly AdoHelper _adoHelper;

        public ScheduleService(AdoHelper adoHelper)
        {
            _adoHelper = adoHelper;
        }

        public async Task<DataTable> SearchSchedulesAsync(SearchScheduleDto request)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@GaDi"] = request.GaDi,
                ["@GaDen"] = request.GaDen,
                ["@NgayDi"] = request.NgayDi.Date
            };

            return await _adoHelper.ExecuteStoredProcedureAsync("sp_TimChuyenTau", parameters);
        }

        public async Task<List<SeatMapDto>> GetSeatMapAsync(int scheduleId)
        {
            var parameters = new Dictionary<string, object?> { ["@ScheduleID"] = scheduleId };
            var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_XemSoDoGhe", parameters);

            // CẢI TIẾN: Sử dụng row.Field<T> để xử lý DBNull an toàn cho các trường chuỗi và số
            var result = table.Rows.Cast<DataRow>().Select(row => new SeatMapDto
            {
                // Sử dụng toán tử ?? để gán chuỗi rỗng nếu giá trị trong DB là null
                MaToa = row.Field<string>("MaToa") ?? string.Empty,
                LoaiToa = row.Field<string>("LoaiToa") ?? string.Empty,
                SoGhe = row.Field<string>("SoGhe") ?? string.Empty,
                LoaiGhe = row.Field<string>("LoaiGhe") ?? string.Empty,
                HangGhe = row.Field<string>("HangGhe") ?? "Economy",

                // Kiểm tra DBNull trước khi chuyển đổi kiểu dữ liệu Boolean và Int
                HasSocket = row["CoO_Cam"] != DBNull.Value && Convert.ToBoolean(row["CoO_Cam"]),
                SeatID = row["SeatID"] == DBNull.Value ? 0 : Convert.ToInt32(row["SeatID"]),
                TrangThai = row.Field<string>("TrangThai") ?? string.Empty,
                GiaVe = row["GiaVe"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaVe"])
            }).ToList();

            return result;
        }

        public async Task<bool> UpdateScheduleStatusAsync(int scheduleId, string status, int? delayMinutes = null)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@ScheduleID"] = scheduleId,
                ["@Status"] = status,
                ["@DelayMinutes"] = (object?)delayMinutes ?? DBNull.Value
            };

            await _adoHelper.ExecuteNonQueryAsync(
                "UPDATE Schedules SET Status=@Status, DelayMinutes=ISNULL(@DelayMinutes,DelayMinutes) WHERE ScheduleID=@ScheduleID", parameters);

            return true;
        }
    }
}