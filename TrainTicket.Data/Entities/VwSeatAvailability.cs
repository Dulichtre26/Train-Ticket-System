using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class VwSeatAvailability
{
    public int ScheduleId { get; set; }

    public string CarriageCode { get; set; } = null!;

    public string CarriageType { get; set; } = null!;

    public int? Floor { get; set; }

    public int SeatId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public string SeatType { get; set; } = null!;

    public string? SeatClass { get; set; }

    public bool? HasSocket { get; set; }

    public decimal Price { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? TicketCode { get; set; }

    public string? PassengerName { get; set; }
}
