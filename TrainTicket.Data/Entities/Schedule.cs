using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int TrainId { get; set; }

    public int RouteId { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public string? Platform { get; set; }

    public int? DelayMinutes { get; set; }

    public string? Status { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Route Route { get; set; } = null!;

    public virtual ICollection<SchedulePrice> SchedulePrices { get; set; } = new List<SchedulePrice>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual Train Train { get; set; } = null!;
}
