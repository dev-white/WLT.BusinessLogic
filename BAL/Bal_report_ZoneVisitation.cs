using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using WLT.BusinessLogic.BAL;
using WLT.DataAccessLayer.DAL;
using System.Configuration;
using System.Data;

namespace WLT.BusinessLogic.BAL
{
    using Newtonsoft.Json;
    using System.Linq;
    using WLT.EntityLayer;

    public class BAL_report_ZoneVisitation
    {
        public int Userid { get; set; }
        public int ReportId { get; set; }
        public string TimeZoneID { get; set; }

        EL_DatesFilter _EL_DatesFilter { get; set; }
        public DataSet GetZoneData()
        {

            //access the database values and get data

            var _DAL_Reports = new DAL_Reports();

            var ds = _DAL_Reports.GetZoneVisitation(Userid, ReportId);

            //get particular device Ids  that are not duplicate to be used for looping through data
            var deviceList = (from q in ds.Tables[0].AsEnumerable()
                              select q["vpkDeviceID"]).Distinct();


            var _ZoneVisitationItems = new ZoneVisitationItems();


            if (ds.Tables[1].Rows[0]["TimeRange"].ToString() != null && ds.Tables[1].Rows[0]["TimeRange"].ToString() != string.Empty)
            {
                var _filter_O = JsonConvert.DeserializeObject<EL_DatesFilter>(ds.Tables[1].Rows[0]["TimeRange"].ToString());
                _filter_O.bAllowFilter = Convert.ToBoolean(ds.Tables[1].Rows[0]["isCustomTimeEnabled"]);
                _filter_O.iTimeFilterType = Convert.ToInt32(ds.Tables[1].Rows[0]["iEnabledDateType"]);
                _EL_DatesFilter = _filter_O;


            }
            else
            {
                _EL_DatesFilter = new EL_DatesFilter();
            }


            //loop the the asset specific data for processing  
            foreach (var ID in deviceList)
            {

                //Filter the database value and get data per asset  int variable @deviceList
                var data = ds.Tables[0].Select("vpkDeviceID = " + ID, "dGpsDatetime asc");


                foreach (var row in data)
                {

                    DateTime dDeviceSentDate = Convert.ToDateTime(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(Convert.ToDateTime(row["dGpsDatetime"]), "UTC", TimeZoneID));

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
                    }

                    _ZoneVisitationItems.Datum = row;

                    _ZoneVisitationItems.TimeZoneID = TimeZoneID;

                    //handle locations zones 
                    if (Convert.ToInt32(row["EventId"]) == 800 || Convert.ToInt32(row["EventId"]) == 801)
                        _ZoneVisitationItems.HandleLocation();

                    //handle no go zones 
                    if (Convert.ToInt32(row["EventId"]) == 802 || Convert.ToInt32(row["EventId"]) == 803)
                        _ZoneVisitationItems.HandleNo_Go();

                    //handle locations 
                    if (Convert.ToInt32(row["EventId"]) == 804 || Convert.ToInt32(row["EventId"]) == 805)
                        _ZoneVisitationItems.HandleKeep_In();

                }




            }

            var merger = new DataTable();

            merger.Merge(_ZoneVisitationItems.LOCATION);

            merger.Merge(_ZoneVisitationItems.kEEP_IN);

            merger.Merge(_ZoneVisitationItems.NO_GO);

            ds.Tables.RemoveAt(0);

            ds.Tables.Add(merger);

