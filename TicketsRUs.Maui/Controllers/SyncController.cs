using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;

namespace TicketsRUs.Maui.Controllers;

public class SyncController
{
    public string ConnectionString { get; set; } = "";
    public int SyncRate { get; set; } = 30; // seconds
    private ITicketService localTicketService;
    public SyncController(string conn, ITicketService localTicketService)
    {
        ConnectionString = conn;
        this.localTicketService = localTicketService;
        SyncLoop();
    }

    public void SyncLoop()
    {
        while (true)
        {
            Sync();
            Thread.Sleep(SyncRate*1000);
        }
    }

    public async void Sync()
    {
        HttpClient client = new HttpClient();

        var localEvents = (await localTicketService.GetAllAvailableEvents()).ToList();
        var localTickets = (await localTicketService.GetAllTickets()).ToList();
        List<AvailableEvent>? apiEvents = await client.GetFromJsonAsync<List<AvailableEvent>>($"{ConnectionString}/ApiTicket/events/");
        List<Ticket>? apiTickets = await client.GetFromJsonAsync<List<Ticket>>($"{ConnectionString}/ApiTicket/tickets/");

        if (apiEvents == null || apiTickets == null) { return; }

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
                await localTicketService.CreateAvailableEvent(ae);
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
                await localTicketService.CreateTicket(at);
                continue;
            }

            bool isScanned = (lt.Scanned ?? false) || (at.Scanned ?? false);
            bool isDuplicateScan = (lt.Scanned ?? false) && (at.Scanned ?? false);

            if (isScanned)
            {
                lt.Scanned = true;
                await localTicketService.UpdateTicket(lt);
            }

            if (isDuplicateScan)
            {
                // TODO: Do some logic here to notify that there was a duplicate scan
            }
        }
    }
}
