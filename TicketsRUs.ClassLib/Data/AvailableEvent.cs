using SQLite;

using SQLiteNetExtensions.Attributes;

namespace TicketsRUs.ClassLib.Data;

public partial class AvailableEvent
{
    [PrimaryKey]
    public int Id { get; set; }

    public DateTime? StartTime { get; set; }

    public string? Name { get; set; }

    [OneToMany]
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}