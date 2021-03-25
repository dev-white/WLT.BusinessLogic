using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using Newtonsoft.Json;
using System.Globalization;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.BusinessLogic.Bal_Reports;
using System.Runtime.Caching;

namespace WLT.BusinessLogic.BAL
{
   public class Bal_GraphicalTriplistingReport
    {
        private int _iCompanyid;
        private bool _showZeroDistance;
        public bool ShowZeroDistance
        {
            get { return _showZeroDistance; }
            set { _showZeroDistance = value; }
        }
        public int iCompanyid
        {
            get { return _iCompanyid; }
            set { _iCompanyid = value; }
        }
        public   Bal_GraphicalTriplistingReport()
        {

        }

        public  object  GetTelemtryFromGeneratedTrips( int report_id)
        {
            ObjectCache cache = MemoryCache.Default;

            var key = $"{report_id}_graphical";

            var data = cache[key];

            cache.Remove(key);  // clear cache

            return data;

        }
        public TripOject GetTripsOfType(El_Report _El_Report)
        {

          


            Trips _clsTrips = new Trips();

            var _ds = _clsTrips.GetPreliminaryInfo(_El_Report.UserId, _El_Report.ifkReportId, 1, _El_Report.ReportTypeID);

            var List_devices = new Dictionary<long, string>();


            foreach (DataRow row in _ds.Tables[2].Rows)
            {
                try
                {
                    List_devices.Add(Convert.ToInt64(row["vpkDeviceID"]), row["vDeviceName"].ToString());

                }
                catch (ArgumentException f) { }
            }

            string devicesCSV = string.Join(",", List_devices.Select(x => x.Key).ToArray());


            if (_ds.Tables[0].Rows[0]["ReportName"].ToString() != null && _ds.Tables[0].Rows[0]["ReportName"].ToString() != string.Empty)
            {
                iCompanyid = Convert.ToInt32(_ds.Tables[0].Rows[0]["CompanyId"].ToString());
            }
            if (_ds.Tables[0].Rows[0]["StartDate"].ToString() != null && _ds.Tables[0].Rows[0]["StartDate"].ToString() != string.Empty)
            {
                _El_Report.dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_ds.Tables[0].Rows[0]["StartDate"].ToString()), _El_Report.TimeZoneID);
                //dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, dStartDate.Day,0,0,0);

            }
            else
            {
                _El_Report.dStartDate = DateTime.Now;
                _El_Report.dStartDate = new DateTime(_El_Report.dStartDate.Year, _El_Report.dStartDate.Month, _El_Report.dStartDate.Day, 0, 0, 0);
            }
            if (_ds.Tables[0].Rows[0]["EndDate"].ToString() != null && _ds.Tables[0].Rows[0]["EndDate"].ToString() != string.Empty)
            {
                _El_Report.dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_ds.Tables[0].Rows[0]["EndDate"].ToString()), _El_Report.TimeZoneID);
                //  dEndDate = new DateTime(dEndDate.Year, dEndDate.Month, dEndDate.Day, 23, 59, 59);
            }
            else
            {
                _El_Report.dEndDate = DateTime.Now;
                _El_Report.dEndDate = new DateTime(_El_Report.dEndDate.Year, _El_Report.dEndDate.Month, _El_Report.dEndDate.Day, 23, 59, 59);
            }
            if (_ds.Tables[0].Rows[0]["MeasurementUnit"].ToString() != null && _ds.Tables[0].Rows[0]["MeasurementUnit"].ToString() != string.Empty)
            {
                _El_Report.intMeasurementUnit = Convert.ToInt32(_ds.Tables[0].Rows[0]["MeasurementUnit"].ToString());
            }
            else
            {
                _El_Report.intMeasurementUnit = 0;
            }

            if (_ds.Tables[1].Rows[0]["vLogo"].ToString() != null && _ds.Tables[1].Rows[0]["vLogo"].ToString() != string.Empty)
            {
                _El_Report.CompanyLogo = Convert.ToString(_ds.Tables[1].Rows[0]["vLogo"].ToString());
            }

            if (_ds.Tables[0].Rows[0]["showZeroDistance"].ToString() != null && _ds.Tables[0].Rows[0]["showZeroDistance"].ToString() != string.Empty)
            {
                ShowZeroDistance = Convert.ToBoolean(_ds.Tables[0].Rows[0]["showZeroDistance"]);
            }
            _El_Report.AssetName = Convert.ToString(_ds.Tables[0].Rows[0]["AssetName"].ToString());


            _El_Report.ReportName = Convert.ToString(_ds.Tables[0].Rows[0]["ReportName"].ToString());



            var TempDS = _clsTrips.GetDirtyTripSummaryForMultipleAssets(devicesCSV, _El_Report.dStartDate, _El_Report.dEndDate, _El_Report.TimeZoneID);


            var FinalList = new List<clsPopulateTripSummary>();

            foreach (var item in List_devices)
            {
                DataTable table = new DataTable();
                if (TempDS.Tables.Count > 0)
                {
                    table = TempDS.Tables[0].Copy();
                    table.DefaultView.RowFilter = "vpkDeviceID  =" + item.Key;
                    table = table.DefaultView.ToTable();

                }


                DataSet ds = new DataSet();

                ds.Tables.Add(table);

                List<clsPopulateTripSummary> CleansummeryList = new List<clsPopulateTripSummary>();

                CleansummeryList = _clsTrips.CleanDirtyTripSummary(ds, iCompanyid, item.Key, _El_Report.dStartDate, _El_Report.dEndDate, _El_Report.TimeZoneID, false);

                var intermediate = new List<clsPopulateTripSummary>();

                if (ShowZeroDistance)
                {
                    intermediate = CleansummeryList;
                }
                else
                {
                    intermediate = _clsTrips.RemoveZeroDistanceFromCleanedTripSummary(0.1, CleansummeryList, _El_Report.TimeZoneID);
                }

                foreach (var _trip in intermediate)
                {
                    _trip.DurationStr = TimeSpanCalculator(_trip.Duration.TotalSeconds);

                    _trip.StartOdometer = Convert.ToDouble(UserSettings.ConvertKMsToXx(_El_Report.intMeasurementUnit, _trip.StartOdometer.ToString(), false));
                    _trip.EndOdometer = Convert.ToDouble(UserSettings.ConvertKMsToXx(_El_Report.intMeasurementUnit, _trip.EndOdometer.ToString(), false));
                    _trip.Distance = Convert.ToDouble(UserSettings.ConvertKMsToXx(_El_Report.intMeasurementUnit, _trip.Distance.ToString(), false));

                    _trip.Device_Name = item.Value;


                    var _TripsTelemetry = new Bal_TripsTelemetry();

                    _TripsTelemetry.StartDate = _trip.StartDate;

                    _TripsTelemetry.EndDate = _trip.EndDate;

                    _TripsTelemetry.TimezoneID = _El_Report.TimeZoneID;

                    _TripsTelemetry.vpkDeviceID = _trip.VpkDeviceID;

                    _trip.Telemetry = _TripsTelemetry.GetTelemetry();

                    FinalList.Add(_trip);
                }



            }
            var tripObj = new TripOject();

            tripObj.Unit = UserSettings.GetOdometerUnitName(_El_Report.intMeasurementUnit);

            tripObj.speedUnit = UserSettings.GetSpeedUnitName(_El_Report.intMeasurementUnit);

            tripObj.UnitCode = _El_Report.intMeasurementUnit;

            tripObj.Trips = FinalList.OrderBy(n => n.StartDate).ToList();


            tripObj.startDate = _El_Report.dStartDate.ToString("dddd dd MMMM yyyy HH:mm:ss", new CultureInfo(_El_Report.CultureID));

            tripObj.endDate = _El_Report.dEndDate.ToString("dddd dd MMMM yyyy HH:mm:ss", new CultureInfo(_El_Report.CultureID));



            tripObj.ReportName = _El_Report.ReportName;

            tripObj.Asset = ReportExtensions.ChangeAssetHeaderLanguageString(_El_Report.CultureID, _El_Report.AssetName, 131);

            tripObj.GeneratedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _El_Report.TimeZoneID).ToString("dddd dd MMMM yyyy HH:mm:ss", new CultureInfo(_El_Report.CultureID));

            tripObj.Logo = _El_Report.CompanyLogo;            

            return tripObj;
        }
        public string  GetTrips(El_Report _El_Report)
        {


            Trips _clsTrips = new Trips();

            var _ds = _clsTrips.GetPreliminaryInfo(_El_Report.UserId, _El_Report.ifkReportId, 1, _El_Report.ReportTypeID);

            var List_devices =   new Dictionary<long, string>();         


            foreach (DataRow row in _ds.Tables[2].Rows)
            {
                try
                {
                    List_devices.Add(Convert.ToInt64(row["vpkDeviceID"]), row["vDeviceName"].ToString());
            
                }
                catch (ArgumentException f) { }
            }

            string devicesCSV = string.Join(",", List_devices.Select(x => x.Key).ToArray());

         
            if (_ds.Tables[0].Rows[0]["ReportName"].ToString() != null && _ds.Tables[0].Rows[0]["ReportName"].ToString() != string.Empty)
            {
                iCompanyid = Convert.ToInt32(_ds.Tables[0].Rows[0]["CompanyId"].ToString());
            }
            if (_ds.Tables[0].Rows[0]["StartDate"].ToString() != null && _ds.Tables[0].Rows[0]["StartDate"].ToString() != string.Empty)
            {
                _El_Report.dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(_ds.Tables[0].Rows[0]["StartDate"].ToString()),_El_Report.TimeZoneID);
                //dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, dStartDate.Day,0,0,0);

            }
            else
            {
                _El_Report.dStartDate = DateTime.Now;
                _El_Report.dStartDate = new DateTime(_El_Report.dStartDate.Year, _El_Report.dStartDate.Month, _El_Report.dStartDate.Day, 0, 0, 0);
            }
            if (_ds.Tables[0].Rows[0]["EndDate"].ToString() != null && _ds.Tables[0].Rows[0]["EndDate"].ToString() != string.Empty)
            {
                _El_Report. dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat( Convert.ToDateTime(_ds.Tables[0].Rows[0]["EndDate"].ToString()), _El_Report.TimeZoneID);
                //  dEndDate = new DateTime(dEndDate.Year, dEndDate.Month, dEndDate.Day, 23, 59, 59);
            }
            else
            {
                _El_Report.dEndDate = DateTime.Now;
                _El_Report. dEndDate = new DateTime(_El_Report.dEndDate.Year, _El_Report.dEndDate.Month, _El_Report.dEndDate.Day, 23, 59, 59);
            }
            if (_ds.Tables[0].Rows[0]["MeasurementUnit"].ToString() != null && _ds.Tables[0].Rows[0]["MeasurementUnit"].ToString() != string.Empty)
            {
                _El_Report.intMeasurementUnit = Convert.ToInt32(_ds.Tables[0].Rows[0]["MeasurementUnit"].ToString());
            }
            else
            {
                _El_Report.intMeasurementUnit = 0;
            }

            if (_ds.Tables[1].Rows[0]["vLogo"].ToString() != null && _ds.Tables[1].Rows[0]["vLogo"].ToString() != string.Empty)
            {
                _El_Report.CompanyLogo = Convert.ToString(_ds.Tables[1].Rows[0]["vLogo"].ToString());
            }

            if (_ds.Tables[0].Rows[0]["showZeroDistance"].ToString() != null && _ds.Tables[0].Rows[0]["showZeroDistance"].ToString() != string.Empty)
            {
                ShowZeroDistance = Convert.ToBoolean(_ds.Tables[0].Rows[0]["showZeroDistance"]);
            }
            _El_Report.AssetName = Convert.ToString(_ds.Tables[0].Rows[0]["AssetName"].ToString());


            _El_Report.ReportName = Convert.ToString(_ds.Tables[0].Rows[0]["ReportName"].ToString());



            var TempDS = _clsTrips.GetDirtyTripSummaryForMultipleAssets(devicesCSV, _El_Report.dStartDate, _El_Report.dEndDate, _El_Report.TimeZoneID);


            var FinalList = new List<clsPopulateTripSummary>();

            foreach ( var item in List_devices)
            {               DataTable table = new DataTable();
                if (TempDS.Tables.Count > 0)
                {
                    table = TempDS.Tables[0].Copy();
                    table.DefaultView.RowFilter = "vpkDeviceID  =" + item.Key;
                    table = table.DefaultView.ToTable();

                }
                               

                DataSet ds = new DataSet();

                ds.Tables.Add(table);

                List<clsPopulateTripSummary> CleansummeryList = new List<clsPopulateTripSummary>();

                CleansummeryList = _clsTrips.CleanDirtyTripSummary(ds, iCompanyid, item.Key,_El_Report.dStartDate, _El_Report.dEndDate, _El_Report.TimeZoneID, false);
                
                var intermediate = new List<clsPopulateTripSummary>();

                if (ShowZeroDistance)
                {
                    intermediate = CleansummeryList;
                }
                else
                {
                    intermediate = _clsTrips.RemoveZeroDistanceFromCleanedTripSummary(0.1, CleansummeryList, _El_Report.TimeZoneID);
                }

                foreach (var _trip in intermediate)
                {
                    _trip.DurationStr =  TimeSpanCalculator(_trip.Duration.TotalSeconds);

                    _trip.StartOdometer = Convert.ToDouble(UserSettings.ConvertKMsToXx(_El_Report.intMeasurementUnit,_trip.StartOdometer.ToString(),false));
                    _trip.EndOdometer = Convert.ToDouble(UserSettings.ConvertKMsToXx(_El_Report.intMeasurementUnit, _trip.EndOdometer.ToString(), false));
                    _trip.Distance = Convert.ToDouble(UserSettings.ConvertKMsToXx(_El_Report.intMeasurementUnit, _trip.Distance.ToString(), false));

                    _trip.Device_Name = item.Value;

                  
                    var _TripsTelemetry = new Bal_TripsTelemetry();

                    _TripsTelemetry.StartDate = _trip.StartDate;

                    _TripsTelemetry.EndDate = _trip.EndDate;

                    _TripsTelemetry.TimezoneID = _El_Report.TimeZoneID;

                    _TripsTelemetry.vpkDeviceID = _trip.VpkDeviceID;                   

                    _trip.Telemetry = _TripsTelemetry.GetTelemetry();

                    FinalList.Add(_trip);
                }

                

            }
            var tripObj = new TripOject();

            tripObj.Unit = UserSettings.GetOdometerUnitName(_El_Report.intMeasurementUnit);

            tripObj.speedUnit = UserSettings.GetSpeedUnitName(_El_Report.intMeasurementUnit);

            tripObj.UnitCode = _El_Report.intMeasurementUnit;

            tripObj.Trips = FinalList.OrderBy(n=>n.StartDate).ToList();
                        

            tripObj.startDate =    _El_Report.dStartDate.ToString("dddd dd MMMM yyyy HH:mm:ss",new CultureInfo(_El_Report.CultureID));

            tripObj.endDate =     _El_Report.dEndDate.ToString("dddd dd MMMM yyyy HH:mm:ss", new CultureInfo(_El_Report.CultureID));

            

            tripObj.ReportName = _El_Report.ReportName;

            tripObj.Asset =  ReportExtensions.ChangeAssetHeaderLanguageString (_El_Report.CultureID, _El_Report.AssetName,131); 

            tripObj.GeneratedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _El_Report.TimeZoneID).ToString("dddd dd MMMM yyyy HH:mm:ss", new CultureInfo(_El_Report.CultureID));

            tripObj.Logo = _El_Report.CompanyLogo;

            var str = JsonConvert.SerializeObject(tripObj);

            return str;
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
    public class TripOject
    {
        public List<clsPopulateTripSummary> Trips { get; set; }
        public int UnitCode { get; set; }

        public string  Unit { get; set; }

        public string speedUnit { get; set; }

        public string ReportName { get; set; }

        public string Asset { get; set; }

        
        public string startDate { get; set; }

        public string endDate { get; set; }

        public string GeneratedDate { get; set; }

        public string Logo { get; set; }
        public TripOject()
        {
            Trips = new List<clsPopulateTripSummary>();
        }

    }
}
