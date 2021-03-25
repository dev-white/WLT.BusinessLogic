//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI.WebControls;
//using System.Web.Script.Services;
//using System.Web.Services;
//using System.Data;
//using System.Configuration;
//using System.Text;
//using WLT.BusinessLogic.Bal_GPSOL;
//using WLT.BusinessLogic.Bal_GPSOL;
//using System.Data.SqlClient;
//using System.Collections;
//using System.Globalization;
//using System.IO;
//using WLT.DataAccessLayer;
//using System.Device.Location;
////using Telerik.Web.UI;
////using eis = Telerik.Web.UI.ExportInfrastructure;
//using WLT;
//using System.Web.UI;
////using GPSOL.Usercontrols;
//using System.Web.UI.HtmlControls;
//using System.Text.RegularExpressions;

//using WLT.BusinessLogic;
//using WLT.BusinessLogic.BAL;
//using WLT.EntityLayer;
//using System.Collections.ObjectModel;
//using Newtonsoft.Json;


//namespace WLT.BusinessLogic
//{
//    class clsTripDetails
//    {

//        [WebMethod]
//        [ScriptMethod]
//        public static List<clsTripReplay> TripReplayEventInst(string TripAssetDeviceIDShowOnMap, DateTime TripSelectedDate, int iTrackerType, bool isall, int selectalldata, string ifkCompanyID)
//        {
//            //int selectalldatavalue = 1;
//            List<clsTripReplay> listLeftMenu = new List<clsTripReplay>();
//            int tripReplaycount = 0;
//            double totalDistanceForDay = 0;
//            string odometerUnit = "";
//            string Stime = "";
//            string Etime = "";
//            string Sseconds = "";
//            string Eseconds = "";
//            DateTime startTime = DateTime.UtcNow;
//            DateTime endTime = DateTime.UtcNow;
//            string StartLocation = "";
//            string EndLocation = "";
//            clsTripReplay objTripReplay = new clsTripReplay();
//            DataSet dsTrip = new DataSet();
//            DateTime dTripSelectedDate = System.DateTime.UtcNow;
//            StringBuilder sbtrip = new StringBuilder();
//            string dqry = "";
//            bool currrenttrip = false;
//            clsRegistration objRegisteration = new clsRegistration();

//            if (Convert.ToString(HttpContext.Current.Session["blnIsLoggedIn"]) == "" || HttpContext.Current.Session["blnIsLoggedIn"] == null)
//            {
//                //  var data = new { status = "login" };  return JsonConvert.SerializeObject(data);
//                listLeftMenu.Add(new clsTripReplay(0, "login", ""));
//            }
//            else
//            {
//                string TimeZoneID = "";
//                double distance = 0.0;
//                objRegisteration = (clsRegistration)HttpContext.Current.Session["clsRegistration"];
//                TimeZoneID = objRegisteration.vTimeZoneID;
//                odometerUnit = UserSettings.GetOdometerUnitName(objRegisteration.ifkMeasurementUnit);

//                try
//                {

//                    string querytrackertype = "";

//                    if (TripAssetDeviceIDShowOnMap == "headclicked")
//                    {
//                        querytrackertype = "SELECT ifk_TrackerType FROM wlt_tblDevices A left outer join wlt_tblAssets B on B.Id=A.ifk_AssignedAssetId WHERE ImeiNumber = 0";//TripAssetDeviceIDShowOnMap + "";
//                    }
//                    else
//                    {
//                        querytrackertype = "SELECT ifk_TrackerType FROM wlt_tblDevices A left outer join wlt_tblAssets B on B.Id=A.ifk_AssignedAssetId WHERE ImeiNumber =" + TripAssetDeviceIDShowOnMap;//TripAssetDeviceIDShowOnMap + "";
//                    }

