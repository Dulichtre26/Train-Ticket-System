using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class SchedulePrice
{
    public int PriceId { get; set; }

    public int ScheduleId { get; set; }

    public string SeatType { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Schedule Schedule { get; set; } = null!;
}
