using SQLite;

using SQLiteNetExtensions.Attributes;

namespace TicketsRUs.ClassLib.Data;

public partial class Ticket
{
    [PrimaryKey]
    public int Id { get; set; }

    [ForeignKey(typeof(AvailableEvent))]
    public int? EventId { get; set; }

    public bool? Scanned { get; set; }

    public string? Identifier { get; set; }

    [ManyToOne]
    public virtual AvailableEvent? Event { get; set; }
}