//                    int iTrackerTypeAll = Convert.ToInt32(SqlHelper.ExecuteScalar(AppConfiguration.ConnectionString().ToString(), CommandType.Text, querytrackertype));
//                    dsTrip = SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString().ToString(), CommandType.StoredProcedure, "sp_TripDetail");
//                    SqlParameter[] param = new SqlParameter[1];
//                    param[0] = new SqlParameter("@ipkCommanTrackingID", SqlDbType.Int);
//                    //param[0].Value = ipkCommanTrackingID;
//                    // dsTrip = (SqlHelper.ExecuteDataset(AppConfiguration.ConnectionString().ToString(), "sp_TripDetail","clsTripDetails" ,CommandType.StoredProcedure));
//                    string _strStartDate = "";
//                    string _strEndDate = "";
//                    if (TripSelectedDate.ToString() == "1/1/0001 12:00:00 AM")
//                    {
//                        _strStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime(dTripSelectedDate, TimeZoneID);
//                        _strEndDate = UserSettings.ConvertLocalDateTimeToUTCDateTime(dTripSelectedDate.AddHours(24).AddSeconds(-1), TimeZoneID);
//                    }
//                    else
//                    {
//                        dTripSelectedDate = TripSelectedDate;

//                        _strStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime(dTripSelectedDate, TimeZoneID);
//                        _strEndDate = UserSettings.ConvertLocalDateTimeToUTCDateTime(dTripSelectedDate.AddDays(1), TimeZoneID);

//                    }

//                    objTripReplay.ifkCompanyID = Convert.ToInt32(ifkCompanyID);
//                    objTripReplay.dDateFrom = Convert.ToDateTime(_strStartDate);
//                    objTripReplay.dDateTo = Convert.ToDateTime(_strEndDate);

//                    if (iTrackerTypeAll == 3 || iTrackerTypeAll == 2) // only for vehicle devices and completed
//                    {
//                        objTripReplay.vpkDeviceID = TripAssetDeviceIDShowOnMap;
//                    }

//                    dsTrip = objTripReplay.GetTripReplayList();

//                    DataView view = new DataView(dsTrip.Tables[0]);
//                    DataTable _dtDevices = view.ToTable(true, "vpkDeviceID");

//                    DataTable _dt = new DataTable();
//                    _dt.Columns.Add("vpkDeviceID");
//                    _dt.Columns.Add("strStartTime", System.Type.GetType("System.DateTime"));
//                    _dt.Columns.Add("strEndTime", System.Type.GetType("System.DateTime"));

//                    foreach (DataRow _drDevices in _dtDevices.Rows)
//                    {
//                        string _dtStartDate = dTripSelectedDate.ToString("yyyy-MM-dd HH:mm:ss tt");
//                        string _dtEndDate = dTripSelectedDate.AddDays(1).AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss tt");
//                        string _strDeviceId = _drDevices["vpkDeviceID"].ToString();

//                        DataRow[] _drSelectedDevices = dsTrip.Tables[0].Select("vpkDeviceID = " + _strDeviceId);//, "dGPSDateTime asc"

//                        bool _isFirstRecord = true;
//                        int _intCount = 0;
//                        int _index = 0;
//                        while (_intCount < _drSelectedDevices.Length)
//                        {

//                            //int _index = _intCount + 1;
//                            //if (_intCount == _drSelectedDevices.Length - 1)
//                            //{
//                            //    _index = _intCount;
//                            //}


//                            //start new code by sanket

//                            while (_intCount < _drSelectedDevices.Length)
//                            {
//                                if (_drSelectedDevices[_intCount]["vReportID"].ToString() == "8")
//                                {
//                                    _index = _intCount + 1;
//                                    while (_index < _drSelectedDevices.Length)
//                                    {
//                                        if (_drSelectedDevices[_index]["vReportID"].ToString() == "8")
//                                        {
//                                            _index--;
//                                            goto tripStart;
//                                        }
//                                        _index++;
//                                    }

//                                tripStart:
//                                    if (_intCount == _index)
//                                    {
//                                        _index = _intCount + 1;
//                                    }
//                                    else if (_drSelectedDevices.Length == _index)
//                                    {
//                                        _index--;
//                                    }
//                                    break;
//                                }
//                                else
//                                {
//                                    if (_intCount == 0)
//                                    {
//                                        _index = _intCount + 1;
//                                        break;
//                                    }
//                                    _intCount++;
//                                }
//                            }
//                            //end new code by sanket

