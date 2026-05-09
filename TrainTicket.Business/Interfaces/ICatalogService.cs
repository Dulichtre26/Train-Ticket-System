using TrainTicket.Business.DTOs;

namespace TrainTicket.Business.Interfaces
{
    public interface ICatalogService
    {
        // ?? TRAINS ??
        Task<List<TrainDto>> GetAllTrainsAsync();
        Task<TrainDto?> GetTrainByIdAsync(int id);
        Task<bool> SaveTrainAsync(TrainDto train);
        Task<bool> DeleteTrainAsync(int id);

        // ?? STATIONS ??
        Task<List<StationDto>> GetAllStationsAsync();
        Task<StationDto?> GetStationByIdAsync(int id);
        Task<bool> SaveStationAsync(StationDto station);
        Task<bool> DeleteStationAsync(int id);

        // ?? ROUTES ??
        Task<List<RouteDto>> GetAllRoutesAsync();
        Task<RouteDto?> GetRouteByIdAsync(int id);
        Task<bool> SaveRouteAsync(RouteDto route);
        Task<bool> DeleteRouteAsync(int id);

        // ?? SCHEDULES ??
        Task<List<ScheduleDto>> GetAllSchedulesAsync();
        Task<ScheduleDto?> GetScheduleByIdAsync(int id);
        Task<bool> SaveScheduleAsync(ScheduleDto schedule);
        Task<bool> DeleteScheduleAsync(int id);
    }
}
