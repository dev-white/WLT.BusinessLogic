using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace WLT.BusinessLogic.BAL
{
   public class Bal_ExternalTrips
    {
        ObjectCache cache = MemoryCache.Default;

        public void GetTripsForDay( long  imei ,DateTime date )
        {
          var dsResultset = new DataSet();

           


            // Store data in the cache    
           

        }

        public void GetTripsForDay(long imei)
        {
            var dsResultset = new DataSet();



        }
    }
}
