using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Notification
{
    public int NotiId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string? Type { get; set; }

    public bool? IsRead { get; set; }

    public int? RelatedId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
