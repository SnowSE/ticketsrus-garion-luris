using Microsoft.Maui.Controls;
using System.Net.Http.Json;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;

namespace TicketsRUs.Maui.Controllers;

public class SyncController
{
    public string ConnectionString { get; private set; } = "https://ticketsruswebapp20240216164850.azurewebsites.net";
    public int SyncRate { get; set; } = 5; // seconds
    private ITicketService localTicketService;
    public SyncController(string conn, ITicketService localTicketService)
    {
        ConnectionString = conn;
        this.localTicketService = localTicketService;
        Thread thread = new Thread(new ThreadStart(StartSyncLoop));
        thread.Start();
    }

    public SyncController(ITicketService localTicketService)
    {
        this.localTicketService = localTicketService;
        Thread thread = new Thread(new ThreadStart(StartSyncLoop));
        thread.Start();
    }

    public void StartSyncLoop()
    {
        while (true)
        {
            Sync();
            Thread.Sleep(SyncRate * 1000);
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
            AvailableEvent? le = localEvents.FirstOrDefault(t => t.Id == ae.Id);

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
            Ticket? lt = localTickets.FirstOrDefault(t => t.Id == at.Id);

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

    public async Task ChangeConnectionString(string newConnectionString)
    {
        ConnectionString = newConnectionString;
        await localTicketService.ResetDatabase();
    }
}
