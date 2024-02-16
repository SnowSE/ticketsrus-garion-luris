using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
using TicketsRUs.Maui.Services;

namespace TicketsRUs.Maui.Controllers;

public class MauiTicketController
{
    ITicketService _localTicketService;

    public MauiTicketController(ITicketService apiService)
    {
        _localTicketService = new MauiTicketService();
    }

    public async Task<IEnumerable<Ticket>> GetAllTickets()
    {
        return await _localTicketService.GetAllTickets();
    }

    public async Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents()
    {
        return await _localTicketService.GetAllAvailableEvents();
    }

    public async Task Sync(
        List<AvailableEvent> apiEvents,
        List<AvailableEvent> localEvents,
        List<Ticket> apiTickets, 
        List<Ticket> localTickets)
    {
        await SyncEvents(apiEvents, localEvents);
        await SyncTickets(apiTickets, localTickets);
    }

    private async Task SyncEvents(List<AvailableEvent> apiEvents, List<AvailableEvent> localEvents)
    {
        foreach (AvailableEvent ae in apiEvents)
        {
            AvailableEvent? le = localEvents.Where(t => t.Id == ae.Id).Single();

            if (le == null)
            {
                await _localTicketService.CreateAvailableEvent(ae);
                continue;
            }
        }
    }

    private async Task SyncTickets(List<Ticket> apiTickets, List<Ticket> localTickets)
    {
        foreach (Ticket at in apiTickets)
        {
            Ticket? lt = localTickets.Where(t => t.Id == at.Id).Single();

            if (lt == null)
            {
                await _localTicketService.CreateTicket(at);
                continue;
            }

            bool isScanned = (lt.Scanned ?? false) || (at.Scanned ?? false);
            bool isDuplicateScan = (lt.Scanned ?? false) && (at.Scanned ?? false);

            if (isScanned)
            {
                lt.Scanned = true;
                await _localTicketService.UpdateTicket(lt);
            }

            if (isDuplicateScan)
            {
                // TODO: Do some logic here to notify that there was a duplicate scan
            }
        }
    }
}