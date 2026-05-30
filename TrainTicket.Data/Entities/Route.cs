using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Route
{
    public int RouteId { get; set; }

    public string RouteName { get; set; } = null!;

    public int DepartureStation { get; set; }

    public int ArrivalStation { get; set; }

    public decimal? Distance { get; set; }

    public decimal? EstimatedHours { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Station ArrivalStationNavigation { get; set; } = null!;

    public virtual Station DepartureStationNavigation { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
