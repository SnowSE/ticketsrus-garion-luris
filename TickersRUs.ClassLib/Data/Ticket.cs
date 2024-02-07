using System;
using System.Collections.Generic;

namespace TicketsRUs.Classlib.Data;

public partial class Ticket
{
    public int Id { get; set; }

    public string? Seat { get; set; }

    public bool? Scanned { get; set; }

    public int? EventId { get; set; }

    public virtual AvailableEvent? Event { get; set; }

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
