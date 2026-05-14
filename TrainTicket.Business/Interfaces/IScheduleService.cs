using System.Data;
using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    // Service nghi?p v? tìm chuy?n và l?y s? ?? gh?.
    public interface IScheduleService
    {
        // G?i SP tìm chuy?n theo ga ?i/ga ??n/ngày ?i.
        Task<DataTable> SearchSchedulesAsync(SearchScheduleDto request);

        // G?i SP l?y tr?ng thái gh? c?a m?t chuy?n.
        Task<List<SeatMapDto>> GetSeatMapAsync(int scheduleId);

        // C?p nh?p tr?ng thái chuy?n
        Task<bool> UpdateScheduleStatusAsync(int scheduleId, string status, int? delayMinutes = null);
    }
}