            return ds;
        }
    }

    public class ZoneVisitationItems
    {
        public DataTable NO_GO { get; set; }
        public DataTable kEEP_IN { get; set; }
        public DataTable LOCATION { get; set; }
        public DataRow Datum { get; set; }
        public string TimeZoneID { set; get; }




        int _rid = 0;

        int ifkZoneId = 0;

        int deviceId = 0;

        public ZoneVisitationItems()
        {
            var _dt = Definition().Clone();

            NO_GO = _dt.Clone();

            kEEP_IN = _dt.Clone();

            LOCATION = _dt.Clone();

        }

        public void HandleNo_Go()
        {
            var _row = Initialize();

            _row["startheader"] = "Entered";

            _row["endheader"] = "Exited";

            if (NO_GO.Select("ifkZoneId =" + ifkZoneId + " and vpkDeviceID = " + deviceId).Count() == 0)
            {
                if (_rid == 803)  // started the record with exited no_go 
                {
                    _row["flagIn"] = 1;

                    _row["message"] = "no entry in report period";
                }
                else
                {
                    _row["flagOut"] = 1;  // no exit flag                     

                    _row["message"] = "no exit in report period";
                }
                NO_GO.Rows.Add(_row.ItemArray);
            }
            else
            {
                /// check last row if theres complementing vreportid 
                var lastRow = NO_GO.Select("ifkZoneId =" + ifkZoneId + " and vpkDeviceID = " + deviceId, "date desc")[0];


                if (Convert.ToInt32(lastRow["vReportId"]) == 802 && _rid == 803)
                {
                    lastRow["exit"] = _row["date"];

                    var _duration = (Convert.ToDateTime(_row["date"]) - Convert.ToDateTime(lastRow["date"]));

                    lastRow["duration"] = _duration.TotalSeconds;

                    lastRow["durationTimeSpan"] = _duration;

                    lastRow["vReportId"] = _rid;

                    lastRow["flagOut"] = 0;

                    lastRow["flagIn"] = 0;
                }

                else if (Convert.ToInt32(lastRow["vReportId"]) == 803 && _rid == 802)
                {
                    _row["flagOut"] = 1;  // no exit flag                     

                    _row["message"] = "no exit in report period";

                    NO_GO.Rows.Add(_row.ItemArray);
                }


            }
        }
        public void HandleKeep_In()
        {
            var _row = Initialize();

            _row["startheader"] = "Exited ";

            _row["endheader"] = "Entered ";

            if (kEEP_IN.Select("ifkZoneId =" + ifkZoneId + " and vpkDeviceID = " + deviceId).Count() == 0)
            {
                if (_rid == 804)  // started the record with exited no_go 
                {
                    _row["flagIn"] = 1;

                    _row["message"] = "no exit in report period";

                }
                else
                {
                    _row["flagOut"] = 1;

                    _row["message"] = "no exit in report period";
                }

                kEEP_IN.Rows.Add(_row.ItemArray);
            }
            else
            {
                /// check last row if theres complementing vreportid 
                var lastRow = kEEP_IN.Select("ifkZoneId =" + ifkZoneId + " and vpkDeviceID = " + deviceId, "date desc")[0];

                if (Convert.ToInt32(lastRow["vReportId"]) == 805 && _rid == 804)
                {
                    lastRow["exit"] = _row["date"];

                    var _duration = (Convert.ToDateTime(_row["date"]) - Convert.ToDateTime(lastRow["date"]));

                    lastRow["duration"] = _duration.TotalSeconds;

                    lastRow["durationTimeSpan"] = _duration;

                    lastRow["flagOut"] = 0;

                    lastRow["flagIn"] = 0;

                    lastRow["vReportId"] = _rid;
                }

                else if (Convert.ToInt32(lastRow["vReportId"]) == 804 && _rid == 805)
                {
                    _row["flagOut"] = 1;  // no exit flag 

                    _row["message"] = "no entry in report period";

                    kEEP_IN.Rows.Add(_row.ItemArray);
                }


            }

        }

        public void HandleLocation()
        {
            var _row = Initialize();

            _row["startheader"] = "Entered";

            _row["endheader"] = "Exited";

            if (LOCATION.Select("ifkZoneId =" + ifkZoneId + " and vpkDeviceID = " + deviceId).Count() == 0)
            {
                if (_rid == 801)  // started the record with exited no_go 
                {
                    _row["flagIn"] = 1;

                    _row["message"] = "no entry in report period";
                }
                else
                {
                    _row["flagOut"] = 1;

                }
                LOCATION.Rows.Add(_row.ItemArray);
            }
            else
            {
                /// check last row if theres complementing vreportid 
                var lastRow = LOCATION.Select("ifkZoneId =" + ifkZoneId + " and vpkDeviceID = " + deviceId, "date desc")[0];

                if (Convert.ToInt32(lastRow["vReportId"]) == 800 && _rid == 801)
                {
                    lastRow["exit"] = _row["date"];

                    var _duration = Convert.ToDateTime(_row["date"]) - Convert.ToDateTime(lastRow["date"]);

                    lastRow["duration"] = _duration.TotalSeconds;

                    lastRow["durationTimeSpan"] = _duration;

                    lastRow["vReportId"] = _rid;

                    lastRow["flagOut"] = 0;

                    lastRow["flagIn"] = 0;
                }

                else if (Convert.ToInt32(lastRow["vReportId"]) == 801 && _rid == 800)
                {
                    _row["flagOut"] = 1;  // no exit flag  

                    LOCATION.Rows.Add(_row.ItemArray);
                }


            }

        }

        private DataRow Initialize()
        {

            _rid = Convert.ToInt32(Datum["EventId"]);

            ifkZoneId = Convert.ToInt32(Datum["ifkZoneId"]);

            deviceId = Convert.ToInt32(Datum["vpkDeviceID"]);

            var dt = Definition().Clone();

            var dr = dt.NewRow();

            var _eventDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(Datum["dGpsDatetime"]), TimeZoneID);

            dr["ifkZoneId"] = Datum["ifkZoneId"];
            dr["vReportId"] = Datum["EventId"];
            dr["deviceName"] = Datum["Asset"];
            dr["date"] = _eventDate;
            dr["vLatitude"] = Convert.ToDouble(Datum["vLatitude"]);
            dr["vLongitude"] = Convert.ToDouble(Datum["vLongitude"]);
            dr["Asset"] = Convert.ToString(Datum["Asset"]);
            dr["vGeoName"] = Convert.ToString(Datum["vGeoName"]);
            dr["vGeofenceType"] = String.IsNullOrEmpty(Convert.ToString(Datum["vGeofenceType"])) ? "Zone type  : Unspecified " : String.Format("Zone type: {0}", Convert.ToString(Datum["vGeofenceType"]));
            dr["vEventName"] = Convert.ToString(Datum["vEventName"]);
            dr["vpkDeviceID"] = Convert.ToString(Datum["vpkDeviceID"]);
            dr["durationTimeSpan"] = TimeSpan.FromSeconds(0);
            dr["duration"] = 0;
            dr["message"] = "no exit in report period";
            dr["durationTimeSpan"] = TimeSpan.FromSeconds(0);
            dr["entry"] = _eventDate;
            dr["exit"] = _eventDate;

            return dr;

        }
        private DataTable Definition()
        {
            var dt = new DataTable();

            dt.Columns.Add("ifkZoneId", typeof(int));
            dt.Columns.Add("vReportId", typeof(int));
            dt.Columns.Add("flagIn", typeof(int));
            dt.Columns.Add("flagOut", typeof(int));
            dt.Columns.Add("deviceName", typeof(string));
            dt.Columns.Add("date", typeof(DateTime));
            dt.Columns.Add("entry", typeof(DateTime));
            dt.Columns.Add("exit", typeof(DateTime));
            dt.Columns.Add("durationStr", typeof(DateTime));
            dt.Columns.Add("duration", typeof(double));
            dt.Columns.Add("durationTimeSpan", typeof(TimeSpan));
            dt.Columns.Add("vGeoName", typeof(string));
            dt.Columns.Add("message", typeof(string));
            dt.Columns.Add("vGeofenceType", typeof(string));
            dt.Columns.Add("vLatitude", typeof(double));
            dt.Columns.Add("vLongitude", typeof(double));
            dt.Columns.Add("Asset", typeof(string));
            dt.Columns.Add("vEventName", typeof(string));
            dt.Columns.Add("vpkDeviceID", typeof(long));
            dt.Columns.Add("startheader", typeof(string));
            dt.Columns.Add("endheader", typeof(string));
            return dt;
        }
    }

}
