using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsRUs.Maui.Init;

namespace TicketsRUs.Maui.Services
{
    public class StartupLocalDatabase
    {
        LocalDataBaseInit _service; 
        public bool isOline { get => Preferences.Default.Get("isOnline", false); }
        public string ApiAddress { get => Preferences.Default.Get("address", "https://localhost:7283"); }
        public int synRate { get => Preferences.Default.Get("syncRate", 60); }
        public StartupLocalDatabase(LocalDataBaseInit service)
        {
            _service = service;
        }
        public void startLocal(bool isOline, int time, string apiAddress)
        {
            Preferences.Default.Set("isOnline", isOline);
            Preferences.Default.Set("syncRate", time);
            Preferences.Default.Set("address", apiAddress);
        }
        public async void EmptyTables()
        {
            await _service.EmptyDatabase();
        }
    }
}
