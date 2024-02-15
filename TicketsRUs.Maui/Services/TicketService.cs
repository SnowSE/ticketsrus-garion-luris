using System.Net.Http.Json;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;

namespace TicketsRUs.Maui.Services
{
    public class TicketService
    {
        HttpClient _httpClient;
        ITicketService _ticketService;
        string AddressApi;

        public TicketService(ITicketService ticketService)
        {
            this._ticketService = ticketService;
        }
        public async Task Sync()
        {
            var t = Preferences.Default.Get("update", 20);
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(t));
            if (Preferences.Default.Get("Online", false) && await timer.WaitForNextTickAsync())
            {
                await SyncTickedScanned();
            }
        }
        public async Task SyncTickedScanned()
        {
            var webs = await GetAllTickets();

            foreach (var web in webs)
            {
                // Check if there's a change in scanned status
                if (web.ApiApp != null && web.MauiaApp == null)
                {
                    await _ticketService.AddTicket(web.ApiApp);
                }
                else if (web.ApiApp != null && web.MauiaApp != null && web.ApiApp.Event.StartTime < web.MauiaApp.Event.StartTime)
                {
                    // Update the ticket in Maui
                    var response = await _httpClient.PatchAsJsonAsync($"{AddressApi}/ticket/updateTicket", web.MauiaApp);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception("Ticket updating was unsuccessful");
                    }
                }
                else if (web.ApiApp != null && web.MauiaApp != null && web.ApiApp.Event.StartTime > web.MauiaApp.Event.StartTime)
                {
                    // Update the ticket in API
                    await _ticketService.UpdateTicket(web.ApiApp);
                }
                else
                {
                    throw new Exception("my sync method has an error");
                }
            }
        }
        public async Task<IEnumerable<Web<Ticket>>> GetAllTickets()
        {
            IEnumerable<Ticket> onlineTickets = await _httpClient.GetFromJsonAsync<IEnumerable<Ticket>>($"{AddressApi}/ticket/getAll");
            IEnumerable<Ticket> localTickets = await _ticketService.GetAllTickets();

            HashSet<(int, DateTime?)> localTicketInfo = new HashSet<(int, DateTime?)>(localTickets.Select(x => (x.Id, x.Event.StartTime)));

            //Tickets that are not in the local
            var onlineChanges = onlineTickets
                .Where(x => !localTicketInfo.Contains((x.Id, x.Event.StartTime)))
                .Select(x => new Web<Ticket>(localTickets.FirstOrDefault(y => y.Id == x.Id), x))
                .ToList();

            return onlineChanges;
        }
    }
}
