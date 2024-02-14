using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Android.Service.QuickSettings;
using SQLite;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
using TicketsRUs.ClassLib.SQLLITE;

namespace TicketsRUs.Maui.SQLLITE
{
    public class LocalDatabase
    {
        SQLiteAsyncConnection _connection;
        ITicketService _ticketservice;

        public LocalDatabase(ITicketService fs)
        {
            _ticketservice = fs;
        }

        async Task Init()
        {
            if (_connection is not null)
                return;
            _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            await _connection.CreateTableAsync<Ticket>();
            await _connection.CreateTableAsync<AvailableEvent>();
        }
        public async Task<List<Ticket>> GetALlTicketsAsync()
        {
            await Init();
            return await _connection.Table<Ticket>().ToListAsync();
        }
        public async Task<int> SaveFileAsync(Ticket t)
        {
            await Init();
            if(t.Id !=0 )
                return await _connection.UpdateAsync(t);
            else
                return await _connection.InsertAsync(t);
    }}
}
