using System.Data;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.ADO;

namespace TrainTicket.Business.Services
{
    // Service truy v?n l?ch trình/s? ?? gh? b?ng ADO.NET + stored procedures.
    public class ScheduleService : IScheduleService
    {
        private readonly AdoHelper _adoHelper;

        public ScheduleService(AdoHelper adoHelper)
        {
            _adoHelper = adoHelper;
        }

        public async Task<DataTable> SearchSchedulesAsync(SearchScheduleDto request)
        {
            // Map DTO -> tham s? SP tm chuy?n.
            var parameters = new Dictionary<string, object?>
            {
                ["@GaDi"] = request.GaDi,
                ["@GaDen"] = request.GaDen,
                ["@NgayDi"] = request.NgayDi.Date
            };

            // Th?c thi SP v tr? DataTable ?? bind tr?c ti?p DataGridView.
            var result = await _adoHelper.ExecuteStoredProcedureAsync("sp_TimChuyenTau", parameters);
            return result;
        }

        public async Task<List<SeatMapDto>> GetSeatMapAsync(int scheduleId)
        {
            // Tham s? cho SP s? ?? gh?.
            var parameters = new Dictionary<string, object?>
            {
                ["@ScheduleID"] = scheduleId
            };

            // ??c d? li?u t? DB.
            var table = await _adoHelper.ExecuteStoredProcedureAsync("sp_XemSoDoGhe", parameters);

            // Map t?ng DataRow thnh DTO m?nh ki?u ?? Form d? s? d?ng.
            var result = table.Rows.Cast<DataRow>().Select(row => new SeatMapDto
            {
                MaToa = row["MaToa"]?.ToString() ?? string.Empty,
                LoaiToa = row["LoaiToa"]?.ToString() ?? string.Empty,
                SoGhe = row["SoGhe"]?.ToString() ?? string.Empty,
                LoaiGhe = row["LoaiGhe"]?.ToString() ?? string.Empty,
                SeatID = row["SeatID"] == DBNull.Value ? 0 : Convert.ToInt32(row["SeatID"]),
                TrangThai = row["TrangThai"]?.ToString() ?? string.Empty,
                GiaVe = row["GiaVe"] == DBNull.Value ? 0 : Convert.ToDecimal(row["GiaVe"])
            }).ToList();

            return result;
        }
    }
}
