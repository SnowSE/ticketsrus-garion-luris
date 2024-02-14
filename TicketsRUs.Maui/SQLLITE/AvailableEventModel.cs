using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsRUs.ClassLib.SQLLITE;

namespace TicketsRUs.Maui.SQLLITE
{
    public class AvailableEventModel
    {
        public int Id { get; set; }

        public DateTime? StartTime { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<TicketModel> Tickets { get; set; } = new List<TicketModel>();
    }
}
