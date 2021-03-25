using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.Reports.HtmlReports;

namespace WLT.BusinessLogic.BAL
{
    [System.ComponentModel.DataObject]
    public class Bal_Reports_Helper
    {

        public List<string> Features { get; set; }

        Trips _clsTrips = new Trips();
        public Bal_Reports_Helper()
        {
            Features = new List<string>();
        }



        public DataTable Source(int UserID, int ReportId, string TimeZoneID, int ReportTypeId)
        {

            El_Custom_Report _El_Dynamic_Report = new El_Custom_Report();

            var _DAL_Reports = new DAL_Reports();

            var _ds = _DAL_Reports.GetParentID(ReportTypeId, 5, UserID);

            foreach (DataRow dr in _ds.Tables[1].Rows)
            {
                Features.Add(Convert.ToString(dr["name"]).Trim());
            }

            _El_Dynamic_Report.TemplateType = Convert.ToInt32(_ds.Tables[0].Rows[0]["iTemplate"]);
            _El_Dynamic_Report.UserId = UserID;
            _El_Dynamic_Report.ReportId = ReportTypeId;
            _El_Dynamic_Report.iReportCriteriaID = ReportId;
            _El_Dynamic_Report.TimeZoneId = TimeZoneID;
            _El_Dynamic_Report.ClientID = Convert.ToInt32(_ds.Tables[0].Rows[0]["clientID"]);


            var dt = ReportResolver(_El_Dynamic_Report);           

            return dt;
        }


        public DataTable ReportResolver(El_Custom_Report _El_Dynamic_Report)
        {

            var dt = new DataTable();

           switch (_El_Dynamic_Report.TemplateType)
            {
                //special reports 

                //alerts report
                case 1:

                    var _DAL_Reports =new DAL_Reports(); ;

                    var _alarts = _DAL_Reports.GetAlertDynamicData(_El_Dynamic_Report);                 

                    dt = ChangeColumns(_alarts, _El_Dynamic_Report); 

                    break;

                default:

                    //all violatins 
                    if (_El_Dynamic_Report.TemplateType > 15 && _El_Dynamic_Report.TemplateType < 24)
                    {
                        clsReports_Project cls = new clsReports_Project();

                        var _violation = cls.GetReportViolations(_El_Dynamic_Report.TimeZoneId, _El_Dynamic_Report.UserId, _El_Dynamic_Report.TemplateType, _El_Dynamic_Report.iReportCriteriaID,  2);

                        

                        dt = ChangeColumns(_violation, _El_Dynamic_Report); ;
                    }

                    //trips & stoppages
                    if (_El_Dynamic_Report.TemplateType == 12 || _El_Dynamic_Report.TemplateType == 47)
                    {


                        var _ds = _clsTrips.GetPreliminaryInfo(_El_Dynamic_Report.UserId, _El_Dynamic_Report.iReportCriteriaID, 1, _El_Dynamic_Report.ReportId);

                        var _dtCompanyLogo = _ds.Tables[1].Copy();

                        var _Assets = GetAssets(_ds);

                        var dates = getHeaderParticulars(_ds);

                        var TempDS = _clsTrips.GetDirtyTripSummaryForMultipleAssets(_Assets.vpkDeviceIDCSV, dates.Item1, dates.Item2, _El_Dynamic_Report.TimeZoneId);

                        dt = ChangeColumns(TempDS, _Assets, dates, _El_Dynamic_Report.TimeZoneId);

                    }
                    
                    break;
            }
            
            return dt;
        }

