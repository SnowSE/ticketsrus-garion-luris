using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsRUs.ClassLib.SQLLITE
{
    public static class Constants
    {
        public const string DataBaseFileName = "L0cal$QLite.db3";
        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine("\\Users\\benson.bird\\AppData\\Local\\Packages\\com.companyname.mauiemailsender_9zz4h110yvjzm\\LocalState\\", DataBaseFileName); //FileSystem.AppDataDirectory
                                                                                                                                                           //"\\Users\benson.bird\\AppData\\Local\\Packages\\com.companyname.mauiemailsender_9zz4h110yvjzm\\LocalState\\"
    }
}
