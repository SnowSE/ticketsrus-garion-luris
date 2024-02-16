using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
using TicketsRUs.ClassLib.SQLLITE;
namespace TicketsRUs.Maui.SQLLITE
{
    public class LocalDatabase
    {
        private readonly PeriodicTimer _periodicTimer;
        private readonly CancellationTokenSource _cancellationToken = new();
        private Task? _task;

        SQLiteAsyncConnection localDB;
        ITicketService _ticketservice;

        public LocalDatabase(ITicketService fs, TimeSpan timeSpam)
        {
            _periodicTimer = new PeriodicTimer(timeSpam);
            _ticketservice = fs;
        }

        async Task Init()
        {
            if (localDB is not null)
                return;
            localDB = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await localDB.CreateTableAsync<Ticket>();
            await localDB.CreateTableAsync<AvailableEvent>();
        }
        public async Task<List<Ticket>> GetALlTicketsAsync()
        {
            await Init();
            return await localDB.Table<Ticket>().ToListAsync();
        }
        public async Task<int> SaveFileAsync(Ticket t)
        {
            await Init();
            if (t.Id != 0)
                return await localDB.UpdateAsync(t);
            else
                return await localDB.InsertAsync(t);
        }
        public async Task UpdateMainDatabaseFromLocal()
        {
            IEnumerable<Ticket> mainTickets = await _ticketservice.GetAllTickets();
            if (localDB is null)
                return;
            var localTickets = await localDB.Table<Ticket>().ToListAsync();
            foreach (var localTicket in localTickets)
            {
                if (localTicket.Scanned == true)
                {
                    var mainTicket = mainTickets.Where(mt => mt.Id == localTicket.Id).Single();
                    if (mainTicket.Scanned == false)
                    {
                        await _ticketservice.UpdateTicket(localTicket);
                    }
                }
            }
        }
        public async Task UpdateLocalDatabase()
        {
            IEnumerable<Ticket> mainTickets = await _ticketservice.GetAllTickets();
            var localTickets = await localDB.Table<Ticket>().ToListAsync();
            foreach (var ticket in mainTickets)
            {
                var dontExitsLocally = localTickets.Where(tid => tid.Id == ticket.Id).ToList();
                if (dontExitsLocally.Count() < 0)
                {
                    foreach(var ticketlocallt in dontExitsLocally)
                    {
                        await _ticketservice.AddTicket(ticketlocallt);
                    }
                }

            }
        }
        public void TurnOnAsync()
        {
            _task = UpdateMainDatabaseFromLocal();
        }
        public async Task TurnOffAsync()
        {
            if (_task is null)
            {
                return;
            }
            _cancellationToken.Cancel();
            await _task;
            _cancellationToken.Dispose();
        }
    }
}