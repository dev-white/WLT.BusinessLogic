using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;

namespace WLT.BusinessLogic
{
    public class AssetInfo
    {
        public string GetCurrentStats(int AssetID)
        {
            DataSet ds = new DataSet();
            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.GetCurrentStats(AssetID);

            DataTable dt = new DataTable();
            DataTable _TripStart = new DataTable();
            DataTable _TripCur = new DataTable();
            DataTable _TripStop = new DataTable();

            if (ds.Tables.Count > 0)
            {
                _TripStart = ds.Tables[0].Copy();
                _TripCur = ds.Tables[1].Copy();
                _TripStop = ds.Tables[2].Copy();
            }

            var data = new
            {
                TripStart = _TripStart,
                TripCur = _TripCur,
                TripStop = _TripStop
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);


        }

        public string ShowIOCurrentStatus(int AssetID)
        {
            DataSet ds = new DataSet();
            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.ShowIOCurrentStatus(AssetID);

            DataTable _AnalogInput = new DataTable();
            DataTable _DigitalIO = new DataTable();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                _AnalogInput = ds.Tables[0].Copy();

                _AnalogInput.Columns.Add("Value", typeof(System.Double));

                foreach (DataRow dr in _AnalogInput.Rows)
                {

                    if(Convert.ToString(dr["SensorData"]) != "")
                    {
                        var sensorObj = JsonConvert.DeserializeObject<EL_AnalogSensorData>(dr["SensorData"].ToString());

                        PropertyInfo prop = sensorObj.GetType().GetProperty("An" + dr["AnType"]);

                        dr["Value"] = prop.GetValue(sensorObj, null) == null ? 0 : Convert.ToDouble(prop.GetValue(sensorObj, null));
                    }
                    else
                    {
                        dr["Value"] = 0;
                    }
                }
            }

            if (ds.Tables.Count > 0)
            {
                //_AnalogInput = ds.Tables[0].Copy();
                _DigitalIO = ds.Tables[1].Copy();
            }

            var data = new
            {
                AnalogInput = _AnalogInput,
                DigitalIO = _DigitalIO
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);

        }

        public string GetDigitalOutputCmds(int AssetID, int ifk_CommonEventId, int ifk_CommonEventId_Opposite, int operation)
        {
            DataSet ds = new DataSet();
            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.GetDigitalOutputCmds(AssetID, ifk_CommonEventId, ifk_CommonEventId_Opposite, operation);

            DataTable _DigitalOutputCmds = new DataTable();

            if (ds.Tables.Count > 0)
            {
                _DigitalOutputCmds = ds.Tables[0].Copy();
            }

            var data = new
            {
                DigitalOutputCmds = _DigitalOutputCmds,

            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);

        }

        public string ShowTodaysOverview(int AssetID, DateTime SelectedDate, string TimeZoneID)
        {
            DateTime UserLocalTime = Convert.ToDateTime(SelectedDate.ToString("yyyy-MM-dd"));

            DateTime StartDate = Convert.ToDateTime(UserSettings.ConvertLocalDateTimeToUTCDateTime(UserLocalTime, TimeZoneID));
            DateTime EndDate = StartDate.AddHours(24).AddSeconds(-1);

            DataSet ds = new DataSet();
            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.ShowTodaysOverview(AssetID, StartDate, EndDate);

            double _Distance = 0;
            int _Alerts = 0;
            int _Violations = 0;
            string _IdleTime = "0 mins";

            if (ds.Tables.Count > 0)
            {

                var _row_count = ds.Tables[0].Rows.Count;

                if (_row_count > 0)
                {
                    ///Total Distance

                    var _vOdometers = ds.Tables[0].AsEnumerable().Where(row => Convert.ToString(row["vOdometer"]) != string.Empty && Convert.ToDouble(row["vOdometer"]) > 0);


                    if (_vOdometers.Count() > 0)
                    {
                        var __max = _vOdometers.Max(row => Convert.ToDouble(row["vOdometer"]));

                        var __min = _vOdometers.Min(row => Convert.ToDouble(row["vOdometer"]));

                        _Distance = __max - __min;
                    }
                    else
                    {
                        _Distance = 0;
                    }


                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    //Total Alerts
                    _Alerts = Convert.ToInt32(ds.Tables[1].Rows[0]["AlertsCount"]);
                }

                if (ds.Tables[2].Rows.Count > 0)
                {
                    //Total Violations
                    _Violations = Convert.ToInt32(ds.Tables[2].Rows[0]["ViolationCounts"]);
                }

                if (ds.Tables[3].Rows.Count > 0)
                {
                    //Total Idle Time
                    _IdleTime = ds.Tables[3].Rows[0]["IdleTime"].ToString() + " mins";
                }

            }

            var data = new
            {
                Distance = _Distance,
                Alerts = _Alerts,
                Violations = _Violations,
                IdleTime = _IdleTime
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);

        }



