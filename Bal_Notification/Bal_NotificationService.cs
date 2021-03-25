using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using WLT.BusinessLogic.Admin_Classes;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.DataAccessLayer.GPSOL;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_Notification
{
    public class data
    {
        public string y { get; set; }
        public int a { get; set; }
        public int b { get; set; }
    }

    public class TrialTabledata
    {
        public string ResellerName { get; set; }
        public bool IsTrial { get; set; }
        public string Email { get; set; }


        public int LoginCount { get; set; }
        public int DeviceCount { get; set; }
        public int ReportingDeviceCount { get; set; }


        public int TrialRemainingTime { get; set; }
        public string ResellerID { get; set; }
        public string Url { get; set; }


        public string UserName { get; set; }
        public string UserMail { get; set; }
    }
    #region Distance Travelled Model
    public class DistanceTravelledGraphModel
    {
        public int UserId { set; get; }
        public int ClientId { set; get; }
        public string timeZoneId { set; get; }
        public DateTime StartDate { set; get; }
        public int Month { set; get; }
        public DistanceTravelledGraphModel() { }


    }
    public class DistanceTravelledGraphOutputModel
    {
        public double TotalDistance { set; get; }
        public DateTime Date { set; get; }

        public double VehicleAvg { set; get; }
        public double OverrallTotalDistance { set; get; }
        public double MonthlyAvg { set; get; }
        public string Unit { set; get; }

        public DistanceTravelledGraphOutputModel() { }


    }
    public class DevicesjObj
    {

        public string DeviceName { get; set; }
        public int TrackerType { get; set; }
        public Int64 vpkDeviceId { get; set; }
        public int ifkDeviceID { get; set; }
    }
    #endregion
    public class Dashboardwidget
    {
        public string DashboardID { get; set; }
        public int widgetId { get; set; }
    }


    public class Alert
    {
        public string icon { get; set; }
        public string AlertName { get; set; }
        public int AlertPriority { get; set; }
        public int AlertCode { get; set; }



    }
    public class ZoneClass
    {
        public string icon { get; set; }
        public string ZoneName { get; set; }
        public int ZoneType { get; set; }
        public string vGeofenceType { get; set; }
        public int zoneCode { get; set; }
        public string Asset { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public TimeSpan Duration { get; set; }
        public Int64 vpkDeviceID { get; set; }
        public string vEventName { get; set; }
        public int vEventID { get; set; }
        public bool IsComplete { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }
    }


    public class EL_Dashboard_aob
    {

        public string ViolationType { get; set; }
        public int count { get; set; }
        public int code { get; set; }

        public int reportId { get; set; }




    }

    public class Widget_Trip
    {

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public string specificDate { get; set; }
        public object DeviceID { get; set; }

        public int No_of_Trips { get; set; }
        public int No_of_Assets { get; set; }

        public List<Widget_Trip> Data_List { get; set; }


        public Widget_Trip()
        {

            this.Data_List = new List<Widget_Trip>();
        }



    }

    public class Widget_Maintenance
    {
        public string Asset { get; set; }
        public string MaintenanceItem { get; set; }

        public int RemainingTime { get; set; }
        public double Set_ParameterValue { get; set; }

        public DateTime Set_ParameterValueDate { get; set; }
        public double Current_Odo { get; set; }
        public double Remaining_Distance { get; set; }
        public string vlogo { get; set; }
    }

    public static class Bal_Helpers
    {
       
     
        
        public static Tuple<DateTime, DateTime> WeekIO(int iWeek, string TimeZoneID)
        {


            var userTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, TimeZoneID);

            userTime = new DateTime(userTime.Year, userTime.Month, userTime.Day);

            var startDate = new DateTime();

            var endDate = new DateTime();

            var daysInWk = 7 * iWeek;


            startDate = userTime.AddHours(-(((daysInWk * 24) / 2) - 1));

            endDate = userTime.AddHours(((daysInWk * 24) / 2));

            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);


            var item = new Tuple<DateTime, DateTime>(
                UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(startDate, TimeZoneID),
                 UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(endDate, TimeZoneID)
                );


            return item;
        }


    }

  
    public class Route
    {
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? RoadSpeed { get; set; }
        public double Speed { get; set; }
        public long vpkDeviceID { get; set; }
    }

}