        public El_Device GetAssets(DataSet ds)
        {


            var assetTbl = ds.Tables[2];


            StringBuilder sb = new StringBuilder();

            var _El_Device = new El_Device();

            foreach (DataRow row in assetTbl.Rows)
            {


                var _RawData = new Extended_Device
                {
                    StartOdometer = row.FieldOrDefault("startOdometer"),
                    EndOdometer = row.FieldOrDefault("endOdometer"),
                    Distance = row.FieldOrDefault("Distance")
                };
               
                _El_Device.Devices.Add(new El_Device(Convert.ToInt64(row["vpkDeviceID"].ToString()), row["vDeviceName"].ToString(), _RawData));

                sb.Append(row[ "vpkDeviceID"].ToString() + ',');

            }
            sb.Append('0');

            _El_Device.vpkDeviceIDCSV = sb.ToString();

            return _El_Device;
        }
        public Tuple<DateTime, DateTime, int,string,string,string> getHeaderParticulars(DataSet ds)
        {

            var datesTbl = ds.Tables[0];

            var startDate = Convert.ToDateTime(datesTbl.Rows[0]["StartDate"]);

            var endDate = Convert.ToDateTime(datesTbl.Rows[0]["EndDate"]);

            var intMeasurementUnit = Convert.ToInt32(datesTbl.Rows[0]["MeasurementUnit"]);


            var reportName = Convert.ToString(datesTbl.Rows[0]["ReportName"]);

            var assetName = Convert.ToString(datesTbl.Rows[0]["AssetName"]);

            var logo = Convert.ToString(ds.Tables[1].Rows[0]["vLogo"]);




            return new Tuple<DateTime, DateTime, int,string,string,string>(startDate, endDate, intMeasurementUnit,reportName, assetName,logo);
        }
        //vioalations section
        public DataTable ChangeColumns(DataSet ds, El_Custom_Report el_custom_model)
        {
            var dt = new DataTable();

            var logo = "";

            if (el_custom_model.TemplateType == 1)
            {
                dt = ds.Tables[0];

                if (ds.Tables.Count > 0)
                    logo = ds.Tables[1].Rows[0]["vLogo"].ToString();

                dt.Columns["dAge"].ColumnName = "dbdate";
                dt.Columns["vVehicleSpeed"].ColumnName = "speed";
                dt.Columns["vAsset"].ColumnName = "asset";
                dt.Columns["iBatteryBackup"].ColumnName = "battery";
                dt.Columns["vLongitude"].ColumnName = "longitude";
                dt.Columns["vLatitude"].ColumnName = "latitude";
                dt.Columns["vDeviceName"].ColumnName = "devicename";
                dt.Columns["vEventName"].ColumnName = "event";
                dt.Columns["vTextPosition"].ColumnName = "location";
                dt.Columns["DriverName"].ColumnName = "driver";
                dt.Columns["vReportName"].ColumnName = "reportname";
                dt.Columns["firstDate"].ColumnName = "fdate";
                dt.Columns["lastDate"].ColumnName = "sdate";
            }
            else if (el_custom_model.TemplateType > 15 && el_custom_model.TemplateType < 24)
            {
                dt = ds.Tables[2];
                if (ds.Tables.Count > 0)
                    logo = ds.Tables[1].Rows[0]["vLogo"].ToString();

                dt.Columns["dDeviceSentDate"].ColumnName = "dbdate";
                dt.Columns["vLongitude"].ColumnName = "longitude";
                dt.Columns["vLatitude"].ColumnName = "latitude";
                dt.Columns["vEventName"].ColumnName = "event";
                dt.Columns["vTextPosition"].ColumnName = "location";
                dt.Columns["DriverName"].ColumnName = "driver";
                dt.Columns["vOdometer"].ColumnName = "odometer";
                dt.Columns["vDeviceName"].ColumnName = "devicename";
                dt.Columns["vVehicleSpeed"].ColumnName = "speed";
                dt.Columns["vRoadSpeed"].ColumnName = "roadspeed";
                dt.Columns["Difference"].ColumnName = "difference";


                dt.Columns.Add("asset", typeof(string));
                dt.Rows[0].SetField("asset", ds.Tables[0].Rows[0]["AssetName"].ToString());

                dt.Columns.Add("reportname", typeof(string));
                dt.Rows[0].SetField("reportname", ds.Tables[0].Rows[0]["ReportName"]);

                dt.Columns.Add("fdate", typeof(DateTime));
                dt.Rows[0].SetField("fdate", Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"]));

                dt.Columns.Add("sdate", typeof(DateTime));
                dt.Rows[0].SetField("sdate", Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"]));
            }

        


            dt.Columns["vpkDeviceID"].ColumnName = "vpkdeviceid";

            dt.Columns.Add("genarateddate", typeof(string));
            dt.AcceptChanges();

            // Features.AddRange(new string[] { "devicename", "date", "time", "genarateddate", "vpkdeviceid", "reportname", "asset", "firstdate", "lastdate" });
               Features.AddRange(new string[] { "date", "time", "genarateddate", "devicename", "vpkdeviceid", "reportname", "asset", "firstdate", "lastdate","logo" });

            var match = Features.FirstOrDefault(stringToCheck => stringToCheck.Contains("time"));

            DateTime today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(DateTime.UtcNow), "UTC", el_custom_model.TimeZoneId);

            if (match != null)
            {
                dt.Columns.Add("time", typeof(string));
                dt.Columns.Add("date", typeof(string));

                dt.Columns.Add("firstdate", typeof(string));
                dt.Columns.Add("lastdate", typeof(string));
                dt.Columns.Add("logo", typeof(string));
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    row["time"] = Convert.ToDateTime(row["dbdate"]).ToString("HH:mm:ss");
                    row["date"] = Convert.ToDateTime(row["dbdate"]).ToString("ddd MMM yyyy");
                    row["genarateddate"] = today.ToString("ddd dddd MMM yyyy HH:mm:ss");
                    if (i < 1)
                    {
                        row["firstdate"] = String.Format("Duration starting on: {0}", Convert.ToDateTime(row["fdate"]).ToString("dd MMM yyyy HH:mm:ss"));
                        row["lastdate"] = String.Format("Duration ending  on: {0}", Convert.ToDateTime(row["sdate"]).ToString("dd MMM yyyy HH:mm:ss"));
                        row["logo"] = logo;

                    }
                    i++;
                }

            }


            var keepColNames = Features;
            var allColumns = dt.Columns.Cast<DataColumn>();
            var allColNames = allColumns.Select(c => c.ColumnName).ToList();

            IEnumerable<string> removeColNames = allColNames.Except(keepColNames);

            var colsToRemove = from r in removeColNames
                               join c in allColumns on r equals c.ColumnName
                               select c;

            while (colsToRemove.Any())
                dt.Columns.Remove(colsToRemove.First());


            dt.AcceptChanges();

            return dt;
        }

