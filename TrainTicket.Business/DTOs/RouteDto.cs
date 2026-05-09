using System;

namespace TrainTicket.Business.DTOs
{
    public class RouteDto
    {
        public int RouteID { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public int DepartureStation { get; set; }
        public int ArrivalStation { get; set; }
        public decimal? Distance { get; set; }
        public string RouteType { get; set; } = "Th??ng";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string RegionCode { get; set; } = "HQ";

        // Optional navigation properties info if needed for UI mapping
        public string? DepartureStationName { get; set; }
        public string? ArrivalStationName { get; set; }
    }
}
