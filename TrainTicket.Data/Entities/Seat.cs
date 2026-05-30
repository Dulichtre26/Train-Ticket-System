using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Seat
{
    public int SeatId { get; set; }

    public int CarriageId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public string SeatType { get; set; } = null!;

    public string? SeatClass { get; set; }

    public bool? HasSocket { get; set; }

    public bool? IsActive { get; set; }

    public virtual Carriage Carriage { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