//                            if (_drSelectedDevices.Length == _index)
//                            {
//                                _index--;
//                            }

//                            Boolean _isInProgress = false;
//                            Boolean _isOffMissing = false;

//                            //if (_drSelectedDevices[_intCount]["vReportID"].ToString() == "17" || _drSelectedDevices[_intCount]["vReportID"].ToString() == "18")
//                            //{
//                            //    string _str = "";
//                            //}

//                            if (_drSelectedDevices[_intCount]["vReportID"].ToString() == "7")//|| _drSelectedDevices[_intCount]["vReportID"].ToString() == "17") //if ignition off is missing
//                            {
//                                //if (Convert.ToDateTime(_dtStartDate).ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy") && _isFirstRecord == true)
//                                if (Convert.ToDateTime(_dtStartDate) > DateTime.Today.AddDays(-3) && _isFirstRecord == true)
//                                {
//                                    _isInProgress = true;
//                                }
//                                else
//                                {
//                                    _isOffMissing = true;
//                                }
//                            }

//                            distance = 0.0;
//                            DateTime _strEndTime, _strStartTime;

//                            if (_isInProgress == true)
//                            {
//                                _strStartTime = Convert.ToDateTime(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                _strEndTime = DateTime.Now;
//                            }
//                            else if (_isOffMissing == true)
//                            {
//                                _strStartTime = Convert.ToDateTime(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                if (_intCount == 0)
//                                {
//                                    _strEndTime = Convert.ToDateTime(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                }
//                                else
//                                {
//                                    _strEndTime = Convert.ToDateTime(_drSelectedDevices[_intCount - 1]["dGPSDateTime"]);
//                                }
//                            }
//                            else
//                            {
//                                _strEndTime = Convert.ToDateTime(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                _strStartTime = Convert.ToDateTime(_drSelectedDevices[_index]["dGPSDateTime"]);
//                            }


//                            //start new code to check duplicate in trip replay
//                            string _strDuplicateQuery = "vpkDeviceID = '" + _strDeviceId + "'";
//                            _strDuplicateQuery += " and strStartTime = #" + _strStartTime.ToString("MM/dd/yyyy HH:mm:ss tt") + "#";
//                            _strDuplicateQuery += " and strEndTime = #" + _strEndTime.ToString("MM/dd/yyyy HH:mm:ss tt") + "#";
//                            DataRow[] _drExistsDuplicate = _dt.Select(_strDuplicateQuery);

//                            if (_drExistsDuplicate.Length > 0)
//                            {
//                                if (_isInProgress == false)
//                                {
//                                    if (_drSelectedDevices[_intCount]["vReportID"].ToString() == "8") //|| _drSelectedDevices[_intCount]["vReportID"].ToString() == "18") && _drSelectedDevices[_index]["vReportID"].ToString() == "7") //if ignition on is missing
//                                    {
//                                        _intCount += 2;
//                                    }
//                                    else
//                                    {
//                                        _intCount += 1;
//                                    }
//                                }
//                                else
//                                {
//                                    _intCount += 1;
//                                }
//                                continue;
//                            }

//                            DataRow _dr = _dt.NewRow();
//                            _dr["vpkDeviceID"] = _drDevices["vpkDeviceID"].ToString();
//                            _dr["strStartTime"] = _strStartTime; // _strStartTime.ToString("MM/dd/yyyy HH:mm:ss tt");
//                            _dr["strEndTime"] = _strEndTime; // _strEndTime.ToString("MM/dd/yyyy HH:mm:ss tt");
//                            _dt.Rows.Add(_dr);
//                            //end new code to check duplicate in trip replay



