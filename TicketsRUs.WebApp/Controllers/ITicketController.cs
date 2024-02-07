using TicketsRUs.WebApp.Data;

namespace TicketsRUs.WebApp.Controllers;

public interface ITicketController
{
    Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents();
    Task<AvailableEvent> GetAvailableEvent(int id);
    Task<IEnumerable<Client>> GetAllClients();
    Task<Client> GetClient(string email);
    Task<IEnumerable<Ticket>> GetAllTickets();
    Task<Ticket> GetTicket(int id);
}
