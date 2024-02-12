using Microsoft.EntityFrameworkCore;
using TicketsRUs.ClassLib.Data;

namespace TicketsRUs.ClassLib.Services;

public class ApiTicketService(IDbContextFactory<PostgresContext> factory) : ITicketService
{
    public async Task CreateClient(string email)
    {
        var context = factory.CreateDbContext();

        context.Add(new Client() { 
            Id = await context.Clients.CountAsync() + 1,
            Email = email 
        });
        await context.SaveChangesAsync();
    }

    public async Task CreateTicket(string email, int event_id)
    {
        var context = factory.CreateDbContext();

        var client = await context.Clients
            .Where(c => c.Email == email)
            .FirstOrDefaultAsync();

        Ticket ticket = new Ticket()
        {
            Id = await context.Tickets.CountAsync() + 1,
            EventId = event_id,
            Scanned = false,
            Seat = $"A{await context.Tickets.Where(t => t.EventId == event_id).CountAsync() + 1}"
        };

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        UserTicket userTicket = new UserTicket()
        {
            Id = await context.UserTickets.CountAsync() + 1,
            TicketId = ticket.Id,
            ClientId = client.Id
        };
        context.UserTickets.Add(userTicket);
        await context.SaveChangesAsync();
    }

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

    public async Task<IEnumerable<UserTicket>> GetUserTicketAll()
    {
        var context = factory.CreateDbContext();
        return await context.UserTickets
            .Include(t => t.Ticket)
                .ThenInclude(e => e.Event)
            .Include(c => c.Client)
            .ToListAsync();
    }

    public async Task UpdateTicket(Ticket t)
    {
        var context = factory.CreateDbContext();
        context.Update(t);

        await context.SaveChangesAsync();
    }
}
