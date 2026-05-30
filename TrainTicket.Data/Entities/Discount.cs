using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Discount
{
    public int DiscountId { get; set; }

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public string DiscountType { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal? MinPrice { get; set; }

    public int? MaxUses { get; set; }

    public int? UsedCount { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }
}