        public string ShowTodayOBDData(int AssetID)
        {
            string record = "";

            DateTime? dGPSDateTime = null;

          DataSet ds = new DataSet();

            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.ShowTodayOBDData(AssetID);
                    

            //Organize data
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["ObdData"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["ObdData"]) != "{}")
                {
                    record = Convert.ToString(ds.Tables[0].Rows[0]["ObdData"]);

                    dGPSDateTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["dGPSDateTime"]);
                }

            }


            var data = new
            {
                OBD = record,

                dGPSDateTime
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);

        }

        public string ShowTodayOBDData_(int AssetID)
        {

            DataSet ds = new DataSet();

            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.ShowTodayOBDData(AssetID);

            DataTable _OBD = new DataTable();

            _OBD.Columns.Add("RPM", typeof(string));
            _OBD.Columns.Add("CTemp", typeof(string));
            _OBD.Columns.Add("ETemp", typeof(string));
            _OBD.Columns.Add("FuelL", typeof(string));
            _OBD.Columns.Add("FuelT", typeof(string));
            _OBD.Columns.Add("AirFlw", typeof(string));
            _OBD.Columns.Add("IaT", typeof(string));
            _OBD.Columns.Add("TPos", typeof(string));
            _OBD.Columns.Add("EngHrs", typeof(string));
            _OBD.Columns.Add("MStatus", typeof(string));
            _OBD.Columns.Add("MILDistance", typeof(string));
            _OBD.Columns.Add("EngLoad", typeof(string));
            _OBD.Columns.Add("dGPSDateTime", typeof(DateTime));

            DAL_OBD dAL_OBD = new DAL_OBD();

            //Organize data
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["ObdData"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["ObdData"]) != "{}")
                {
                    EL_OBDData eL_OBD = JsonConvert.DeserializeObject<EL_OBDData>(Convert.ToString(ds.Tables[0].Rows[0]["ObdData"]));

                    _OBD.Rows.Add(
                           eL_OBD.RPM ?? "-",                          
                           eL_OBD.CTemp ?? "-",
                           eL_OBD.EngOilTemperature ?? "-",
                           eL_OBD.FuelL ?? "-",
                           eL_OBD.FuelT ?? "-",
                           eL_OBD.AirFlw ?? "-",
                           eL_OBD.IaT ?? "-",
                           eL_OBD.TPos ?? "-",
                           eL_OBD.EngHrs ?? "-",
                           eL_OBD.MStatus ?? "-",
                           eL_OBD.MILDistance ?? "-",
                           eL_OBD.EngLoad ?? "-",
                           Convert.ToDateTime(ds.Tables[0].Rows[0]["dGPSDateTime"])
                       );
                }

            }


            var data = new
            {
                OBD = _OBD
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);

        }

        public string ShowOBDDataForGraph(int Operation, int AssetID, string obdType, int ShowDataFor, string startTime, string endTime, int UserID,string TimeZoneID)
        {

            DataSet ds = new DataSet();

            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.ShowOBDDataForGraph(Operation, AssetID, ShowDataFor, startTime, endTime, UserID);

            DataTable _OBD = new DataTable();

            _OBD.Columns.Add(obdType, typeof(double));

            _OBD.Columns.Add("ignitionstatus", typeof(bool));

            _OBD.Columns.Add("speed", typeof(double));

            _OBD.Columns.Add("dGPSDateTime", typeof(DateTime));

            _OBD.Columns.Add("odo", typeof(double));

            DAL_OBD dAL_OBD = new DAL_OBD();

            EL_OBDData eL_OBD = new EL_OBDData();

            //Organize data
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if(eL_OBD.GetType().GetProperty(obdType) is  { })
                foreach (DataRow dr in ds.Tables[0].Rows)
                {                   

                        if (Convert.ToString(dr["ObdData"]) != "" && Convert.ToString(dr["ObdData"]) != "{}")
                        {
                            eL_OBD = JsonConvert.DeserializeObject<EL_OBDData>(Convert.ToString(dr["ObdData"]));

                            string _propertyStrValue = eL_OBD.GetType().GetProperty(obdType).GetValue(eL_OBD, null).ToString();

                            var _val = "0.0";

                            if ("CTemp" == obdType)
                                _val = _propertyStrValue.Split('&')[0] ?? "0.0";
                            else
                                _val = _propertyStrValue.Split(' ')[0] ?? "0.0";


                            _OBD.Rows.Add
                                (
                                      _val,
                                      Convert.ToBoolean(dr["ignitionStatus"]),
                                       Convert.ToDouble(dr["speed"]),
                                       UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["dGPSDateTime"]), TimeZoneID),
                                         Convert.ToDouble(dr["odo"])
                                   );

                        }
                }
            }


            var data = new
            {
                OBD = _OBD
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);

        }


     
        public string GetViolationDetails(int AssetID, DateTime SelectedDate, string TimeZoneID)
        {
            DateTime UserLocalTime = Convert.ToDateTime(SelectedDate.ToString("yyyy-MM-dd"));

            DateTime StartDate = Convert.ToDateTime(UserSettings.ConvertLocalDateTimeToUTCDateTime(UserLocalTime, TimeZoneID));
            DateTime EndDate = StartDate.AddHours(24).AddSeconds(-1);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DAL_AssetInfo DAL_AssetInfo = new DAL_AssetInfo();

            ds = DAL_AssetInfo.GetViolationDetails(AssetID, StartDate, EndDate);



            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0].Copy();

            }

            var data = new
            {
                Violations = dt
            };

            return JsonConvert.SerializeObject(data, Formatting.Indented);

        }
    }


}
