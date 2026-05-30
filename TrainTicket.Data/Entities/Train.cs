using System;
using System.Collections.Generic;

namespace TrainTicket.Data.Entities;

public partial class Train
{
    public int TrainId { get; set; }

    public string TrainCode { get; set; } = null!;

    public string TrainName { get; set; } = null!;

    public string TrainType { get; set; } = null!;

    public int? MaxSpeed { get; set; }

    public string? Manufacturer { get; set; }

    public int? YearBuilt { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Carriage> Carriages { get; set; } = new List<Carriage>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
