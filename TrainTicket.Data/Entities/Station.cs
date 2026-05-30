using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Station
{
    public int StationId { get; set; }

    public string StationCode { get; set; } = null!;

    public string StationName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? Address { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Route> RouteArrivalStationNavigations { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteDepartureStationNavigations { get; set; } = new List<Route>();
}
