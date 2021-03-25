using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
   public static class BAL_DailySummaryReport
    {
      
        public static Tuple<DataRow,object,DataTable>  DailySummaryData( int reportID, int UserID,string TimeZoneID,int ifkReportTypeId) {


            var TimeMinutes = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, TimeZoneID).Subtract(DateTime.UtcNow).TotalMinutes;

            var _GetDailySummaryData = DAL_DailySummaryReport.GetDailySummaryData(reportID, UserID, TimeZoneID, ifkReportTypeId, TimeMinutes);

            var AllData = _GetDailySummaryData.Tables[0].Copy();

            var _header = _GetDailySummaryData.Tables[1].Rows[0];

            var _logo = _GetDailySummaryData.Tables[2].Rows[0]["vLogo"];

            // change utc time to user time
           


            //remove zero distances 
            if (AllData.Rows.Count> 0 && Convert.ToInt32(_header["AllowZeroDistance"]) == 0)
            {
                var temp_data = AllData.Select("distance > 0.01");

                if (temp_data.Count() > 0 )
                    AllData =temp_data.CopyToDataTable();
            }

            var _EL_DatesFilter = new EL_DatesFilter();                      

                var _filter_O = JsonConvert.DeserializeObject<EL_DatesFilter>(Convert.ToString(_header["vCustomTime"].ToString()));

                _filter_O.bAllowFilter = Convert.ToBoolean(Convert.ToString(_header["isCustomDateEnabled"]));

                _filter_O.iTimeFilterType = Convert.ToInt32(Convert.ToString(_header["iEnabledDateType"]));

                _EL_DatesFilter = _filter_O;

            var filteredData = AllData.Clone();

            AllData.Columns.Add("vDistanceUnit", typeof(string));

            foreach (DataRow row in AllData.Rows)
            {
                //row["startTime"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["startTime"]), TimeZoneID);
                //row["endTime"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["endTime"]), TimeZoneID);



                var unitcode = Convert.ToInt32(row["unitcode"]);

                row["vDistanceUnit"] = UserSettings.GetOdometerUnitName(unitcode);

                DateTime dDeviceSentDate = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(row["startTime"]), "UTC", TimeZoneID));

                row["startTime"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["startTime"]), TimeZoneID);

                row["endTime"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["endTime"]), TimeZoneID);

                if (_EL_DatesFilter.bAllowFilter)
                {
                    var _Today = dDeviceSentDate;

                    var startDt = Convert.ToDateTime(_Today.ToString("yyyy-MM-dd ") + _EL_DatesFilter.startTime);

                    var EndDt = Convert.ToDateTime(_Today.ToString("yyyy-MM-dd ") + _EL_DatesFilter.endTime);

                    if ((_Today < startDt || _Today > EndDt) && _EL_DatesFilter.iTimeFilterType == 1)
                    {
                        continue;
                    }

                    if ((_Today >= startDt && _Today <= EndDt) && _EL_DatesFilter.iTimeFilterType == 2)
                    {
                        continue;
                    }

                   
                    filteredData.Rows.Add(row.ItemArray);
                }
                            
                
            }



            return new Tuple<DataRow,object,DataTable>(_header,_logo, _EL_DatesFilter.bAllowFilter? filteredData:AllData);




        }

    }
}
