using System;
using System.Collections.Generic;

namespace TicketsRUs.WebApp.Data;

public partial class UserTicket
{
    public int Id { get; set; }

    public int? TicketId { get; set; }

    public int? ClientId { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Ticket? Ticket { get; set; }
}
