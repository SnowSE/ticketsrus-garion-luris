using Microsoft.EntityFrameworkCore;
using TicketsRUs.WebApp.Data;

namespace TicketsRUs.WebApp.Controllers;

public class ApiTicketController : ITicketController
{
    PostgresContext _context = new();

    public async Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents()
    {
        return await _context.AvailableEvents.ToListAsync();
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetAllTickets()
    {
        return await _context.Tickets.ToListAsync();
    }

    public async Task<AvailableEvent> GetAvailableEvent(int id)
    {
        return await _context.AvailableEvents
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Client> GetClient(string email)
    {
        return await _context.Clients
            .Where(e => e.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<Ticket> GetTicket(int id)
    {
        return await _context.Tickets
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }
}
