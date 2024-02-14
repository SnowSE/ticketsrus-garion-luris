using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsRUs.Maui.SQLLITE;

namespace TicketsRUs.ClassLib.SQLLITE
{
    public class TicketModel
    {
        public int Id { get; set; }
        public int? EventId { get; set; }

        public bool? Scanned { get; set; }

        public string? Identifier { get; set; }

        public virtual AvailableEventModel? Event { get; set; }

    }
}
