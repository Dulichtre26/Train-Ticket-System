using System.Data;
using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    // Service nghiep vu tìm chuyen và alay so ghe.
    public interface IScheduleService
    {
        // Gui SP tìm chuyen theo ga gai/ga den/ngày di.
        Task<DataTable> SearchSchedulesAsync(SearchScheduleDto request);

        // Gui SP lay trang thai ghe cua mot chuyen.
        Task<List<SeatMapDto>> GetSeatMapAsync(int scheduleId);

        // Cap nhat trang thai chuyen
        Task<bool> UpdateScheduleStatusAsync(int scheduleId, string status, int? delayMinutes = null);
    }
}
