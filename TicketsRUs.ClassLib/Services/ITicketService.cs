using TicketsRUs.ClassLib.Data;

namespace TicketsRUs.ClassLib.Services;

public interface ITicketService
{
    Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents();
    Task<AvailableEvent> GetAvailableEvent(int id);
    Task<IEnumerable<Ticket>> GetAllTickets();
    Task<Ticket> GetTicket(int id);
    Task CreateTicket(int event_id);
    Task UpdateTicket(Ticket t);
}
