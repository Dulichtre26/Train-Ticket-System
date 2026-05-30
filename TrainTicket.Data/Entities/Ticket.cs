using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Ticket
{
    public int TicketId { get; set; }

    public string TicketCode { get; set; } = null!;

    public int UserId { get; set; }

    public int ScheduleId { get; set; }

    public int SeatId { get; set; }

    public string PassengerName { get; set; } = null!;

    public string PassengerId { get; set; } = null!;

    public string? PassengerPhone { get; set; }

    public string SeatType { get; set; } = null!;

    public decimal OriginalPrice { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalPrice { get; set; }

    public string? DiscountCode { get; set; }

    public string? Status { get; set; }

    public bool? CheckedIn { get; set; }

    public DateTime? CheckInAt { get; set; }

    public DateTime? BookedAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string? CancelReason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
