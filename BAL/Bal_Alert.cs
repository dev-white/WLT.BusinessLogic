using System;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using Newtonsoft.Json;
using System.Data;

namespace WLT.BusinessLogic.BAL
{
  public class Bal_Alert
    {

        public Bal_Alert() { }


        public string GetAlerts(EL_Alert _EL_Alert)
        {
            var returnStr = string.Empty;

            var _DAL_Alerts = new DAL_Alerts();

            var _ds = _DAL_Alerts.GetAlerts(_EL_Alert);
                 
             if (_ds.Tables.Count > 0)
            {
                returnStr = JsonConvert.SerializeObject(_ds.Tables[0]);
            }

            return returnStr;

        }


        public string GetNotificationAlerts(EL_Alert _EL_Alert)
        {
            var returnStr = string.Empty;

            var _DAL_Alerts = new DAL_Alerts();

            CreateRangeFromDate(_EL_Alert);

            var _ds = _DAL_Alerts.GetAlerts(_EL_Alert);

            if (_ds.Tables.Count > 0)
            {

                var _dt = _ds.Tables[0];

                _dt.Columns.Add("UserTimeDate", typeof(DateTime));

                foreach (DataRow _row in _ds.Tables[0].Rows)
                {
                    _row["UserTimeDate"] = UserSettings.ConvertUTCDateTimeToLocalDateTime(Convert.ToDateTime( _row["GpsDateTime"]), _EL_Alert.TimeZoneID);
                }
                returnStr = JsonConvert.SerializeObject(_dt);
            }

            return returnStr;

        }
        public string GetMoreAlertDetails(EL_Alert _EL_Alert)
        {
            var returnStr = string.Empty;

            var _DAL_Alerts = new DAL_Alerts();

            var _ds = _DAL_Alerts.GetAlerts(_EL_Alert);

            returnStr = JsonConvert.SerializeObject(_ds);

            return returnStr;

        }

        public void CreateRangeFromDate(EL_Alert _EL_Alert)
        {
            var _dt = _EL_Alert.Date;

            var _startuserTime = new DateTime(_dt.Year, _dt.Month, _dt.Day, 00, 00, 00);

            var _enduserTime = new DateTime(_dt.Year, _dt.Month, _dt.Day,23,59,59);

            var _utcstartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_startuserTime, _EL_Alert.TimeZoneID);

            var _utcendTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_enduserTime, _EL_Alert.TimeZoneID);

            _EL_Alert.StartDate = _utcstartTime;

            _EL_Alert.EndDate = _utcendTime;

        }
     

    }


}
