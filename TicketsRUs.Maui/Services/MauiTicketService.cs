﻿using Microsoft.Extensions.Logging;
using SQLite;
using TicketsRUs.ClassLib;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;

namespace TicketsRUs.Maui.Services;

public class MauiTicketService : ITicketService
{
    SQLiteAsyncConnection db;

    async Task Init()
    {
        if (db is not null) { return; }

        db = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

        await db.CreateTableAsync<Ticket>();
        await db.CreateTableAsync<AvailableEvent>();
    }

    public async Task<AvailableEvent> CreateAvailableEvent(AvailableEvent ai)
    {
        await Init();

        int count = await db.Table<AvailableEvent>().CountAsync();

        await db.InsertAsync(ai);
        return ai;
    }

    public async Task<Ticket> CreateTicket(int event_id)
    {
        await Init();

        int count = await db.Table<Ticket>().CountAsync();
        Ticket t = new Ticket()
        {
            Id = count + 1,
            EventId = event_id,
            Scanned = false,
            Identifier = $"{Math.Abs(DateTime.Now.ToString().GetHashCode())}{Math.Abs(DateTime.Now.Millisecond.ToString().GetHashCode())}{DateTime.Now.Ticks}"
        };

        await db.InsertAsync(t);
        return t;
    }

    public async Task<Ticket> CreateTicket(Ticket t)
    {
        await Init();

        int count = await db.Table<Ticket>().CountAsync();

        await db.InsertAsync(t);
        return t;
    }

    public async Task<IEnumerable<AvailableEvent>> GetAllAvailableEvents()
    {
        await Init();
        return await db.Table<AvailableEvent>().ToListAsync();
    }

    public async Task<IEnumerable<Ticket>> GetAllTickets()
    {
        await Init();
        return await db.Table<Ticket>().ToListAsync();
    }

    public async Task<AvailableEvent> GetAvailableEvent(int id)
    {
        await Init();
        return (await db.Table<AvailableEvent>().ToListAsync())
                .Where(t => t.Id == id)
                .First();
    }

    public async Task<Ticket> GetTicket(int id)
    {
        await Init();
        return (await db.Table<Ticket>().ToListAsync())
                .Where(t => t.Id == id)
                .First();
    }

    public async Task UpdateAvailableEvent(AvailableEvent ai)
    {
        await Init();
        await db.UpdateAsync(ai);
    }

    public async Task UpdateTicket(Ticket t)
    {
        await Init();
        await db.UpdateAsync(t);
    }
}