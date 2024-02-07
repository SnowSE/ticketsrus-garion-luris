using System;
using System.Collections.Generic;

namespace TicketsRUs.ClassLib.Data;

public partial class AvailableEvent
{
    public int Id { get; set; }

    public DateTime? StartTime { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