//                            string _strQry = " dGPSDateTime >=#" + _strStartTime.ToString("MM/dd/yyyy HH:mm:ss tt") + "# ";
//                            if (_isInProgress == false)
//                            {
//                                _strQry += " and dGPSDateTime <=#" + _strEndTime.ToString("MM/dd/yyyy HH:mm:ss tt") + "# ";
//                            }
//                            _strQry += " and vpkDeviceID = " + _strDeviceId + " and vReportID in(124,7,8)"; //,17,18

//                            DataRow[] _drSelected = dsTrip.Tables[1].Select(_strQry);//, "dGPSDateTime asc"


//                            string _strFromTime = "";
//                            if (_isInProgress == true || _isOffMissing == true)
//                            {
//                                _strFromTime = UserSettings.ConvertUTCDateTimeToLocalDateTime(Convert.ToDateTime(_drSelectedDevices[_intCount]["dGPSDateTime"].ToString()), TimeZoneID);
//                                startTime = Convert.ToDateTime(_strFromTime);
//                                Stime = startTime.ToString("HH:mm");
//                                Sseconds = startTime.ToString("ss");
//                                StartLocation = _drSelectedDevices[_intCount]["vTextMessage"].ToString();
//                            }
//                            else
//                            {
//                                _strFromTime = UserSettings.ConvertUTCDateTimeToLocalDateTime(Convert.ToDateTime(_drSelectedDevices[_index]["dGPSDateTime"].ToString()), TimeZoneID);
//                                startTime = Convert.ToDateTime(_strFromTime);
//                                Stime = startTime.ToString("HH:mm");
//                                Sseconds = startTime.ToString("ss");
//                                StartLocation = _drSelectedDevices[_index]["vTextMessage"].ToString();
//                            }

//                            double avgspeed = 0.0;

//                            TimeSpan span = new TimeSpan(0, 0, 0, 0, 0);
//                            //if (_isInProgress == false)
//                            //{
//                            string _strToTime = "";
//                            if (_isOffMissing == true)
//                            {
//                                _strToTime = UserSettings.ConvertUTCDateTimeToLocalDateTime(Convert.ToDateTime(_drSelected[_drSelected.Length - 1]["dGPSDateTime"].ToString()), TimeZoneID);
//                                endTime = Convert.ToDateTime(_strToTime);
//                                Etime = endTime.ToString("HH:mm");
//                                Eseconds = endTime.ToString("ss");
//                                EndLocation = _drSelected[_drSelected.Length - 1]["vTextMessage"].ToString();

//                                if (Convert.ToDouble(_drSelected[_drSelected.Length - 1]["vOdometer"]) == 0 && _drSelected.Length > 2)
//                                {
//                                    int _i = 1;
//                                    while (_drSelected.Length - _i >= _intCount)
//                                    {
//                                        if (Convert.ToDouble(_drSelected[_drSelected.Length - _i]["vOdometer"]) > 0)
//                                        {
//                                            if (Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]) > 0)
//                                            {
//                                                //double distance1 = Convert.ToDouble(UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, _drSelected[_drSelected.Length - _i]["vOdometer"].ToString(), false, 1));
//                                                //double distance2 = Convert.ToDouble(UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, _drSelectedDevices[_intCount]["vOdometer"].ToString(), false, 1));
//                                                distance = Convert.ToDouble(_drSelected[_drSelected.Length - _i]["vOdometer"]) - Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]);
//                                                //distance = distance1 - distance2;
//                                            }
//                                            else
//                                            {
//                                                distance = 0;
//                                            }
//                                            break;
//                                            // _i = _intCount - 1;
//                                        }
//                                        else
//                                        {
//                                            _i++;
//                                        }
//                                    }

//                                }
//                                else
//                                {
//                                    //distance = Convert.ToDouble(_drSelected[_drSelected.Length - 1]["vOdometer"]) - Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]);

//                                    if (Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]) > 0)
//                                    {
//                                        //double distance1 = Convert.ToDouble(UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, _drSelected[_drSelected.Length - 1]["vOdometer"].ToString(), false, 1));
//                                        //double distance2 = Convert.ToDouble(UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, _drSelectedDevices[_intCount]["vOdometer"].ToString(), false, 1));

