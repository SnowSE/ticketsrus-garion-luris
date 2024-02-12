using TicketsRUs.ClassLib.Data;

namespace TicketsRUs.ClassLib.Services;

public interface ITicketService
{
    Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents();
    Task<AvailableEvent> GetAvailableEvent(int id);
    Task<IEnumerable<Client>> GetAllClients();
    Task<Client> GetClient(string email);
    Task<IEnumerable<Ticket>> GetAllTickets();
    Task<Ticket> GetTicket(int id);
    Task<IEnumerable<UserTicket>> GetUserTicketAll();
    Task CreateTicket(string email, int event_id);
    Task CreateClient(string email);
    Task UpdateTicket(Ticket t);
}
