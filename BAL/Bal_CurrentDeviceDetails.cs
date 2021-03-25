using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic.BAL
{
   public  class Bal_CurrentDeviceDetails
    {


        public   DataSet GetRawDeviceDetails(int Operation, long reportId, int userId, int reportTypeId , string TimeZoneID)
        {

            var _returnDs = new DataSet();

            var _ds =  DAL_CurrentDeviceDetails.GetReportDeviceDetails( Operation,  reportId,  userId,  reportTypeId );
            var _dataDatable = new DataTable();
            _dataDatable.Columns.Add("AssetName",typeof(string));
            _dataDatable.Columns.Add("Odometer", typeof(double));
            _dataDatable.Columns.Add("date", typeof(string));
            _dataDatable.Columns.Add("EngineHours", typeof(Int32));

            int RowCount = 0;

            foreach(DataTable dt in _ds.Tables)
            {
                if (RowCount == 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var currentDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( DateTime.UtcNow,TimeZoneID);

                        var EngineHours = new TimeSpan();
                        var Odometer = 0.0;

                        if (row["EngineHours"] != DBNull.Value)                        
                            EngineHours = TimeSpan.FromTicks(Convert.ToInt64(row["EngineHours"]));

                        if (row["vOdometer"] != DBNull.Value)
                            Odometer = Convert.ToDouble(row["vOdometer"]);


                        _dataDatable.Rows.Add(row["AssetName"], Odometer , currentDate.ToString("dd/MM/yyyy"),EngineHours.TotalHours);
                    }
                }


                RowCount++;
            }
            // add the body section 
            _returnDs.Tables.Add(_dataDatable);

            return _returnDs;
        }


        public static string TimeSpanCalculator(object _seconds)
        {

            string returnResult = string.Empty;

            var smTimespan = new TimeSpan();

            if (_seconds != DBNull.Value)
            {
                smTimespan = TimeSpan.FromSeconds(Convert.ToDouble(_seconds));
            }
            else
            {
                returnResult = TimeSpan.FromSeconds(0).ToString();

                smTimespan = TimeSpan.FromSeconds(0);
            }


            if (smTimespan.Days > 0)
            {

                returnResult = smTimespan.Days + " days " + smTimespan.Hours + " hrs " + smTimespan.Minutes + " mins " + smTimespan.Seconds + " secs";
            }
            else if (smTimespan.Days < 1 && smTimespan.Hours > 0)
            {
                returnResult = smTimespan.Hours + " hrs " + smTimespan.Minutes + " mins " + smTimespan.Seconds + " secs";
            }
            else if (smTimespan.Hours < 1 && smTimespan.Minutes > 0)
            {
                returnResult = smTimespan.Minutes + " mins " + smTimespan.Seconds + " secs";
            }
            else if (smTimespan.Minutes < 1 && smTimespan.Seconds > 0)
            {
                returnResult = smTimespan.Seconds + " secs";
            }
            else if (smTimespan.Seconds == 0)
            {
                returnResult = "0 secs";
            }

            return returnResult;






        }

    }
}
