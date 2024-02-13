﻿using System;
using System.Collections.Generic;

namespace TicketsRUs.ClassLib.Data;

public partial class Ticket
{
    public int Id { get; set; }

    public int? EventId { get; set; }

    public bool? Scanned { get; set; }

    public string? Identifier { get; set; }

    public virtual AvailableEvent? Event { get; set; }
}
