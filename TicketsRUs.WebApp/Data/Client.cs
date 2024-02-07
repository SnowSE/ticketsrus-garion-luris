using System;
using System.Collections.Generic;

namespace TicketsRUs.WebApp.Data;

public partial class Client
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
