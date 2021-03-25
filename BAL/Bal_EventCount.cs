using System;
using System.Linq;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic.BAL
{
   public class Bal_EventCount
    {
        public long DeviceId { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public int harshaccel { get; set; }
        public int harsbraking { get; set; }
        public int harscornering { get; set; }
        public int excessiveidle { get; set; }
        public int overspeed { get; set; }

        public System.Data.DataSet RawDataset { get; set; }

        public Bal_EventCount()
        {
             

        }
        public Bal_EventCount(int ReportID,DateTime _start, DateTime _end, string vpkDeviceIdCSV,string TimeZoneId )
        {
            var utcStart = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_start,TimeZoneId);
            var utcEnd = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_end, TimeZoneId);

            RawDataset = DAL_Reports.GetEventsCount(ReportID, utcStart, utcEnd, vpkDeviceIdCSV);

        }
        public Bal_EventCount GetEventsCount(DateTime _start, DateTime _end,long vpkdeviceId)
        {
         

            var _ds = RawDataset;

               if (_ds.Tables.Count>0  && _ds.Tables[0].Rows.Count>0)
                {
                harsbraking =     _ds.Tables[0].Select("vpkDeviceID ='"+  vpkdeviceId + "'  and event ='harsh_braking' and dGPSDateTime  >= #" + _start.ToString("yyyy-MM-dd HH:mm:ss") + "#  and dGPSDateTime <= #"+_end.ToString("yyyy-MM-dd HH:mm:ss") + "# ").Count();
                harshaccel =      _ds.Tables[0].Select("vpkDeviceID ='" + vpkdeviceId + "' and event ='harsh_acceleration' and dGPSDateTime  >= #" + _start.ToString("yyyy-MM-dd HH:mm:ss") + "#  and dGPSDateTime <= #" + _end.ToString("yyyy-MM-dd HH:mm:ss") + "#").Count();
                harscornering =   _ds.Tables[0].Select("vpkDeviceID ='" + vpkdeviceId + "' and event ='harsh_cornering' and dGPSDateTime  >= #" + _start.ToString("yyyy-MM-dd HH:mm:ss") + "#  and dGPSDateTime <= #" + _end.ToString("yyyy-MM-dd HH:mm:ss") + "#").Count();
                overspeed =       _ds.Tables[0].Select("vpkDeviceID ='" + vpkdeviceId + "' and event ='Overspeed' and dGPSDateTime  >= #" + _start.ToString("yyyy-MM-dd HH:mm:ss") + "#  and dGPSDateTime <= #" + _end.ToString("yyyy-MM-dd HH:mm:ss") + "#").Count();
                excessiveidle =   _ds.Tables[0].Select("vpkDeviceID ='" + vpkdeviceId + "' and event ='excessive_idle' and dGPSDateTime  >= #" + _start.ToString("yyyy-MM-dd HH:mm:ss") + "#  and dGPSDateTime <= #" + _end.ToString("yyyy-MM-dd HH:mm:ss") + "#").Count();

            }
                           
           
            return this;
        }

    }
}
