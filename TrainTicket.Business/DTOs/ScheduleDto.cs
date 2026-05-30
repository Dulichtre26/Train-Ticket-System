using System;

namespace TrainTicket.Business.DTOs
{
    public class ScheduleDto
    {
        public int ScheduleId { get; set; }
        public int TrainId { get; set; }
        public int RouteId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Status { get; set; } = "Scheduled";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation info for UI
        public string? TrainName { get; set; }
        public string? RouteName { get; set; }
    }
}