//                                        distance = Convert.ToDouble(_drSelected[_drSelected.Length - 1]["vOdometer"]) - Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]);
//                                        //distance = distance1 - distance2;
//                                    }
//                                    else
//                                    {
//                                        distance = 0;
//                                    }
//                                }
//                                distance = Math.Round(distance, 2, MidpointRounding.AwayFromZero);
//                                totalDistanceForDay = totalDistanceForDay + distance;
//                            }
//                            else
//                            {
//                                _strToTime = UserSettings.ConvertUTCDateTimeToLocalDateTime(Convert.ToDateTime(_drSelectedDevices[_intCount]["dGPSDateTime"].ToString()), TimeZoneID);
//                                endTime = Convert.ToDateTime(_strToTime);
//                                Etime = endTime.ToString("HH:mm");
//                                Eseconds = endTime.ToString("ss");
//                                EndLocation = _drSelectedDevices[_intCount]["vTextMessage"].ToString();

//                                if ((Convert.ToDecimal(_drSelectedDevices[_intCount]["vOdometer"]) == 0 || Convert.ToDecimal(_drSelectedDevices[_index]["vOdometer"]) == 0) && _drSelected.Length > 2)
//                                {
//                                    int _i = 1;
//                                    while (_drSelected.Length - _i >= _intCount)
//                                    {
//                                        if (Convert.ToDouble(_drSelected[_drSelected.Length - _i]["vOdometer"]) > 0)
//                                        {
//                                            if (Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]) > 0)
//                                            {
//                                                //double distance1 = Convert.ToDouble(UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, _drSelected[_drSelected.Length - _i]["vOdometer"].ToString(), false, 1));
//                                                //double distance2 = Convert.ToDouble(UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, _drSelectedDevices[_intCount]["vOdometer"].ToString(), false, 1));

//                                                distance = Convert.ToDouble(_drSelected[_drSelected.Length - _i]["vOdometer"]) - Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]);
//                                                //distance = distance1 - distance2;
//                                            }
//                                            else
//                                            {
//                                                distance = 0;
//                                            }
//                                            break;
//                                            //_i = _intCount - 1;
//                                        }
//                                        else
//                                        {
//                                            _i++;
//                                        }
//                                    }

//                                }
//                                else
//                                {
//                                    //distance = Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]) - Convert.ToDouble(_drSelectedDevices[_index]["vOdometer"]);   

//                                    if (Convert.ToDouble(_drSelectedDevices[_index]["vOdometer"]) > 0)
//                                    {
//                                        //double distance1 = Convert.ToDouble(UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, _drSelectedDevices[_intCount]["vOdometer"].ToString(), false, 1));
//                                        //double distance2 = Convert.ToDouble(UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, _drSelectedDevices[_index]["vOdometer"].ToString(), false, 1));

//                                        distance = Convert.ToDouble(_drSelectedDevices[_intCount]["vOdometer"]) - Convert.ToDouble(_drSelectedDevices[_index]["vOdometer"]);
//                                        //distance = distance1 - distance2;
//                                    }
//                                }
//                                distance = Math.Round(distance, 2, MidpointRounding.AwayFromZero);
//                            }

//                            if (distance < 0)
//                            {
//                                distance = 0;
//                            }
//                            totalDistanceForDay = totalDistanceForDay + distance;

//                            span = endTime.Subtract(startTime);

//                            if (span.Hours > 0 && span.Minutes > 0 && distance > 0)
//                            {
//                                double totaltime = Convert.ToDouble(span.Hours) + (Convert.ToDouble(span.Minutes) / 60);
//                                avgspeed = (distance / totaltime);
//                                avgspeed = Math.Round(avgspeed, 1);
//                            }
//                            else if (span.Minutes > 0 && distance > 0)
//                            {
//                                double t = (Convert.ToDouble(span.Minutes) / 60);
//                                avgspeed = (distance / t);
//                                avgspeed = Math.Round(avgspeed, 1);
//                            }
//                            //}

