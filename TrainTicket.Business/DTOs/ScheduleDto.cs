using System;

namespace TrainTicket.Business.DTOs
{
    public class ScheduleDto
    {
        public int ScheduleID { get; set; }
        public int TrainID { get; set; }
        public int RouteID { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string Status { get; set; } = "Scheduled";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string RegionCode { get; set; } = "HQ";

        // Navigation info for UI
        public string? TrainName { get; set; }
        public string? RouteName { get; set; }
    }
}
