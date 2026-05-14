using System.Data;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.ADO;

namespace TrainTicket.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly AdoHelper _adoHelper;

        public ReportService(AdoHelper adoHelper)
        {
            _adoHelper = adoHelper;
        }

        public async Task<DataTable> GetRevenueReportAsync(ReportFilterDto filter)
        {
            var parameters = new Dictionary<string, object?>
            {
                ["@Nam"] = filter.Year,
                ["@Thang"] = filter.Month,
                ["@RouteID"] = filter.RouteID
            };

            // Lưu ý: Đảm bảo sp_BaoCaoDoanhThu trong SQL cũng đã cập nhật lọc 'Confirmed' thay vì 'Paid'
            return await _adoHelper.ExecuteStoredProcedureAsync("sp_BaoCaoDoanhThu", parameters);
        }

        public async Task<DataTable> GetTopRoutesAsync(int year, int topN = 5)
        {
            var parameters = new Dictionary<string, object?> { ["@Nam"] = year, ["@TopN"] = topN };

            // CẬP NHẬT: Đồng bộ điều kiện Status IN ('Confirmed', 'Used')
            return await _adoHelper.ExecuteQueryAsync(
                @"SELECT TOP (@TopN)
                    r.RouteName, COUNT(*) AS SoVe, SUM(t.FinalPrice) AS DoanhThu
                  FROM Tickets t
                  JOIN Schedules sc ON t.ScheduleID = sc.ScheduleID
                  JOIN Routes    r  ON sc.RouteID   = r.RouteID
                  WHERE YEAR(t.BookedAt)=@Nam AND t.Status IN ('Confirmed','Used')
                  GROUP BY r.RouteName ORDER BY DoanhThu DESC", parameters);
        }

        public async Task<DataTable> GetDailySummaryAsync(DateTime date)
        {
            var parameters = new Dictionary<string, object?> { ["@Date"] = date.Date };

            // CẬP NHẬT: Sử dụng Status IN ('Confirmed', 'Used') để tính doanh thu thực tế
            return await _adoHelper.ExecuteQueryAsync(
                @"SELECT
                    COUNT(CASE WHEN Status IN ('Confirmed','Used') THEN 1 END) AS SoVe,
                    SUM(CASE WHEN Status IN ('Confirmed','Used') THEN FinalPrice ELSE 0 END) AS DoanhThu,
                    COUNT(CASE WHEN Status='Cancelled' THEN 1 END) AS SoVeHuy
                  FROM Tickets WHERE CAST(BookedAt AS DATE) = @Date", parameters);
        }
    }
}