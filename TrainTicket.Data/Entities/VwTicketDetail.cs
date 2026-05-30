using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class VwTicketDetail
{
    public int TicketId { get; set; }

    public string TicketCode { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? BookedAt { get; set; }

    public bool? CheckedIn { get; set; }

    public DateTime? CheckInAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string? CancelReason { get; set; }

    public string BookerName { get; set; } = null!;

    public string BookerEmail { get; set; } = null!;

    public string? BookerPhone { get; set; }

    public string PassengerName { get; set; } = null!;

    public string PassengerId { get; set; } = null!;

    public string? PassengerPhone { get; set; }

    public string SeatType { get; set; } = null!;

    public decimal OriginalPrice { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal FinalPrice { get; set; }

    public string? DiscountCode { get; set; }

    public DateTime GioDi { get; set; }

    public DateTime GioDen { get; set; }

    public string? KeGa { get; set; }

    public int? TrePhut { get; set; }

    public string? TripStatus { get; set; }

    public string MaTau { get; set; } = null!;

    public string TenTau { get; set; } = null!;

    public string TrainType { get; set; } = null!;

    public string GaDi { get; set; } = null!;

    public string MaGaDi { get; set; } = null!;

    public string GaDen { get; set; } = null!;

    public string MaGaDen { get; set; } = null!;

    public string MaToa { get; set; } = null!;

    public string LoaiToa { get; set; } = null!;

    public string SoGhe { get; set; } = null!;

    public string? PaymentMethod { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? PaidAt { get; set; }

    public string? TransactionId { get; set; }
}
