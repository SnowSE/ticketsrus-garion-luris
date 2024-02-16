using SQLite;
using TicketsRUs.ClassLib.Data;
using TicketsRUs.ClassLib.Services;
using TicketsRUs.ClassLib.SQLLITE;
namespace TicketsRUs.Maui.SQLLITE;

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
        if (localDB is not null) { return; }
        localDB = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await localDB.CreateTableAsync<Ticket>();
        await localDB.CreateTableAsync<AvailableEvent>();
    }

    //public async Task<List<Ticket>> GetALlTicketsAsync()
    //{
    //    await Init();
    //    return await localDB.Table<Ticket>().ToListAsync();
    //}

    //public async Task<int> SaveFileAsync(Ticket t)
    //{
    //    await Init();
    //    if (t.Id != 0)
    //        return await localDB.UpdateAsync(t);
    //    else
    //        return await localDB.InsertAsync(t);
    //}

    public async Task UpdateMainDatabaseFromLocal()
    {
        await Init();

        List<Ticket> apiTickets = (await _ticketservice.GetAllTickets()).ToList();
        var localTickets = await localDB.Table<Ticket>().ToListAsync();
        
        foreach (Ticket lt in localTickets)
        {
            Ticket at = apiTickets.Where(t => t.Id == lt.Id).Single();

            bool isScanned = (lt.Scanned ?? false) || (at.Scanned ?? false);
            bool isDuplicateScan = (lt.Scanned ?? false) && (at.Scanned ?? false);

            if (isScanned)
            {
                at.Scanned = true;
                lt.Scanned = true;

                await _ticketservice.UpdateTicket(at);
            }

            if (isDuplicateScan)
            {
                // TODO: Do some logic here to notify that there was a duplicate scan
            }
        }
    }
    public async Task UpdateLocalDatabase()
    {
        IEnumerable<Ticket> mainTickets = await _ticketservice.GetAllTickets();
        var localTickets = await localDB.Table<Ticket>().ToListAsync();
        foreach (var ticket in mainTickets)
        {
            var dontExitsLocally = localTickets.Where(t => t.Id == ticket.Id).ToList();
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

    public async Task Sync()
    {
        await Init();

        List<Ticket> apiTickets = (await _ticketservice.GetAllTickets()).ToList();
        var localTickets = await localDB.Table<Ticket>().ToListAsync();

        foreach (Ticket at in apiTickets)
        {
            Ticket? lt = localTickets.Where(t => t.Id == at.Id).Single();

            if (lt == null)
            {

            }

            bool isScanned = (lt.Scanned ?? false) || (at.Scanned ?? false);
            bool isDuplicateScan = (lt.Scanned ?? false) && (at.Scanned ?? false);

            if (isScanned)
            {
                at.Scanned = true;
                lt.Scanned = true;

                await _ticketservice.UpdateTicket(at);
            }

            if (isDuplicateScan)
            {
                // TODO: Do some logic here to notify that there was a duplicate scan
            }
        }
    }
}