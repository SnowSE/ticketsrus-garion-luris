using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsRUs.Maui.Controllers;

public class SyncController
{
    public string ConnectionString { get; set; } = "";
    public int SyncRate { get; set; } = 30; // seconds
    public SyncController(string conn)
    {
        ConnectionString = conn;
        SyncLoop();
    }

    public void SyncLoop()
    {
        //while (true)
        //{

        //}
    }
}
