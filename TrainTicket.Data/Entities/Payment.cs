using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int TicketId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? Status { get; set; }

    public string? TransactionId { get; set; }

    public string? GatewayRef { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? RefundedAt { get; set; }

    public decimal? RefundAmount { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