       //trips section 
        public DataTable ChangeColumns(DataSet _ds, El_Device _El_Device, Tuple<DateTime, DateTime, int,string,string,string> _headerParticulars, string TimeZoneID)
        {
            DateTime today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(DateTime.UtcNow), "UTC", TimeZoneID);

            Features.AddRange(new string[] { "date", "time", "genarateddate", "devicename","vpkdeviceid", "reportname", "asset", "firstdate", "lastdate","logo" });

            var dt = new DataTable();

            dt.Columns.Add("date", typeof(string));
            dt.Columns.Add("starttime", typeof(string));
            dt.Columns.Add("endtime", typeof(string));
            dt.Columns.Add("distance", typeof(string));
            dt.Columns.Add("duration", typeof(TimeSpan));
            dt.Columns.Add("avgspeed", typeof(string));
            dt.Columns.Add("maxspeed", typeof(string));
            dt.Columns.Add("from", typeof(string));
            dt.Columns.Add("to", typeof(string));
            dt.Columns.Add("vpkdeviceid", typeof(long));
            dt.Columns.Add("genarateddate", typeof(string));
            dt.Columns.Add("asset", typeof(string));   
            dt.Columns.Add("reportname", typeof(string));
            dt.Columns.Add("firstdate", typeof(string));
            dt.Columns.Add("lastdate", typeof(string));
            dt.Columns.Add("devicename", typeof(string));
            dt.Columns.Add("logo", typeof(string));
            foreach (El_Device _Device in _El_Device.Devices)
            {

                DataTable table = new DataTable();

                if (_ds.Tables.Count > 0)
                {
                    table = _ds.Tables[0].Copy();
                    table.DefaultView.RowFilter = "vpkDeviceID  =" + _Device.vpkDeviceID;
                    table = table.DefaultView.ToTable();

                }

                DataSet ds = new DataSet();

                ds.Tables.Add(table);

                List<clsPopulateTripSummary> CleansummeryList = new List<clsPopulateTripSummary>();

                CleansummeryList = _clsTrips.CleanDirtyTripSummary(ds, _El_Device.CompanyId, _Device.vpkDeviceID, _headerParticulars.Item1, _headerParticulars.Item2, TimeZoneID, false);

                var cleanTrips = _clsTrips.RemoveZeroDistanceFromCleanedTripSummary(0.01, CleansummeryList, TimeZoneID);

                foreach (var record in cleanTrips)
                {
                    var diff = Convert.ToDateTime(record.EndDate).Subtract(Convert.ToDateTime(record.StartDate));

                    DataRow row = dt.NewRow();

                    if (diff.TotalHours != 0)
                    {
                        row["avgspeed"] = UserSettings.ConvertKMsToXx(_headerParticulars.Item3, Convert.ToString(Math.Round(Convert.ToDouble(record.Distance / diff.TotalHours), 2)), false, 2) + UserSettings.GetSpeedUnitName(_headerParticulars.Item3);
                        //Math.Round(Convert.ToDouble(row.Distance / diff.TotalHours), 2) + UserSettings.GetOdometerUnitName(intMeasurementUnit); ;

                    }
                    else
                    {
                        row["avgspeed"] = "0 " + UserSettings.GetSpeedUnitName(_headerParticulars.Item3);
                    }


                    row["date"] = record.StartDate.ToString("ddd dd MM-yyy"); ;
                    row["starttime"] = record.StartDate.ToString("HH:mm:ss");
                    row["endtime"] = record.EndDate.ToString("HH:mm:ss"); ;
                    row["distance"] = record.Distance;
                    row["devicename"] = _Device.deviceName;
                    row["duration"] = record.Duration;
                    row["maxspeed"] = 0; //clsReports_Project.GetTripSummary_GetMaxSpeedOfSpecificTrip(record.IdStart, record.IdEnd, record.VpkDeviceID, dates.Item3);
                    row["from"] = record.LocationStart;
                    row["to"] = record.LocationEnd;
                    row["vpkdeviceid"] = _Device.vpkDeviceID;

                    dt.Rows.Add(row);

                }
                     
            }

