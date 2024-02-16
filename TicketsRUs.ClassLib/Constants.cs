using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsRUs.ClassLib;

public static class Constants
{
    public const string DataBaseFileName = "Local.db3";
    public const SQLite.SQLiteOpenFlags Flags =
    // open the database in read/write mode
        SQLite.SQLiteOpenFlags.ReadWrite |
    // create the database if it doesn't exist
        SQLite.SQLiteOpenFlags.Create |
    // enable multi-threaded database access
        SQLite.SQLiteOpenFlags.SharedCache;

    public static string DatabasePath => Path.Combine(Directory.GetCurrentDirectory(), DataBaseFileName);
}
