using Microsoft.EntityFrameworkCore;
using TicketsRUs.Classlib.Data;
using TicketsRUs.WebApp.Data;

namespace TicketsRUs.Classlib.Services;

public class ApiTicketService(IDbContextFactory<PostgresContext> factory) : ITicketService
{
    public async Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents()
    {
        var context = factory.CreateDbContext();
        return await context.AvailableEvents.ToListAsync();
    }

    public async Task<IEnumerable<Client>> GetAllClients()
    {
        var context = factory.CreateDbContext();
        return await context.Clients.ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetAllTickets()
    {
        var context = factory.CreateDbContext();
        return await context.Tickets.ToListAsync();
    }

    public async Task<AvailableEvent> GetAvailableEvent(int id)
    {
        var context = factory.CreateDbContext();
        return await context.AvailableEvents
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Client> GetClient(string email)
    {
        var context = factory.CreateDbContext();
        return await context.Clients
            .Where(e => e.Email == email)
            .FirstOrDefaultAsync();
    }

    public async Task<Ticket> GetTicket(int id)
    {
        var context = factory.CreateDbContext();
        return await context.Tickets
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }
}
