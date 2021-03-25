using System;
using System.Collections.Generic;
using System.Linq;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using System.Data;
using System.Globalization;

using WLT.BusinessLogic.Bal_GPSOL;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Widgets
    {
        public List<EL_Widgets> GetAllDrillDownInfo(EL_Widgets el_Widget, string TimeZoneId, int measurementUnit)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.GetODrillDownInfo(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets
                    {
                        AssetName = Convert.ToString(dr["AssetName"]),
                        AssetPhoto = Convert.ToString(dr["AssetPhoto"]),
                        DriverName = Convert.ToString(dr["DriverName"]),
                        DriverPhoto = Convert.ToString(dr["DriverPhoto"]),
                        DeviceID = Convert.ToInt32(dr["ifk_AssignedAssetId"]),
                        DriverID = Convert.ToInt32(dr["ifkDriverID"]),
                    });
                }


            }

            return list;
        }

        public List<EL_Widgets> GetFinalBreakDown(EL_Widgets el_Widget, string TimeZoneId, int measurementUnit)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.GetODrillDownInfo(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets
                    {
                        AssetName = Convert.ToString(dr["AssetName"]),
                        AssetPhoto = Convert.ToString(dr["AssetPhoto"]),
                        DriverName = Convert.ToString(dr["DriverName"]),
                        DeviceID = Convert.ToInt32(dr["ifk_AssignedAssetId"]),
                        vTextMessage = Convert.ToString(dr["vTextMessage"]),
                        DriverPhoto = Convert.ToString(dr["DriverPhoto"]),
                        Date = Convert.ToDateTime(dr["dGPSDateTime"]).ToString("ddd dd MM-yyyy HH:mm:"),
                        RoadSpeed = Convert.ToDouble(dr["vRoadSpeed"]),
                        VehicleSpeed = Convert.ToDouble(dr["vVehicleSpeed"]),
                        Excess = Convert.ToDouble(dr["Excess"]),
                    });
                }


            }

            return list;
        }
        
        public DataSet GetFuelPreliminaryParameters(int Operation)
        {

            var ds = new DataSet();

            var _DAL_Widgets = new DAL_Widgets();

            ds = _DAL_Widgets.GetFuelPreliminaryParameters(Operation);



            return ds;

        }
        
        public List<El_FuelWidgets> GetFleetSummary(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();

            double TotalFilled = 0.000;

            var DAL_Widgets = new DAL_Widgets();

            ds = DAL_Widgets.GetFleetSummary(el_Widget);


            List<El_FuelWidgets> list = new List<El_FuelWidgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    Byte[] logobytes = null;

                    string uploadedLogo = "";

                    if (dr["vLogo"] != DBNull.Value)
                    {
                        //logobytes = (Byte[])dr["vLogo"];

                        //string imageBase64 = Convert.ToBase64String(logobytes);

                        // uploadedLogo = string.Format("<img   class = 'serverImage'  src = data:image/gif;base64,{0} />", imageBase64);
                        var imgsrc = Convert.ToString(dr["vLogo"]);
                        uploadedLogo = string.Format("<img   class = 'serverImage'  src = '{0}' />", imgsrc);
                    }

                    list.Add(new El_FuelWidgets
                    {
                        AssetName = Convert.ToString(dr["AssetName"]),
                        // DriverName = Convert.ToString(dr["DriverName"]),
                        CurrentSpeed = UserSettings.ConvertKMsToXx(el_Widget.measurementUnit, Convert.ToString(dr["CurrentSpeed"]), true, 2),
                        TopSpeed = UserSettings.ConvertKMsToXx(el_Widget.measurementUnit, Convert.ToString(dr["t_max_speed"]), true, 2),
                        TodaysDistance = UserSettings.ConvertKMsToXxOdoMeter(el_Widget.measurementUnit, Convert.ToString(dr["TodaysDistance"]), true, 2),
                        MonthsDistance = UserSettings.ConvertKMsToXxOdoMeter(el_Widget.measurementUnit, Convert.ToString(dr["MonthsDistance"]), true, 2),
                        TotalDistance = UserSettings.ConvertKMsToXxOdoMeter(el_Widget.measurementUnit, Convert.ToString(dr["TotalDistance"]), true, 2),
                        Location = Convert.ToString(dr["Location"]),
                        vLogo = uploadedLogo
                    });

                }
            }


            return list;
        }

        public List<EL_Widgets> GetAllDistancesCovered(EL_Widgets el_Widget, string TimeZoneId, int measurementUnit)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetOverallDistancesForAllAssets(el_Widget);
            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // string DriverPhoto, string AssetName, int Distance, string vLogo

                //      modelOutPut.Add(new DistanceTravelledGraphOutputModel { Unit = MeasurementUnt, OverrallTotalDistance = TotalSum, MonthlyAvg = UnitCalculation(TotalSum, DeviceList.Count, datesTuple).Item1, VehicleAvg = UnitCalculation(TotalSum, DeviceList.Count, datesTuple).Item2 });

                var Month = el_Widget.MonthValue == 0 ? 1 : el_Widget.MonthValue;
                var totalDistance = Convert.ToDouble(ds.Tables[0].Compute("Sum(distance)", ""));
                var numbOfDays = el_Widget.EndDate.Subtract(el_Widget.StartTime);

                var DailyAvarage = Math.Round((totalDistance / numbOfDays.TotalDays), 2);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets
                    {
                        Unit = UserSettings.GetOdometerUnitName(measurementUnit),
                        OverrallTotalDistance = UserSettings.ConvertKMsToXxOdoMeter(measurementUnit, totalDistance.ToString(), true, 2),
                        TotalAssetDistance = UserSettings.ConvertKMsToXxOdoMeter(measurementUnit, Convert.ToString(dr["distance"]), false, 2),
                        DailyAvg = UserSettings.ConvertKMsToXxOdoMeter(measurementUnit, DailyAvarage.ToString(), true, 2),
                        VehicleAvg = Convert.ToDouble(UserSettings.ConvertKMsToXxOdoMeter(measurementUnit, (Convert.ToDouble(dr["distance"]) / Convert.ToDouble(dr["AssetCount"])).ToString(), false, 2)),
                        Date = Convert.ToDateTime(dr["Date"]).ToString("yyyy-MM-dd")


                    });

                }


            }
            return list.OrderBy(x => x.Date).ToList();
        }

        public List<EL_Widgets> GetMostTravelledAssets(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.GetMostTravelledAssets(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // string DriverPhoto, string AssetName, int Distance, string vLogo
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    var UserDefinedDistance = UserSettings.ConvertKMsToXxOdoMeter(el_Widget.measurementUnit, Convert.ToString(dr["Distance"]), true, 2);

                    list.Add(new EL_Widgets { AssetName = Convert.ToString(dr["AssetName"]), AssetPhoto = getUrlLogo(dr["AssetPhoto"]), DriverName = Convert.ToString(dr["DriverName"]), DriverPhoto = getUrlLogo(dr["DriverPhoto"]), Mileage = UserDefinedDistance });
                    //list.Add(new EL_Widgets { AssetName = dr["AssetName"].ToString(), Distance = Convert.ToDouble(dr["Odometer"]) });


                }

            }

            return list;
        }

        public DataTable GetTemperatureSupportingDevices(El_Report _El_Report)
        {
            var supportedDevices = DAL_Widgets.GetTemperatureSupportingDevices(_El_Report);

            return supportedDevices;
        }

        public List<EL_Widgets> DashboardTemperatureGraph(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            var _report = new El_Report();

            var _Bal_Temperature = new Bal_Temperature();

            var result = _Bal_Temperature.GetTemperatureReportInfo(_report);

            List<EL_Widgets> list = new List<EL_Widgets>();


            return list;
        }
        
        public List<EL_Widgets> GetZoneViolations(EL_Widgets el_Widget, string TimeZoneId)
        {

            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetZoneViolations(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //AssetName, Count(1) as [Count], DriverName, vLogo 
                    list.Add(new EL_Widgets { AssetName = Convert.ToString(dr["AssetName"]), AssetPhoto = getUrlLogo(dr["AssetPhoto"]), DriverName = Convert.ToString(dr["DriverName"]), DriverPhoto = getUrlLogo(dr["DriverPhoto"]), Count = Convert.ToInt32(dr["Count"]) });
                }

            }

            return list;
        }

        public List<EL_Widgets> GetNumberOfAlerts(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetNumberOfAlerts(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets(dr["DriverName"].ToString(), dr["AssetName"].ToString(), getUrlLogo(dr["vLogo"]), Convert.ToInt32(dr["Count"].ToString())));
                }

            }

            return list;
        }

        public List<EL_Widgets> GetTopSpeed(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetTopSpeed(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    list.Add(new EL_Widgets { MeasurementUnit = UserSettings.GetSpeedUnitName(el_Widget.measurementUnit) });


                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets { DriverName = dr["DriverName"].ToString(), AssetName = dr["AssetName"].ToString(), MaxSpeed = UserSettings.ConvertKMsToXx(el_Widget.measurementUnit, Convert.ToString(dr["MaxSpeed"]), false, 2), vLogo = getUrlLogo(dr["vLogo"]), AssetPhoto = getUrlLogo(dr["AssetPhoto"]) });
                }

            }

            return list;
        }

        public List<EL_Widgets> GetViolationSummary(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetViolationSummary(el_Widget);

            int TotalOverSpeeds = 0;
            int TotalAlerts = 0;
            int TotalHashBehaviours = 0;
            int ZoneViolation = 0;

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalOverSpeeds = Convert.ToInt32(ds.Tables[0].Rows[0]["Count"]);
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    TotalAlerts = Convert.ToInt32(ds.Tables[1].Rows[0]["Count"]);
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    TotalHashBehaviours = Convert.ToInt32(ds.Tables[2].Rows[0]["Count"]);
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    ZoneViolation = Convert.ToInt32(ds.Tables[3].Rows[0]["Count"]);
                }

            }

            list.Add(new EL_Widgets("Total OverSpeeds", TotalOverSpeeds));
            list.Add(new EL_Widgets("Total Alerts", TotalAlerts));
            list.Add(new EL_Widgets("Harsh Behaviour", TotalHashBehaviours));
            list.Add(new EL_Widgets("NOGO/Keep In", ZoneViolation));

            return list;
        }

        public List<EL_Widgets> GetHarshBehaviour_(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetHarshBehaviour(el_Widget);

            int TotalHarshAcceleration = 0;
            int TotalHarshBraking = 0;
            int TotalHashCornering = 0;
            int TotalOverspeed = 0;

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    TotalHarshAcceleration = Convert.ToInt32(ds.Tables[0].Rows[0]["Count"]);
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    TotalHarshBraking = Convert.ToInt32(ds.Tables[1].Rows[0]["Count"]);
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    TotalHashCornering = Convert.ToInt32(ds.Tables[2].Rows[0]["Count"]);
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    TotalOverspeed = Convert.ToInt32(ds.Tables[3].Rows[0]["Count"]);
                }

            }

            list.Add(new EL_Widgets("Harsh Acceleration", TotalHarshAcceleration));
            list.Add(new EL_Widgets("Harsh Braking", TotalHarshBraking));
            list.Add(new EL_Widgets("Harsh Cornering", TotalHashCornering));
            list.Add(new EL_Widgets("Overspeed", TotalOverspeed));

            return list;
        }
        
        public List<EL_Widgets> GetHarshBehaviour(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetHarshBehaviour(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new EL_Widgets { vTextMessage = Convert.ToString(row["vEventName"]), Count = Convert.ToInt32(row["Total"]) });

            }


            return list;
        }

        public List<EL_Widgets> GetCustomAlertCurfew(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetCustomAlertCurfew(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets { DriverName = dr["DriverName"].ToString(), AssetName = dr["AssetName"].ToString(), vLogo = getUrlLogo(dr["vLogo"]), Distance = Convert.ToDouble(dr["Odometer"].ToString()) });
                }

            }

            return list;
        }

        public List<EL_Widgets> GetTopHarshOffenders(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetTopHarshOffenders(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets(dr["DriverName"].ToString(), dr["AssetName"].ToString(), getUrlLogo(dr["vLogo"]), Convert.ToInt32(dr["Count"].ToString())));
                }

            }

            return list;
        }

        public List<EL_Widgets> GetTopSpeeders(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetTopSpeeders(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets
                    {
                        DriverID = Convert.ToInt32(dr["ipkDriverID"]),
                        DriverName = dr["DriverName"].ToString(),
                        DriverPhoto = getUrlLogo(dr["DriverPhoto"]),
                        AssetName = dr["AssetName"].ToString(),
                        AssetPhoto = getUrlLogo(dr["AssetPhoto"]),
                        DeviceID = Convert.ToInt32(dr["ifk_AssignedAssetId"]),
                        Count = Convert.ToInt32(dr["Total"].ToString())
                    });
                }

            }

            return list;
        }

        public List<EL_Widgets> GetTopTimeSpentOverspeeding(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.GetTopTimeOverSpeeders(el_Widget);


            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var time = Convert.ToInt32(dr["Total"].ToString());

                    TimeSpan _TimeSpan = TimeSpan.FromSeconds(time);

                    var stringTime = ConvertTimeSpanToString(_TimeSpan);

                    list.Add(new EL_Widgets { DriverID = Convert.ToInt32(dr["ifkDriverID"]), DriverName = dr["DriverName"].ToString(), DriverPhoto = getUrlLogo(dr["DriverPhoto"]), AssetName = dr["AssetName"].ToString(), AssetPhoto = getUrlLogo(dr["AssetPhoto"]), DeviceID = Convert.ToInt32(dr["ifk_AssignedAssetId"]), TimeString = stringTime });
                }

            }

            return list;
        }

        public double GetTotalDuration(long vpkDeviceID, EL_Widgets el_Widget, string TimeZoneId, double minutes)
        {
            var trips = new Trips();


            var type = 2;

            var dirtyTripSummary = trips.GetDirtyTripSummary(vpkDeviceID, new DateTime(el_Widget.StartTime.AddMinutes(minutes).Ticks, DateTimeKind.Unspecified), new DateTime(el_Widget.EndDate.AddMinutes(minutes).Ticks, DateTimeKind.Unspecified), TimeZoneId, type);

            var CleansummeryList = trips.CleanDirtyTripSummary(dirtyTripSummary, el_Widget.ClientID, vpkDeviceID, new DateTime(el_Widget.StartTime.AddMinutes(minutes).Ticks, DateTimeKind.Unspecified), new DateTime(el_Widget.EndDate.AddMinutes(minutes).Ticks, DateTimeKind.Unspecified), TimeZoneId, false);

            var totalDuration = 0.0;

            foreach (var item in CleansummeryList)
            {
                totalDuration += Math.Round(item.Duration.TotalSeconds, 0);
            }
            return totalDuration;
        }

        public EL_Widgets LoadDriverAssetsDetails(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            EL_Widgets Re_el_Widget = new EL_Widgets();

            ds = dal_Widget.LoadDriverAssetsDetails(el_Widget);

            Re_el_Widget.Table = ds.Tables[0];

            var TotalTimeElapsed = 0;
            var TotalDuration = 0.0;


            try
            {
                int i = 0;

                foreach (DataRow row in Re_el_Widget.Table.Rows)
                {
                    if (i == 0)
                    {
                        TotalDuration = GetTotalDuration(Convert.ToInt64(row["vpkDeviceID"]), el_Widget, TimeZoneId, dates.Item3);
                    }
                    i++;
                    var seconds = Convert.ToInt32(row["additionalEventInfo"]);

                    var timespan = System.TimeSpan.FromSeconds(seconds);
                    TotalTimeElapsed += seconds;
                    row["additionalEventInfo"] = ConvertTimeSpanToString(timespan);
                    row["dGPSDateTime"] = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["dGPSDateTime"]), TimeZoneId);

                }
            }
            catch (Exception ex)
            {
            }
            Re_el_Widget.TotalTimeInt = TotalTimeElapsed;

            Re_el_Widget.TotalTimeElapsed = ConvertTimeSpanToString(TimeSpan.FromSeconds(TotalTimeElapsed));

            Re_el_Widget.TotalDuration = TotalDuration;

            Re_el_Widget.TotalDurationString = ConvertTimeSpanToString(TimeSpan.FromSeconds(TotalDuration)); ;

            return Re_el_Widget;
        }

        public string ConvertTimeSpanToString(TimeSpan _TimeSpan)
        {
            string timestring = "";

            if (_TimeSpan.TotalDays > 1)
            {
                timestring = _TimeSpan.Days + " Days " + _TimeSpan.Hours + " hrs " + _TimeSpan.Minutes + " mins";

            }
            else
            {
                if (_TimeSpan.TotalHours > 1)
                {
                    timestring = _TimeSpan.Hours + " hrs " + _TimeSpan.Minutes + " mins " + _TimeSpan.Seconds + " secs";
                }


                if (_TimeSpan.TotalMinutes > 1 && _TimeSpan.TotalHours < 1)
                {
                    timestring = _TimeSpan.Minutes + " mins " + _TimeSpan.Seconds + " secs";
                }
                else if (_TimeSpan.TotalSeconds > 1 && _TimeSpan.TotalMinutes < 1)
                {
                    timestring = _TimeSpan.Seconds + " secs";
                }
                else if (_TimeSpan.TotalSeconds == 0)
                {
                    timestring = "0 secs";
                }

            }

            return timestring;
        }
       
        public List<EL_Widgets> GetTopIdlers(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetTopIdlers(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    //Byte[] logobytes = null;
                    //string uploadedLogo = "";
                    //if (dr["vLogo"] != DBNull.Value)
                    //{
                    //    logobytes = (Byte[])dr["vLogo"];
                    //    string imageBase64 = Convert.ToBase64String(logobytes);
                    //    uploadedLogo = string.Format("<img  src = data:image/gif;base64,{0} />", imageBase64);

                    //}


                    list.Add(new EL_Widgets(dr["DriverName"].ToString(), dr["AssetName"].ToString(), getUrlLogo(dr["vLogo"]), Convert.ToInt32(dr["Count"].ToString())));
                }

            }

            return list;
        }

        public List<EL_Widgets> GetMostViolatedZones(EL_Widgets el_Widget, string TimeZoneId)
        {

            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetMostViolatedZones(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    list.Add(new EL_Widgets { ZoneTypeId = dr["ZoneTypeId"].ToString(), vTextMessage = dr["vTextMessage"].ToString(), geofenceType = dr["vGeofenceType"].ToString(), Count = Convert.ToInt32(dr["Count"].ToString()) });
                }

            }

            return list;
        }

        public List<EL_Widgets> GetMostVisitedLocations(EL_Widgets el_Widget, string TimeZoneId)
        {
            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetMostVisitedLocations(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    list.Add(new EL_Widgets(dr["vTextMessage"].ToString(), Convert.ToInt32(dr["Count"].ToString())));
                }

            }

            return list;
        }

        public List<EL_Widgets> GetSpeficZoneVisits(EL_Widgets el_Widget, string TimeZoneId)
        {

            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetSpeficZoneVisits(el_Widget);

            List<EL_Widgets> list = new List<EL_Widgets>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    list.Add(new EL_Widgets(dr["DriverName"].ToString(), dr["AssetName"].ToString(), getUrlLogo(dr["vLogo"]), Convert.ToInt32(dr["Count"].ToString())));
                }

            }

            return list;
        }

        public List<EL_Dashboard_aob> FetchTotalViolations(EL_Widgets el_Widget, string TimeZoneId)
        {

            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;
            el_Widget.EndDate = dates.Item2;


            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            ds = dal_Widget.GetTotalViolations(el_Widget);

            List<EL_Dashboard_aob> list = new List<EL_Dashboard_aob>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int i = 1;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    list.Add(new EL_Dashboard_aob { code = i, ViolationType = dr["ViolationType"].ToString(), count = Convert.ToInt32(dr["Total"]), reportId = Convert.ToInt32(dr["id"]) });

                    i++;
                }

            }

            return list;
        }

        public Widget_Trip FetchTotalTrips(EL_Widgets el_Widget, string TimeZoneId)
        {

            #region                     

            //var timeo = calenderTimeHelper_(el_Widget, TimeZoneId,true);

            //el_Widget.StartTime = timeo.Item1;

            //el_Widget.EndDate = timeo.Item2;

            var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;

            DataSet ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.GetAssets(el_Widget);

            #endregion

            Trips trip = new Trips();

            var deviceIMElist = (from q in ds.Tables[0].AsEnumerable()
                                 select q["ImeiNumber"]);

            var deviceIMEcsv = String.Join(",", deviceIMElist.Select(x => x.ToString()).ToArray());

            var rawdata = trip.GetDirtyTripSummaryForMultipleAssets(deviceIMEcsv, el_Widget.StartTime, el_Widget.EndDate);

            //get distinct device list
            var deviceList = (from q in rawdata.Tables[0].AsEnumerable()
                              select q["vpkDeviceID"]).Distinct();

            var timelog = CreatePoints(el_Widget.StartTime.AddMinutes(dates.Item3), el_Widget.EndDate.AddMinutes(dates.Item3));

            var tripsList = new Widget_Trip();

            foreach (var item in timelog)
            {

                //  var dailyList = new List<Widget_Trip>();   
                var TotalTrips = 0;

                foreach (var assetID in deviceList)
                {

                    string filter = string.Format(CultureInfo.InvariantCulture, "dGPSDateTime >= #{0}# AND dGPSDateTime <= #{1}# AND vpkDeviceID ={2} ", item.Value.startDate, item.Value.endDate, assetID);

                    var filtered = rawdata.Tables[0].Select(filter);

                    if (filtered.Length > 0)
                    {
                        var dataTable = filtered.CopyToDataTable();

                        var dirtyDataset = new DataSet();

                        dirtyDataset.Tables.Add(dataTable);

                        var trips = trip.CleanDirtyTripSummary(dirtyDataset, el_Widget.ClientID, Convert.ToInt64(assetID), item.Value.startDate, item.Value.endDate, TimeZoneId, false);

                        var cleanTrips = trip.RemoveZeroDistanceFromCleanedTripSummary(0.01, trips, TimeZoneId);

                        TotalTrips += cleanTrips.Count;
                        // dailyList.Add(new Widget_Trip {No_of_Trips= cleanTrips.Count,DeviceID= assetID } );
                    }

                }
                tripsList.Data_List.Add(new Widget_Trip { specificDate = item.Value.startDate.AddMinutes(dates.Item3).ToString("ddd dd MMM"), No_of_Trips = TotalTrips });

            }

            tripsList.No_of_Assets = deviceList.Count();

            return tripsList;
        }

        public List<Widget_Maintenance> FetchMaintenanceItems(EL_Widgets el_Widget, string TimeZoneId)
        {
            var userOdo = UserSettings.ConvertXxToKms(el_Widget.measurementUnit, el_Widget.paramValue_M.ToString(), false, 2);

            el_Widget.paramValue_M = Convert.ToDouble(userOdo);

            var widget_Maintenance = new List<Widget_Maintenance>();

            // var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = DateTime.UtcNow;
            el_Widget.EndDate = DateTime.UtcNow;


            var ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.GetMaintenanceItems(el_Widget);

            foreach (DataRow item in ds.Tables[0].Rows)
            {

                widget_Maintenance.Add(new Widget_Maintenance
                {
                    Asset = item["Name"].ToString(),
                    MaintenanceItem = item["MaintenanceItemName"].ToString(),
                    Remaining_Distance = Convert.ToDouble(UserSettings.ConvertKMsToXxOdoMeterNegative(el_Widget.measurementUnit, Convert.ToString(item["Remaining"]), false, 2)),
                    Current_Odo = Convert.ToDouble(item["Odometer"]),
                    Set_ParameterValue = Convert.ToDouble(item["ParameterValue"]),
                    vlogo = getUrlLogo(item["vlogo"])
                });
            }

            return widget_Maintenance;


        }
        
        public List<Widget_Maintenance> FetchReminders(EL_Widgets el_Widget, string TimeZoneId)
        {

            //hellow
            var widget_Maintenance = new List<Widget_Maintenance>();

            // var dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = DateTime.UtcNow;

            el_Widget.EndDate = DateTime.UtcNow;

            var ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.GetRemindersItems(el_Widget);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                try
                {

                    widget_Maintenance.Add(new Widget_Maintenance
                    {
                        Asset = item["Name"].ToString(),
                        MaintenanceItem = item["MaintenanceItemName"].ToString(),
                        RemainingTime = Convert.ToInt32(item["remainder"]),
                        Set_ParameterValueDate = Convert.ToDateTime(item["ParameterValue"]),
                        vlogo = getUrlLogo(item["vlogo"])
                    });
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

            return widget_Maintenance;


        }
        
        public List<EL_Widgets> TotalMileageGraph(EL_Widgets el_Widget, string TimeZoneId)
        {


            var dates = calenderWeekHelper(el_Widget.WeekValue, TimeZoneId);

            el_Widget.MinutesDifference = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, TimeZoneId).Subtract(DateTime.UtcNow).TotalMinutes;

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;

            var ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.TotalMileageGraph(el_Widget);

            var dt = ds.Tables[0];

            List<EL_Widgets> info = new List<EL_Widgets>();


            foreach (DataRow row in dt.Rows)
            {
                var userMileage = UserSettings.ConvertKMsToXxOdoMeter(el_Widget.measurementUnit, row["mileage"].ToString(), false, 2);
                info.Add(new EL_Widgets { AssetName = row["Asset"].ToString(), Distance = Convert.ToDouble(userMileage), StartTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(row["date"]), TimeZoneId), Id = Convert.ToInt32(row["id"]) });
            }



            return info;


        }
        
        public DataTable DailyViolationsSummary(EL_Widgets el_Widget, string TimeZoneId)
        {


            var dates = calenderWeekHelper(el_Widget.WeekValue, TimeZoneId);

            el_Widget.StartTime = dates.Item1;

            el_Widget.EndDate = dates.Item2;

            var ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            ds = dal_Widget.DailyViolationsSummery(el_Widget);

            var dt = new DataTable();

            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;


        }



        public static Tuple<DateTime, DateTime, double> AssetInfoCalenderWeekHelper(int i, string TimeZoneId)
        {    //beginning Date instantiation
            var dStartDate = new DateTime();

            //get current utc datetime 
            var UtcNow = DateTime.UtcNow;

            //get current user local  time 


            DateTime userTimeNow = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(UtcNow, TimeZoneId);

            var timeDifference = userTimeNow.Subtract(UtcNow);

            //Create A starting Date for that month


            switch (i)
            {
                case 0:
                    //today 
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date, TimeZoneId);

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;

                case 1:
                    //yesterday 
                    var yesterdayEnd = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date, TimeZoneId).AddSeconds(-1);

                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(yesterdayEnd.Date, TimeZoneId); 

                    UtcNow = yesterdayEnd;
                    break;
                case 7:
                    //last seven days
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-6).Date, TimeZoneId);

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                                  
                case 2:
                    //previuos week

                    var lastWeeksSunday = userTimeNow.GetFirstDayOfThisWeek().Date.AddSeconds(-1);

                    var lastWeeksMonday = lastWeeksSunday.GetFirstDayOfThisWeek().Date;

                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(lastWeeksMonday, TimeZoneId);

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(lastWeeksSunday, TimeZoneId);

                    break;
                case 14:
                    //last 2 weeks
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-13).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                case 30:
                    // last 30 days 
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-29).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                case 5:
                    //current month
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, 01), TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                case 6:
                    //previous month

                    var lastMonth = (new DateTime(userTimeNow.Year, userTimeNow.Month, 01)).AddSeconds(-1);

                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(lastMonth.Year, lastMonth.Month, 1), TimeZoneId);
                    UtcNow = lastMonth;

                    break;

                default:
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-7), TimeZoneId);
                    break;



            }

            return new Tuple<DateTime, DateTime, double>(dStartDate, UtcNow, timeDifference.TotalMinutes);

        }





        public static Tuple<DateTime, DateTime> calenderTimeHelper(int i, string TimeZoneId)
        {    //beginning Date instantiation
            var dStartDate = new DateTime();

            //get current utc datetime 
            var UtcNow = DateTime.UtcNow;

            //get current user local  time 
            DateTime userTimeNow = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(UtcNow, TimeZoneId);

            //stretch user time to the last of the day 
            userTimeNow = new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59);

            //converting back user time to utc
            UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow, TimeZoneId);


            //Create A starting Date for that month
            if (i == 1 || i == 0)
            {
                i = 1;
                //Default to the First day 1st


                dStartDate = new DateTime(UtcNow.Year, UtcNow.Month, i);
            }
            else
            {
                // dStartDate = UtcNow.AddMonths(-(i - 1));

                dStartDate = UtcNow.AddMonths(-i);
                //dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, 1);
            }

            return new Tuple<DateTime, DateTime>(dStartDate, UtcNow);

        }

        public static Tuple<DateTime, DateTime, double> calenderTimeHelper_(EL_Widgets o, string TimeZoneId, bool parseToUtc)
        {

            var i = 0;

            if (o.paramID == "week")
            {
                i = o.WeekValue * 7;
                if (i == 0) i = 7;

            }
            else if (o.paramID == "day")
            {
                i = o.Dayvalue;
                if (i == 0) i = 1;
            }
            else
            {
                i = o.MonthValue;
                if (i == 0) i = 30;
            }
            var dStartDate = new DateTime();
            //Get users Today date
            var tempDate = DateTime.UtcNow;

            DateTime CurrentDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(tempDate, TimeZoneId);

            var diff = CurrentDate.Subtract(tempDate);

            CurrentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 23, 59, 59);

            dStartDate = CurrentDate.AddHours(-(24 * i));

            dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, dStartDate.Day);
            //
            var rtnstmt = new Tuple<DateTime, DateTime, double>(dStartDate, CurrentDate, diff.TotalMinutes);

            if (parseToUtc) rtnstmt = new Tuple<DateTime, DateTime, double>(dStartDate.Add(diff), CurrentDate.Add(diff), diff.TotalMinutes);

            //
            return rtnstmt;

        }

        public static Tuple<DateTime, DateTime, double> calenderWeekHelper(int i, string TimeZoneId)
        {    //beginning Date instantiation
            var dStartDate = new DateTime();

            //get current utc datetime 
            var UtcNow = DateTime.UtcNow;

            //get current user local  time 


            DateTime userTimeNow = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(UtcNow, TimeZoneId);

            var timeDifference = userTimeNow.Subtract(UtcNow);

            //Create A starting Date for that month


            switch (i)
            {
                case -2:
                    //today 
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date, TimeZoneId);

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;

                case -1:
                    //yesterday 
                    var yesterdayEnd = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date, TimeZoneId).AddSeconds(-1);

                    dStartDate = yesterdayEnd.Date;

                    UtcNow = yesterdayEnd;
                    break;
                case 0:
                    //last seven days
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-6).Date, TimeZoneId);

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;

                case 1:
                    //current week
                    var monday = userTimeNow.GetFirstDayOfThisWeek().Date;

                    DateTime utcMonday = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(monday, TimeZoneId);

                    dStartDate = utcMonday;

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);

                    break;
                case 2:
                    //previuos week

                    var lastWeeksSunday = userTimeNow.GetFirstDayOfThisWeek().Date.AddSeconds(-1);

                    var lastWeeksMonday = lastWeeksSunday.GetFirstDayOfThisWeek().Date;

                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(lastWeeksMonday, TimeZoneId);

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(lastWeeksSunday, TimeZoneId);

                    break;
                case 3:
                    //last 2 weeks
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-13).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                case 4:
                    // last 30 days 
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-29).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                case 5:
                    //current month
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, 01), TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                case 6:
                    //previous month

                    var lastMonth = (new DateTime(userTimeNow.Year, userTimeNow.Month, 01)).AddSeconds(-1);

                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(lastMonth.Year, lastMonth.Month, 1), TimeZoneId);
                    UtcNow = lastMonth;

                    break;

                default:
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-7), TimeZoneId);
                    break;



            }

            return new Tuple<DateTime, DateTime, double>(dStartDate, UtcNow, timeDifference.TotalMinutes);

        }

        //public static Tuple <object,object> ThreeOptionEvaluator(EL_Widgets _EL_Widgets)
        //{

        //    if(_EL_Widgets.iParamValue_D ==1)
        //    {

        //        return new Tuple<object, object>();







        //    }
        //    if (_EL_Widgets.iParamValue_D == 2)
        //    {
        //        return new Tuple<object, object>();
        //    }
        //    else
        //    {

        //           return new Tuple<object, object>();

        //    }




        //}
        public void Timehelper(EL_Widgets o, string TimeZoneId)
        {    //beginning Date instantiation

            var i = 0;

            if (o.paramID == "week")
            {
                i = o.WeekValue;
                if (i == 0) i = 1;

            }
            else if (o.paramID == "day")
            {
                i = o.Dayvalue;
                if (i == 0) i = 1;
            }
            else
            {
                i = o.MonthValue;
                if (i == 0) i = 30;
            }


            o.iParamValue_D = i;



        }
        
        public string getBase64Logo(object logo)
        {

            Byte[] logobytes = null;
            string uploadedLogo = "";
            if (logo != DBNull.Value)
            {
                logobytes = (Byte[])logo;
                string imageBase64 = Convert.ToBase64String(logobytes);
                uploadedLogo = string.Format("<img   class = 'serverImage' src = data:image/gif;base64,{0} />", imageBase64);

            }

            return uploadedLogo;
        }
       
        public string getUrlLogo(object logo)
        {
            string uploadedLogo = "";

            if (logo != DBNull.Value)
            {
                var imageUrl = Convert.ToString(logo);

                uploadedLogo = string.Format("<img   class = 'serverImage' src = '{0}' />", imageUrl);

            }

            return uploadedLogo;
        }
        
        public List<EL_Widgets> getUserData(int unitCode)
        {
            List<EL_Widgets> list = new List<EL_Widgets>();

            list.Add(new EL_Widgets { Unit = UserSettings.GetOdometerUnitName(unitCode), SpeedUnit = UserSettings.GetSpeedUnitName(unitCode) });
            return list;
        }

        private static Dictionary<int, Widget_Trip> CreatePoints(DateTime dt1, DateTime dt2)
        {

            var timespan = dt2.Subtract(dt1);

            var log = new Dictionary<int, Widget_Trip>();

            int i = 0;

            while (dt1 < dt2)
            {
                var t = new DateTime(dt1.Year, dt1.Month, dt1.Day, 23, 59, 59);
                if (!(dt1.AddDays(1) >= dt2))
                {
                    log.Add(i, new Widget_Trip { startDate = new DateTime(dt1.Ticks, DateTimeKind.Unspecified), endDate = t });
                }
                else
                {
                    var diff = (dt2 - dt1).TotalMinutes;
                    log.Add(i, new Widget_Trip { startDate = new DateTime(t.Date.Ticks, DateTimeKind.Unspecified), endDate = t });
                }

                i++;

                dt1 = dt1.AddDays(1);

            }


            return log;
        }

        public El_Eco_Drive_Model Get_Eco_Driving_Score(EL_Widgets el_Widget, string TimeZoneId)
        {


            var _dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

            el_Widget.StartTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(_dates.Item1, TimeZoneId);

            //  var  _endDate = DateTime.SpecifyKind(_dates.Item2, DateTimeKind.Unspecified);

            el_Widget.EndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(_dates.Item2, TimeZoneId);

            var _Bal_Eco_Drive = new Bal_Eco_Drive();

            _Bal_Eco_Drive.TimezoneID = TimeZoneId;

            _Bal_Eco_Drive.userID = el_Widget.ifkUserID;

            _Bal_Eco_Drive.startTime = el_Widget.StartTime;

            _Bal_Eco_Drive.endTime = el_Widget.EndDate;

            var _O = _Bal_Eco_Drive.Getdrivingscore(el_Widget.ClientID);

            return _O;
        }

        public List<El_Heatmap> Get_Heatmap(EL_Widgets el_Widget, string TimeZoneId)
        {

            DataSet ds = new DataSet();
            DAL_Widgets dal_Widget = new DAL_Widgets();
            var heatmap = new List<El_Heatmap>();

            try
            {
                var _dates = calenderWeekHelper(el_Widget.MonthValue, TimeZoneId);

                el_Widget.StartTime = _dates.Item1;
                el_Widget.EndDate = _dates.Item2;

                ds = dal_Widget.Get_HeatmapData(el_Widget);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (Convert.ToBoolean(row["bIsIgnitionOn"]))
                        {
                            if (!string.IsNullOrWhiteSpace(Convert.ToString(row["vLatitude"]))
                                && !string.IsNullOrWhiteSpace(Convert.ToString(row["vLongitude"])))                            
                                heatmap.Add(new El_Heatmap
                                {

                                    Lat = row["vLatitude"].ToString(),
                                    Lon = row["vLongitude"].ToString(),
                                    Count =10

                                });
                            

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Widget.cs", "Get_Heatmap()", ex.Message + " : " + ex.StackTrace);
            }

            return heatmap;

        }

        public List<Route> Getdeviceroute(EL_Widgets el_Widget, string TimezoneID)
        {
            DataSet ds = new DataSet();

            DAL_Widgets dal_Widget = new DAL_Widgets();

            el_Widget.StartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(el_Widget.StartTime, TimezoneID);

            el_Widget.EndDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(el_Widget.EndDate, TimezoneID);

            ds = dal_Widget.GetDeviceRoute(el_Widget);

            var _routePoints = new List<Route>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                _routePoints.Add(new Route
                {
                    Location = Convert.ToString(row["vTextMessage"]),
                    Latitude = Convert.ToDouble(row["vLatitude"]),
                    Longitude = Convert.ToDouble(row["vLongitude"]),
                    Speed = Convert.ToDouble(row["vVehicleSpeed"]),
                    RoadSpeed = Convert.ToDouble(row["vRoadSpeed"]),
                    vpkDeviceID = Convert.ToInt64(row["vpkDeviceID"]),

                });
            }

            return _routePoints;

        }

    }
    public static class DateTimeExtension
    {
        public static DateTime GetFirstDayOfThisWeek(this DateTime d)
        {
            CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            var first = (int)ci.DateTimeFormat.FirstDayOfWeek;
            var current = (int)d.DayOfWeek;

            var result = first <= current ?
              d.AddDays(-1 * (current - first)) :
              d.AddDays(first - current - 7);

            return result;
        }
    }

}