//                            string zeroDistanceClass = "zeroDistanceTrue";
//                            if (distance > 0)
//                            {
//                                zeroDistanceClass = "";
//                            }

//                            string _strStartIndex, _strEndIndex, _strStartDateTime, _strEndDateTime, _strDeviceIMEI;
//                            if (_isInProgress == true)
//                            {
//                                zeroDistanceClass = "";
//                                _strStartIndex = _drSelectedDevices[_intCount]["ipkCommanTrackingID"].ToString();
//                                _strEndIndex = "-1";
//                                _strStartDateTime = Convert.ToString(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                _strEndDateTime = _strStartDateTime;
//                                _strDeviceIMEI = _drSelectedDevices[_intCount]["vpkDeviceID"].ToString();

//                                sbtrip.Append("<div id='divRoute" + _drSelectedDevices[_intCount]["ipkCommanTrackingID"].ToString() + "' class='alert_new " + zeroDistanceClass + "' style='cursor:pointer; background-color:#A6BB6C;' onclick='ColorRoute(" + _strStartIndex + "," + _strEndIndex + "," + _strStartIndex + "," + _strDeviceIMEI + ",\"" + _strStartDateTime + "\",\"" + _strEndDateTime + "\");'>");
//                            }
//                            else if (_isOffMissing == true)
//                            {
//                                //sbtrip.Append("<div id='divRoute" + _drSelectedDevices[_intCount]["ipkCommanTrackingID"].ToString() + "' class='alert_new " + zeroDistanceClass + "' style='cursor:pointer;' onclick='ColorRoute(" + _drSelectedDevices[_intCount]["ipkCommanTrackingID"].ToString() + "," + _drSelected[_drSelected.Length - 1]["ipkCommanTrackingID"].ToString() + "," + _drSelectedDevices[_intCount]["ipkCommanTrackingID"].ToString() + "," + _drSelectedDevices[_intCount]["vpkDeviceID"].ToString() + ");'>");

//                                if (Convert.ToDateTime(_drSelectedDevices[_intCount]["dGPSDateTime"]) < Convert.ToDateTime(_drSelected[_drSelected.Length - 1]["dGPSDateTime"]))
//                                {
//                                    _strStartIndex = Convert.ToString(_drSelectedDevices[_intCount]["ipkCommanTrackingID"]);
//                                    _strEndIndex = Convert.ToString(_drSelected[_drSelected.Length - 1]["ipkCommanTrackingID"]);
//                                    _strStartDateTime = Convert.ToString(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                    _strEndDateTime = Convert.ToString(_drSelected[_drSelected.Length - 1]["dGPSDateTime"]);
//                                }
//                                else
//                                {
//                                    _strStartIndex = Convert.ToString(_drSelected[_drSelected.Length - 1]["ipkCommanTrackingID"]);
//                                    _strEndIndex = Convert.ToString(_drSelectedDevices[_intCount]["ipkCommanTrackingID"]);
//                                    _strStartDateTime = Convert.ToString(_drSelected[_drSelected.Length - 1]["dGPSDateTime"]);
//                                    _strEndDateTime = Convert.ToString(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                }
//                                _strDeviceIMEI = _drSelectedDevices[_intCount]["vpkDeviceID"].ToString();

//                                sbtrip.Append("<div id='divRoute" + _strStartIndex + "' class='alert_new " + zeroDistanceClass + "' style='cursor:pointer;' onclick='ColorRoute(" + _strStartIndex + "," + _strEndIndex + "," + _strStartIndex + "," + _strDeviceIMEI + ",\"" + _strStartDateTime + "\",\"" + _strEndDateTime + "\");'>");
//                            }
//                            else
//                            {

