using System;

namespace TrainTicket.Business.DTOs
{
    public class RouteDto
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public int DepartureStation { get; set; }
        public int ArrivalStation { get; set; }
        public decimal? Distance { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Optional navigation properties info if needed for UI mapping
        public string? DepartureStationName { get; set; }
        public string? ArrivalStationName { get; set; }
    }
}
