using TicketsRUs.Classlib.Data;

namespace TicketsRUs.Classlib.Services;

public interface ITicketService
{
    Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents();
    Task<AvailableEvent> GetAvailableEvent(int id);
    Task<IEnumerable<Client>> GetAllClients();
    Task<Client> GetClient(string email);
    Task<IEnumerable<Ticket>> GetAllTickets();
    Task<Ticket> GetTicket(int id);
}