//                                if (Convert.ToDateTime(_drSelectedDevices[_index]["dGPSDateTime"]) < Convert.ToDateTime(_drSelectedDevices[_intCount]["dGPSDateTime"]))
//                                {
//                                    _strStartIndex = Convert.ToString(_drSelectedDevices[_index]["ipkCommanTrackingID"]);
//                                    _strEndIndex = Convert.ToString(_drSelectedDevices[_intCount]["ipkCommanTrackingID"]);
//                                    _strStartDateTime = Convert.ToString(_drSelectedDevices[_index]["dGPSDateTime"]);
//                                    _strEndDateTime = Convert.ToString(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                }
//                                else
//                                {
//                                    _strStartIndex = Convert.ToString(_drSelectedDevices[_intCount]["ipkCommanTrackingID"]);
//                                    _strEndIndex = Convert.ToString(_drSelectedDevices[_index]["ipkCommanTrackingID"]);
//                                    _strStartDateTime = Convert.ToString(_drSelectedDevices[_intCount]["dGPSDateTime"]);
//                                    _strEndDateTime = Convert.ToString(_drSelectedDevices[_index]["dGPSDateTime"]);
//                                }
//                                _strDeviceIMEI = _drSelectedDevices[_intCount]["vpkDeviceID"].ToString();

//                                //sbtrip.Append("<div id='divRoute" + _drSelectedDevices[_index]["ipkCommanTrackingID"].ToString() + "' class='alert_new " + zeroDistanceClass + "' style='cursor:pointer;' onclick='ColorRoute(" + _drSelectedDevices[_index]["ipkCommanTrackingID"].ToString() + "," + _drSelectedDevices[_intCount]["ipkCommanTrackingID"].ToString() + "," + _drSelectedDevices[_index]["ipkCommanTrackingID"].ToString() + "," + _drSelectedDevices[_intCount]["vpkDeviceID"].ToString() + ");'>");

//                                sbtrip.Append("<div id='divRoute" + _strStartIndex + "' class='alert_new " + zeroDistanceClass + "' style='cursor:pointer;' onclick='ColorRoute(" + _strStartIndex + "," + _strEndIndex + "," + _strStartIndex + "," + _strDeviceIMEI + ",\"" + _strStartDateTime + "\",\"" + _strEndDateTime + "\");'>");
//                            }


//                            sbtrip.Append("<div class='trip_new1'>");
//                            sbtrip.Append("<p class='trip_textforAssetName'>" + _drSelectedDevices[_intCount]["vDeviceName"].ToString() + "</p>");
//                            if (_isInProgress == false)
//                            {
//                                if (span.Hours > 0)
//                                {
//                                    if (span.Hours > 1)
//                                    {
//                                        sbtrip.Append("<p class='trip_text2'> " + " " + Stime + "<span class='tripSeconds'>:" + Sseconds + "</span>" + "&nbsp;<span data-localize='trip_To'>&nbsp; To &nbsp;</span>" + Etime + "<span class='tripSeconds'>:" + Eseconds + "&nbsp;</span>" + "<span style='float:right;'>" + UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, distance.ToString(), false, 1) + UserSettings.GetOdometerUnitName(objRegisteration.ifkMeasurementUnit) + "/" + (span.Minutes > 0 ? "" + span.Hours + "&nbsp;<span data-localize='trip_Hrs'> hrs &nbsp;</span>" + span.Minutes + "&nbsp;<span data-localize='trip_Mins'> mins &nbsp;</span>" + span.Seconds + "&nbsp;<span data-localize='trip_Secs:'> secs" : "&nbsp;</span>" + span.Hours + "&nbsp;<span data-localize='trip_Hrs'> hrs &nbsp;</span>" + span.Minutes + "&nbsp;<span data-localize='trip_Mins'> mins &nbsp;</span>" + span.Seconds + "&nbsp;<span data-localize='trip_Secs'> secs</span>") + "</span></p>");
//                                    }
//                                    else
//                                    {
//                                        sbtrip.Append("<p class='trip_text2'> " + " " + Stime + "<span class='tripSeconds'>:" + Sseconds + "</span>" + "&nbsp;<span data-localize='trip_To'>&nbsp; To  &nbsp;</span>" + Etime + "<span class='tripSeconds'>:" + Eseconds + "&nbsp;</span>" + "<span style='float:right;'>" + UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, distance.ToString(), false, 1) + UserSettings.GetOdometerUnitName(objRegisteration.ifkMeasurementUnit) + "/" + (span.Minutes > 0 ? "" + span.Hours + "&nbsp;<span data-localize='trip_Hr'> hr &nbsp;</span>" + span.Minutes + "&nbsp;<span data-localize='trip_Mins'> mins &nbsp;</span>" + span.Seconds + "&nbsp;<span data-localize='trip_Secs:'> secs" : "&nbsp;</span>" + span.Hours + "&nbsp;<span data-localize='trip_Hr'> hr &nbsp;</span>" + span.Minutes + "&nbsp;<span data-localize='trip_Mins'> mins &nbsp;</span>" + span.Seconds + "&nbsp;<span data-localize='trip_Secs'> secs</span>") + "</span></p>");
//                                    }
//                                }
//                                else
//                                {
//                                    sbtrip.Append("<p class='trip_text2'> " + " " + Stime + "<span class='tripSeconds'>:" + Sseconds + "</span>" + "&nbsp;<span data-localize='trip_To'>&nbsp; To &nbsp;</span>" + Etime + "<span class='tripSeconds'>:" + Eseconds + "&nbsp;</span>" + "<span style='float:right;'>" + UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, distance.ToString(), false, 1) + UserSettings.GetOdometerUnitName(objRegisteration.ifkMeasurementUnit) + "/" + (span.Minutes > 1 ? "" + span.Minutes + "&nbsp;<span data-localize='trip_Mins'> mins &nbsp;</span>" + span.Seconds + "&nbsp;<span data-localize='trip_Secs:'> secs" : "&nbsp;</span>" + span.Minutes + "&nbsp;<span data-localize='trip_Min'> min &nbsp;</span>" + span.Seconds + "&nbsp;<span data-localize='trip_Secs'> secs</span>") + "</span></p>");
//                                }