            dt.Rows[0].SetField("genarateddate", today.ToString("dddd MMM yyyy HH:mm:ss"));
            dt.Rows[0].SetField("firstdate", String.Format("Report period from: {0}", _headerParticulars.Item1.ToString("ddd MMM yyyy HH:mm:ss")));
            dt.Rows[0].SetField("lastdate", String.Format("Report period to: {0}",_headerParticulars.Item2.ToString("ddd MMM yyyy HH:mm:ss")));           
            dt.Rows[0].SetField("reportname", _headerParticulars.Item4);
            dt.Rows[0].SetField("asset", _headerParticulars.Item5);
            dt.Rows[0].SetField("logo", _headerParticulars.Item6);

            var keepColNames = Features;
            var allColumns = dt.Columns.Cast<DataColumn>();
            var allColNames = allColumns.Select(c => c.ColumnName).ToList();

            IEnumerable<string> removeColNames = allColNames.Except(keepColNames);

            var colsToRemove = from r in removeColNames
                               join c in allColumns on r equals c.ColumnName
                               select c;

            while (colsToRemove.Any())
                dt.Columns.Remove(colsToRemove.First());


            dt.AcceptChanges();

            return dt; 

        }
    }
    public static class DataTableExtensions
    {

        public static void SetColumnsOrder(this DataTable dtbl, params String[] columnNames)
        {
            List<string> listColNames = columnNames.ToList();

            //Remove invalid column names.
            foreach (string colName in columnNames)
            {
                if (!dtbl.Columns.Contains(colName))
                {
                    listColNames.Remove(colName);
                }
            }

            foreach (string colName in listColNames)
            {
                dtbl.Columns[colName].SetOrdinal(listColNames.IndexOf(colName));
            }
        }
    }
}
