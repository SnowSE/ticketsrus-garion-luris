using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsRUs.ClassLib.SQLLITE
{
    public class Connection
    {
        public bool HasConnection()
        {
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                return true;
            else
                return false;
        }
    }
}
