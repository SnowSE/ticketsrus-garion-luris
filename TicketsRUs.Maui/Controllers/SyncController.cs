using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TicketsRUs.ClassLib.Data;

namespace TicketsRUs.Maui.Controllers;

public class SyncController
{
    public string ConnectionString { get; set; } = "";
    public int SyncRate { get; set; } = 30; // seconds
    private MauiTicketController localTicketController;
    public SyncController(string conn, MauiTicketController mc)
    {
        ConnectionString = conn;
        localTicketController = mc;
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

        var localEvents = (await localTicketController.GetAllAvailableEvents()).ToList();
        var localTickets = (await localTicketController.GetAllTickets()).ToList();
        List<AvailableEvent>? apiEvents = await client.GetFromJsonAsync<List<AvailableEvent>>($"{ConnectionString}/ApiTicket/events/");
        List<Ticket>? apiTickets = await client.GetFromJsonAsync<List<Ticket>>($"{ConnectionString}/ApiTicket/tickets/");

        if (apiEvents == null || apiTickets == null) { return; }

        await localTicketController.Sync(apiEvents, localEvents, apiTickets, localTickets);
    }
}
