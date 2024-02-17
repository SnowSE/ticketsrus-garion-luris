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
    public int SyncRate { get; set; } = 5; // seconds
    private ITicketService localTicketService;
    public SyncController(string conn, ITicketService localTicketService)
    {
        ConnectionString = conn;
        this.localTicketService = localTicketService;
    }

    public async Task Start()
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(SyncRate));
        while( await timer.WaitForNextTickAsync() )
        {
            Sync();
        }
    }

    public async void Sync()
    {
        try
        {
            HttpClient client = new HttpClient();
            List<AvailableEvent>? apiEvents = await client.GetFromJsonAsync<List<AvailableEvent>>($"{ConnectionString}/ApiTicket/events/");
            List<Ticket>? apiTickets = await client.GetFromJsonAsync<List<Ticket>>($"{ConnectionString}/ApiTicket/tickets/");

            if (apiEvents == null || apiTickets == null)
            {
                return;
            }

            await SyncEvents(apiEvents);
            await SyncTickets(apiTickets);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error in the sync method {ex.Message}");
        }
    }

    private async Task SyncEvents(List<AvailableEvent> apiEvents)
    {
        var localEvents = await localTicketService.GetAllAvailableEvents();

        foreach (var ae in apiEvents)
        {
            var le = localEvents.Where(t => t.Id == ae.Id).Single();

            if (le == null)
            {
                await localTicketService.CreateAvailableEvent(ae);
            }
        }
    }

    private async Task SyncTickets(List<Ticket> apiTickets)
    {
        var localTickets = await localTicketService.GetAllTickets();
        foreach (Ticket at in apiTickets)
        {
            var lt = localTickets.Where(t => t.Id == at.Id).Single();

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
        }
    }
}
