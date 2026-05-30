using System.Data;
using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    // Service nghiep vu lay du lieu bao cao.
    public interface IReportService
    {
        // Lay DataTable doanh thu dua tren bo loc dau vao.
        Task<DataTable> GetRevenueReportAsync(ReportFilterDto filter);
        Task<DataTable> GetTopRoutesAsync(int year, int topN = 5);   // [Moi]
        Task<DataTable> GetDailySummaryAsync(DateTime date);         // [Moi]
    }
}
