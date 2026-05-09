using System.Data;
using TrainTicket.Business.DTOs;
using TrainTicket.Business.Interfaces;
using TrainTicket.Data.ADO;

namespace TrainTicket.Business.Services
{
    // Service báo cáo: l?y d? li?u doanh thu qua stored procedure.
    public class ReportService : IReportService
    {
        private readonly AdoHelper _adoHelper;

        public ReportService(AdoHelper adoHelper)
        {
            _adoHelper = adoHelper;
        }

        public async Task<DataTable> GetRevenueReportAsync(ReportFilterDto filter)
        {
            // Map b? l?c t? UI sang tham s? truy v?n bo co.
            var parameters = new Dictionary<string, object?>
            {
                ["@Nam"] = filter.Year,
                ["@Thang"] = filter.Month,
                ["@RouteID"] = filter.RouteID
            };

            // Th?c thi SP, tr? DataTable ?? bind DataGridView/Chart.
            var result = await _adoHelper.ExecuteStoredProcedureAsync("sp_BaoCaoDoanhThu", parameters);
            return result;
        }
    }
}
