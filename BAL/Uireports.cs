using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLT.BusinessLogic.BAL
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
        public string  Email { get; set; }
        public int LoginCount { get; set; }
        public int DeviceCount { get; set; }
        public int ReportingDeviceCount { get; set; }
        public int TrialRemainingTime { get; set; }
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
    public class DevicesjObj {

        public string DeviceName{get;set;}
        public Int64 ipkDeviceID { get; set; }
        public int TrackerType { get; set; }
    
    
    }
    public class Dashboardwidget    {
        public string DashboardID { get; set; }
        public int widgetId { get; set; }
    }
   #endregion 
}
