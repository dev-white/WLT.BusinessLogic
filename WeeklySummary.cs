using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using WLT.EntityLayer;
using WLT.DataAccessLayer;
using WLT.EntityLayer.Utilities;

namespace WLT.BusinessLogic
{
    public class WeeklySummary
    {

       static string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public double  DailyDistance{get;set;}
        public DateTime DailyTotalTime { get; set; }
        public long vpkDeviceID { get; set; }

        public  static   DataSet GetpreferredDistanceAsDataset(string  csvMultipleDeviceIDs, DateTime startDate, DateTime endDate, string TimeZoneID, int op) { 
        
        //return Mon: Start 1212, End 2322 | Tue: Start 100, End 200...
        // work out total distance for each day 
        // Work out total distance for each week.

            DataSet _ds = new DataSet();
        
            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@Operation",SqlDbType.Int);
            param[0].Value = op;
            param[1] = new SqlParameter("@StartDate", SqlDbType.NChar);
            param[1].Value = UserSettings.ConvertLocalDateTimeToUTCDateTime(startDate, TimeZoneID);

            param[2] = new SqlParameter("@EndDate", SqlDbType.NChar);    
            param[2].Value = UserSettings.ConvertLocalDateTimeToUTCDateTime( endDate,TimeZoneID);

            param[3] = new SqlParameter("@imeiNumbersCsv", SqlDbType.NVarChar);
            param[3].Value =   csvMultipleDeviceIDs;


            var thisdate = Convert.ToDateTime(UserSettings.ConvertLocalDateTimeToUTCDateTime(endDate, TimeZoneID));

            switch (thisdate.ToString("ddd"))
                    {
                        case "Sun":

                            string x = "";

                            break;
                        case "Mon":


                            string x1 = "";

                            break;
                        case "Tue":

                            string x2 = "";
                            break;
                        case "Wed":

                            string x3 = "";

                            break;
                        case "Thu":

                            string x4 = "";


                            break;
                        case "Fri":

                            string x5 = "";


                            break;
                        case "Sat":

                            string x6 = "";
                            break;

                    }

            _ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "GetDistance", param);



            return _ds;

        }


        public static   List<clsPopulateTripSummary> GetPreferredDistanceAsList(string csvMultipleDeviceIDs, DateTime startDate, DateTime endDate, string TimeZoneID,int op)
        {

            DataSet _ds = GetpreferredDistanceAsDataset(csvMultipleDeviceIDs, startDate, endDate, TimeZoneID,op);
            var list = new List<clsPopulateTripSummary>();

            if (_ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow row in _ds.Tables[0].Rows)
                {

                    list.Add(new clsPopulateTripSummary { VpkDeviceID = Convert.ToInt64(row["vpkDeviceID"]), StartOdometer = Convert.ToDouble(row["minvOdometer"]), EndOdometer = Convert.ToDouble(row["maxvOdometer"]) });

                }

            }

          

            return list;
        }


        public static  List<WeeklySummary> CalculateDailyTotalDistance(string csvMultipleDeviceIDs, DateTime startDate, DateTime endDate, string TimeZoneID,int op)
        {
           List<WeeklySummary> _dailyData = new List<WeeklySummary>();
         var   start = startDate;
         var   end = endDate;


           while (start < end)
           {
               System.TimeSpan diff = end.Subtract(start);

               DateTime EndLocaldate = new DateTime();

               if (diff.TotalDays < 1)
               {         
                   EndLocaldate = end;                                

               }

               else
               {

                   EndLocaldate = new DateTime(start.Year, start.Month, start.Day, 23, 59, 59);

               }


               if (start.ToString("ddd") == "Sat") {
                   string f = "";
               
               }
               var d = GetPreferredDistanceAsList(csvMultipleDeviceIDs, start, EndLocaldate, TimeZoneID, op);

               var _deviceList =   d.Select(x => x.VpkDeviceID).Distinct();


              foreach (var VpkDeviceID in _deviceList)
               {
                  
                   var SpecificDeficeData = d.Where(n => n.VpkDeviceID == VpkDeviceID).ToList();

                   double firstOdometer = SpecificDeficeData.Count == 0 ? 0 : SpecificDeficeData.AsQueryable().FirstOrDefault().StartOdometer;

                   double lastOdometer = SpecificDeficeData.Count == 0 ? 0 : SpecificDeficeData.AsQueryable().FirstOrDefault().EndOdometer;



                   _dailyData.Add(new WeeklySummary { vpkDeviceID = VpkDeviceID, DailyDistance = (lastOdometer -  firstOdometer), DailyTotalTime = start });

               }

               start = start.AddDays(1);
               start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);
           }
         
            return _dailyData;
        }
        
        

    }
   
}
