using Microsoft.AspNetCore.Mvc;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;

namespace TicketsRUs.ClassLib.Controllers;

[Route("[controller]")]
[ApiController]
public class ApiTicketController : ControllerBase
{
    ITicketService _service;
    public ApiTicketController(ITicketService service)
    {
            _service = service;
    }

    [HttpGet("events")]
    public async Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents()
    {
        return await _service.GetAllAvailableEvents();
    }

    [HttpGet("clients")]
    public async Task<IEnumerable<Client>> GetAllClients()
    {
        return await _service.GetAllClients();
    }

    [HttpGet("tickets")]
    public async Task<IEnumerable<Ticket>> GetAllTickets()
    {
        return await _service.GetAllTickets();
    }

    [HttpGet("event/{id}")]
    public async Task<AvailableEvent> GetAvailableEvent(int id)
    {
        return await _service.GetAvailableEvent(id);
    }

    [HttpGet("client/{id}")]
    public async Task<Client> GetClient(string email)
    {
        return await _service.GetClient(email);
    }

    [HttpGet("ticket/{id}")]
    public async Task<Ticket> GetTicket(int id)
    {
        return await _service.GetTicket(id);
    }

    [HttpPost("purchase")]
    public async void CreateTicket(string email, int event_id) 
    {
        await _service.CreateTicket(email, event_id);
    }

    [HttpPost("client")]
    public async void CreateClient(string email)
    {
        await _service.CreateClient(email);
    }

    [HttpPost("ticket")]
    public async void UpdateTicket(Ticket t)
    {
        await _service.UpdateTicket(t);
    }
}