//                                sbtrip.Append("<p class='trip_text2'><span style='color:#175AB5;'>" + StartLocation + "</span>&nbsp;<span data-localize='trip_To'> to </span><span style='color:#175AB5;'>" + EndLocation + "</span></p>");
//                            }
//                            else
//                            {
//                                sbtrip.Append("<p class='trip_text2'> " + "<span data-localize='trip_CurrentTrip'> Current trip started at </span>" + Stime + "<span class='tripSeconds'>:" + Sseconds + "</span>" + "</p>");
//                                sbtrip.Append("<p class='trip_text2'><span style='color:#175AB5;'><span data-localize='trip_From'>From </span>" + StartLocation + "</span></p>");
//                            }

//                            sbtrip.Append("</div>");
//                            sbtrip.Append("</div>");
//                            tripReplaycount = tripReplaycount + 1;

//                            //if (_isInProgress == false)
//                            //{
//                            //    if (_drSelectedDevices[_intCount]["vReportID"].ToString() == "8") // || _drSelectedDevices[_intCount]["vReportID"].ToString() == "18") && _drSelectedDevices[_index]["vReportID"].ToString() == "7") //if ignition on is missing
//                            //    {
//                            //        _intCount += 2;
//                            //    }
//                            //    else
//                            //    {
//                            //        _intCount += 1;
//                            //    }
//                            //}
//                            //else
//                            //{
//                            //    _intCount += 1;
//                            //}

//                            _isFirstRecord = false;
//                        }

//                    }


//                }
//                catch (Exception ex)
//                {
//                    LogError.RegisterErrorInLogFile( "MyAccount.cs", "TripReplayEventInst()->trip device Event Inst", ex.Message  + ex.StackTrace);
//                }

//                listLeftMenu.Add(new clsTripReplay(tripReplaycount, sbtrip.ToString(), UserSettings.ConvertKMsToXx(objRegisteration.ifkMeasurementUnit, totalDistanceForDay.ToString(), false, 1) + " " + odometerUnit));
//            }
//            return listLeftMenu;
//        }



//    }
//}
