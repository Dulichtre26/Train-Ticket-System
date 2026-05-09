using System.Data;
using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    // Service nghi?p v? l?y d? li?u báo cáo.
    public interface IReportService
    {
        // L?y DataTable doanh thu d?a trên b? l?c ??u vào.
        Task<DataTable> GetRevenueReportAsync(ReportFilterDto filter);
    }
}
