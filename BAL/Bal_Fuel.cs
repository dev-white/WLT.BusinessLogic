using MathNet.Numerics.Statistics;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WLT.BusinessLogic.Bal_Reports;
using WLT.DataAccessLayer;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Fuel
    {
       static string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public double MaxFuelUsed { get; set; }
        public double MinFuelUsed { get; set; }

        List<IgnitionChunk> TripsLogs { get; set; }
        public Queue<double> ReadingsUnitsAveraged { get; set; }
        public Bal_Fuel()
        {
            MinFuelUsed = 10000; //just to start off           
        }

        public El_FuelDataSource GetFuelData(EL_Fuel source_EL_Fuel)
        {

            EL_Fuel eL_Fuel =  (EL_Fuel) source_EL_Fuel.Clone();

            var _El_FuelDataSource = new El_FuelDataSource();

            int sensorType = eL_Fuel.sensorType;

            int iAnalogNumber = eL_Fuel.iAnalogNumber;

            int canBusType = eL_Fuel.canBusType;

            DataTable Fuel = new DataTable();

            Fuel.Columns.Add("date", typeof(DateTime));
            Fuel.Columns.Add("id", typeof(int));
            Fuel.Columns.Add("odo", typeof(string));
            Fuel.Columns.Add("RawFuel", typeof(double));
            Fuel.Columns.Add("ign", typeof(bool));
            Fuel.Columns.Add("spid", typeof(double));
            Fuel.Columns.Add("rpm", typeof(double));
            Fuel.Columns.Add("vTextMessage", typeof(string));
            Fuel.Columns.Add("ipkCommanTrackingID", typeof(long));
            Fuel.Columns.Add("EngineHours", typeof(string));

            Fuel.Columns.Add("Lat", typeof(double));
            Fuel.Columns.Add("Lon", typeof(double));

            eL_Fuel.startDate = UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(eL_Fuel.startDate), eL_Fuel.TimeZoneID);

            eL_Fuel.endDate = UserSettings.ConvertLocalDateTimeToUTCDateTime(Convert.ToDateTime(eL_Fuel.endDate), eL_Fuel.TimeZoneID);

            var _DAL_Fuel = new DAL_Fuel();

            var ds = _DAL_Fuel.GetAnalogSensorData(eL_Fuel);

       

            try
            {

                var allRowsCount = ds.Tables[0].Rows.Count;
                //Organize data
                for (int i = 0; i < allRowsCount; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    EL_AnalogSensorData elAnalogSensor;

                    EL_OBDData canBus;

                    double? FuelValue = 0.0;

                    double? RPM = 0.0;

                    if (sensorType != 7 && Convert.ToString(dr["SensorData"]) != "")
                    {
                        elAnalogSensor = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(dr["SensorData"]));

                        if (sensorType == 1)
                        {       
                            if (iAnalogNumber == 1)
                                FuelValue = elAnalogSensor.An1;


                            if (iAnalogNumber == 2)
                                FuelValue = elAnalogSensor.An2;

                            if (iAnalogNumber == 3)
                                FuelValue = elAnalogSensor.An3;


                            if (iAnalogNumber == 4)
                                FuelValue = elAnalogSensor.An4;

                            if (iAnalogNumber == 5)
                                FuelValue = elAnalogSensor.An5;

                            if (iAnalogNumber == 6)
                                FuelValue = elAnalogSensor.An6;
                        }

                        if (sensorType == 2)
                        {
                            FuelValue = iAnalogNumber == 1 ? elAnalogSensor.S1 : elAnalogSensor.S2;
                        }
                        if (sensorType == 3)
                        {
                            //FuelValue = elAnalogSensor.OneW;
                        }
                    }
                    else if (sensorType == 7 && Convert.ToString(dr["ObdData"]) != "")
                    {
                        canBus = JsonConvert.DeserializeObject<EL_OBDData>(Convert.ToString(dr["ObdData"]));

                        if ((canBusType == 1 || canBusType == 4) &&
                            !string.IsNullOrEmpty(canBus.FuelL))
                        {
                            FuelValue = Convert.ToDouble(canBus.FuelL.Split(' ')[0]);
                            RPM = 0;//Convert.ToDouble(canBus.RPM.Split(' ')[0]);
                        }
                        else
                        {
                            var NullMessaage = "";
                        }


                    }


                

                    /// removes zeros from the graph 
                    if (Convert.ToString(FuelValue) != "" && FuelValue > 0)
                    {
                        var location = Convert.ToString(dr["vTextMessage"]);

                        Fuel.Rows.Add(
                             UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["dGPSDateTime"]), eL_Fuel.TimeZoneID),
                            Convert.ToInt32(dr["vReportID"]),
                            dr["vOdometer"],
                            FuelValue,
                             Convert.ToBoolean(dr["ignitionStatus"]),
                            Convert.ToDouble(dr["speed"]),
                            RPM,
                            Convert.ToString(dr["vTextMessage"]),
                            Convert.ToInt64(dr["ipkCommanTrackingID"]),
                            dr["EngineHours"],
                            dr["vLatitude"],
                            dr["vLongitude"]
                        );


                    }



                    if (long.TryParse(Convert.ToString(dr["EngineHours"]), out _))
                        _El_FuelDataSource.FinalEngineHours = Convert.ToInt64(dr["EngineHours"]);

                    if (_El_FuelDataSource.InitialEngineHours < 0 && _El_FuelDataSource.FinalEngineHours >= 0)
                        _El_FuelDataSource.InitialEngineHours = _El_FuelDataSource.FinalEngineHours;


                    if (double.TryParse(Convert.ToString(dr["vOdometer"]), out _))
                        _El_FuelDataSource.FinalOdometer = Convert.ToDouble(dr["vOdometer"]);


                    if (_El_FuelDataSource.InitiaOdometer < 0.0 && _El_FuelDataSource.FinalOdometer >= 0.0)
                        _El_FuelDataSource.InitiaOdometer = _El_FuelDataSource.FinalOdometer;

                }


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Fuel.cs", "GetFuelData()", ex.Message  + ex.StackTrace);

            }


            _El_FuelDataSource.dtSource = Fuel;            

            return _El_FuelDataSource.Summarize();
        }

        public string GetCoordianates(EL_Fuel eL_Fuel)
        {
            var _coodinates = string.Empty;

            var coorniates = new object();

            var ds = new DAL_Fuel().GetFuelEventCoodinates(eL_Fuel);

            foreach (DataTable dt in ds.Tables)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    coorniates = new
                    {
                        Lat = Convert.ToString(dr["vLatitude"]),
                        Lng = Convert.ToString(dr["vLongitude"]),
                    };

                    _coodinates = JsonConvert.SerializeObject(coorniates);
                }

            }


            return _coodinates;
        }

        private double GetPreviousfuelReading(DataTable Parenttbl, DateTime currentDate, int prev_index, int current_index)
        {
            int rowCount = Parenttbl.Rows.Count;

            DataRow rowItem;

            var isZeroMark = false;

            if (prev_index > 0)

                rowItem = Parenttbl.Rows[prev_index];
            else
            {
                rowItem = Parenttbl.Rows[0];

                isZeroMark = true;
            }

            DateTime itemDate = Convert.ToDateTime(rowItem["dGPSDateTime"]);

            double itemValue = Convert.ToDouble(rowItem["Fuel"]);


            if (!isZeroMark && (itemDate == currentDate || itemValue == Convert.ToDouble(Parenttbl.Rows[current_index]["Fuel"])))
            {
                return GetPreviousfuelReading(Parenttbl, currentDate, prev_index - 1, current_index);
            }
            else
            {
                return itemValue;
            }
        }

        public double ConvertCurrentValueToAveragedValue(FuelQueueHelper _Helper)
        {
            //set the threshold for averaging
            double averagedUnits = 0.0;

            if (_Helper.ReadingsUnitsAveraged.Count > _Helper.AvgCount)
            {
                //remove data from Q so its always at 5
                _Helper.ReadingsUnitsAveraged.Dequeue();
            }

            foreach (double item in _Helper.ReadingsUnitsAveraged)
            {
                averagedUnits = averagedUnits + item;
            }

            // double newAveragedUnitValue = ((CurrentPointValue + averagedUnits) / (ReadingsUnitsAveraged + 1));

            double newAveragedUnitValue = Convert.ToDouble((_Helper.CurrentPointValue + averagedUnits) / (_Helper.ReadingsUnitsAveraged.Count + 1));

            _Helper.ReadingsUnitsAveraged.Enqueue(newAveragedUnitValue);


            // this is a hack for now where dont have 812/813
            if (Math.Sqrt(Math.Pow(newAveragedUnitValue - _Helper.CurrentPointValue, 2)) > 10)
            {
                newAveragedUnitValue = _Helper.CurrentPointValue;
                _Helper.ReadingsUnitsAveraged.Clear();
            }

            //return new point which has been altered as an averaged value.
            return newAveragedUnitValue;

        }


        public DataSet GetConnectedAnalogDevice(EL_Fuel eL_Fuel)
        {

            DataSet ds = new DataSet();

            try
            {
                DAL_Fuel _Fuel = new DAL_Fuel();

                ds = _Fuel.GetConnectedAnalogDevice(eL_Fuel);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Fuel.cs", "GetConnectedAnalogDevice()", ex.Message  + ex.StackTrace);
            }

            return ds;
        }

        public EL_AnalogMapping GetConnectedAnalogMappings( EL_Fuel eL_Fuel)
        {

          
                Bal_Fuel Bal_Fuel = new Bal_Fuel();

                DataTable _AnalogData = new DataTable();

                DataSet ds = new DataSet();

                bool _ObdStatus = false;


                ds = GetConnectedAnalogDevice(eL_Fuel);

                var _t_count = 0;

                if (ds.Tables.Count > 0)
                {
                    foreach (DataTable dt in ds.Tables)
                    {

                        if (_t_count == 0)
                            _AnalogData = dt;


                        if (_t_count == 1 && dt.Rows.Count > 0)
                            _ObdStatus = Convert.ToBoolean(ds.Tables[1].Rows[0]["ObdStatus"]);

                        _t_count += 1;
                    }

                    _AnalogData = ds.Tables[0].Copy();                   

                    if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        _ObdStatus = Convert.ToBoolean(ds.Tables[1].Rows[0]["ObdStatus"]);
                    }
                }

          
            var data = new EL_AnalogMapping
            {
                AnalogData = _AnalogData,
                ObdStatus = _ObdStatus,
            };

            return data;
           
        }

        public static Tuple<DateTime, DateTime, double> calenderWeekHelper(EL_Fuel _EL_Fuel)
        {

            string TimeZoneId = _EL_Fuel.TimeZoneID;

            int i = _EL_Fuel.ShowDataFor;

            //beginning Date instantiation
            var dStartDate = new DateTime();

            //get current utc datetime 
            var UtcNow = new DateTime(DateTime.UtcNow.Ticks, DateTimeKind.Unspecified);

            //get current user local  time 


            DateTime userTimeNow = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(UtcNow, TimeZoneId);

            var timeDifference = userTimeNow.Subtract(UtcNow);

            //Create A starting Date for that month


            switch (i)
            {

                case -1:
                    //custom date                

                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(Convert.ToDateTime(_EL_Fuel.startDate), TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(Convert.ToDateTime(_EL_Fuel.endDate), TimeZoneId); ;
                    break;
                case 0:
                    //today 
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date, TimeZoneId);

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;

                case 1:
                    //yesterday                   
                    var yesterdayEnd = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date.AddSeconds(-1), TimeZoneId);

                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.Date.AddSeconds(-1).Date, TimeZoneId);

                    UtcNow = yesterdayEnd;
                    break;
                case 7:
                    //last seven days
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-6).Date, TimeZoneId);

                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;



                //case 1:
                //    //current week
                //    var monday = userTimeNow.GetFirstDayOfThisWeek().Date;

                //    DateTime utcMonday = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(monday, TimeZoneId);

                //    dStartDate = utcMonday;

                //    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);

                //    break;
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

        public static Tuple<DateTime, DateTime, double> GetUserTimeCalenderWeekHelper(EL_Fuel _EL_Fuel)
        {
            var _tuple = calenderWeekHelper(_EL_Fuel);
            var dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(_tuple.Item1, _EL_Fuel.TimeZoneID);
            var dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(_tuple.Item2, _EL_Fuel.TimeZoneID);

            return new Tuple<DateTime, DateTime, double>(dStartDate, dEndDate, _tuple.Item3);

        }

        public static El_SensorConfig GetAssetConfigSettings(EL_Fuel eL_Fuel)
        {

            var _El_SensorConfig = new El_SensorConfig();


            SqlParameter[] param = new SqlParameter[1];

            param[0] = new SqlParameter("@ifk_asset_id", SqlDbType.VarChar);

            param[0].Value = eL_Fuel.ifkAssetID;

            var ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_fuel_module_get_limits", param);

            Tuple<double, double> _min_max = new Tuple<double, double>(0, 60);

            foreach (DataTable dt in ds.Tables)
                foreach (DataRow dr in dt.Rows)
                {
                    _El_SensorConfig.Max = Convert.ToDouble(dr["iMaxValue"]);
                    _El_SensorConfig.Min = Convert.ToDouble(dr["iMinValue"]);
                    _El_SensorConfig.SampleAvarage = Convert.ToInt32(dr["iAveraging"]);
                    _El_SensorConfig.IgnoreWhenIgnOff = Convert.ToBoolean(dr["ignoreWhenIgnOff"]);
                    _El_SensorConfig.TrackerType = Convert.ToInt32(dr["TrackerType"]);




                    //###################################################################################                   
                    _El_SensorConfig.SpikeRemover_SamplePoints = Convert.ToInt32(dr["spike_sample_points"]);
                    _El_SensorConfig.SpikeRemover_Threshold = Convert.ToInt32(dr["spike_threshold"]);
                    _El_SensorConfig.SpikeRemover_ThresholdReturnDiff = Convert.ToInt32(dr["spike_threshold_return"]);
                    _El_SensorConfig.IgnoreFirstNMinsWhenIgnOn = Convert.ToInt32(dr["ignore_ign_on_mins"]);

                             


                    _El_SensorConfig.EventSuddenDecreaseValue = Convert.ToInt32(dr["sudden_drop"]);
                    _El_SensorConfig.EventSuddenIncreaseValue = Convert.ToInt32(dr["sudden_increase"]);
                    _El_SensorConfig.ImeiNumber = Convert.ToInt64(dr["ImeiNumber"]);
                    
                }



            return _El_SensorConfig;
        }

        public static Hashtable GetGraphData_AssetInfo(EL_Fuel _EL_Fuel, El_FuelDataSource existing_data = null)
        {
            var _returnObj = new Hashtable();

            //Get RAW data set from Database
            var _Bal_Fuel = new Bal_Fuel();    

            var _ObjectSource = existing_data ?? _Bal_Fuel.GetFuelData(_EL_Fuel);

            var rawDataSet = _ObjectSource.dtSource;

            if (rawDataSet.Rows.Count == 0)
            {
                // return null; //todo: test and handle this

                /// rono : it broke the graph logic coz it doesnt anticipate null so i did this                /// 
                              

                _returnObj.Add("GraphItem", new El_FuelUIObj());
                _returnObj.Add("DrainsItem", new El_FuelUIObj());
                _returnObj.Add("RefillsItem", new El_FuelUIObj());

                return _returnObj; 

            }

            /////////////////////////////////////////////////////
            // Variables for cleaning and smoothing data
            /////////////////////////////////////////////////////

            //get asset configuration (drain threshold etc)
            var assetConfig = Bal_Fuel.GetAssetConfigSettings(_EL_Fuel);

            _EL_Fuel.vpkDeviceId = assetConfig.ImeiNumber;

            //ignore the first N mins of ignition on data to accomodate fuel gauges that slowly rise when ign on
            int ignoreMinsAfterIgnOn = 0; //todo investigate why this makes graph steppy
            ignoreMinsAfterIgnOn = assetConfig.IgnoreFirstNMinsWhenIgnOn;

            //get the amount of data points to average by
            int meanFilter_SamplePointsAveraging = assetConfig.SampleAvarage < 1 ? 1 : assetConfig.SampleAvarage; // MEAN FILTER- Looks ahead by N points and gives the average

            //assetConfig.IgnoreWhenIgnOff = assetConfig.IgnoreWhenIgnOff ;
           

            int spikeRemover_SamplePoints  = assetConfig.SpikeRemover_SamplePoints;           //SPIKER REMOVER - Looks ahead by N points to see if spike comes back in range
            double spikeRemover_Threshold  = assetConfig.SpikeRemover_Threshold;              //SPIKER REMOVER - anything above this threshold is considered a SPIKE
            double spikeRemover_ReturnDiff = assetConfig.SpikeRemover_ThresholdReturnDiff;    //SPIKER REMOVER - if any of the future points come within N point to the first value, then it will be DeSpiked, the lower the value the less accurate the de-spiking.


            /////////////////////////////////////////////////////

             
            // declaring various lists that hold data as its processed, reprocessed and reprocessed again...
            List<FuelDataSet> processedFuelListToReturn = new List<FuelDataSet>();

            Dictionary<int, double> rawFuelValues = new Dictionary<int, double>();
            Dictionary<int, double> processedFuelValues = new Dictionary<int, double>();

            //Dictionary<int, double> ignitionRemoverValues = new Dictionary<int, double>();
            //Dictionary<int, double> deSpikedValues = new Dictionary<int, double>();

            List<FuelDataSet> graphDataPointsList = new List<FuelDataSet>();
            List<FuelDataSet> cleanedFuelList = new List<FuelDataSet>();
            List<double> data = new List<double>();


            // var rawDataCount = rawDataSet.Rows.Count;


            /////////////////////////////////////////////////////
            /// Load Raw Fuel data into Dictonary
            /////////////////////////////////////////////////////
            for (int i = 0; i < rawDataSet.Rows.Count; i++)
            {
                rawFuelValues[i] = Convert.ToDouble(rawDataSet.Rows[i]["RawFuel"]);
                processedFuelValues[i] = rawFuelValues[i];

            }
            /////////////////////////////////////////////////////




            /////////////////////////////////////////////////////
            /// Ignition off remover
            /////////////////////////////////////////////////////
            /// Summary: cleans data if config is set to ignore ign off data (this is populated regardless if they have set to ignore ign off data)
            #region #Ignition off remover

            double lastIgnitionOnFuelValue = 0;

            ///////////////////////////
            // set lastIgnitionOnFuelValue
            ///////////////////////////
            // Summmary: get and set first ignition on value for graph
            #region #Ignition off remover - get first ignition on value

            if (assetConfig.IgnoreWhenIgnOff)
            {

                if (!Convert.ToBoolean(rawDataSet.Rows[0]["ign"])) 
                {
                    //first graph point is ignition off, so we have to find the last ignition on fuel value
                    //todo check these datetimes are correct UTC or use?

                    //###################################################################################
                    // TODO TESTING HACK REMOVE
                    //_EL_Fuel.vpkDeviceId = 358480086750090;
                    //###################################################################################

                    var _DAL_Fuel = new DAL_Fuel();
                    var ds = _DAL_Fuel.GetLastIgnitionOnSensorValue(
                        _EL_Fuel.vpkDeviceId
                        , Convert.ToDateTime(rawDataSet.Rows[0]["date"]).AddDays(-30)
                        , Convert.ToDateTime(rawDataSet.Rows[0]["date"])
                        ); ;

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        
                        // No previous ign on value, find first ignition on value and use that instead.

                        for (int i = 0; i < processedFuelValues.Count; i++)
                        {
                            if (Convert.ToBoolean(rawDataSet.Rows[i]["ign"]))
                            {
                                //get last ignition on fuel value
                                lastIgnitionOnFuelValue = processedFuelValues[i];
                                break; //we have the first ignition on value, so exit
                            }
                        }
                    }
                    else
                    {

                        List<double> dbFuelVals = new List<double>();

                        foreach( DataRow dr in ds.Tables[0].Rows)
                        {
                           
                            var fuel_value = Bal_FuelUtils.GetFuelValue( dr, _EL_Fuel.sensorType, _EL_Fuel.iAnalogNumber);

                            dbFuelVals.Add(fuel_value ?? 0);
                           
                        }
                        lastIgnitionOnFuelValue = dbFuelVals.Mean();

                    }
                    
                }
                else
                {
                    lastIgnitionOnFuelValue = Convert.ToDouble(rawDataSet.Rows[0]["RawFuel"]);
                }

                if (lastIgnitionOnFuelValue == 0)
                {
                    //if it hits here, there is no ignition on data on the whole dataset
                    lastIgnitionOnFuelValue = Convert.ToDouble(rawDataSet.Rows[0]["RawFuel"]);
                }


                
            }
            else
            {
                //IgnoreWhenIgnOff is false for this asset so use first item in data
                lastIgnitionOnFuelValue = processedFuelValues[0];
            }
            #endregion
            ///////////////////////////

            for (int i = 0; i < processedFuelValues.Count; i++)
            {

                //deal with outOfRange
                bool firstValue = i == 0;
                bool lastValue = i == processedFuelValues.Count - 1;
                double currentValue = processedFuelValues[i];
                double previousValue = currentValue;
                double nextValue = currentValue;
                if (!firstValue) { previousValue = processedFuelValues[i - 1]; } //Convert.ToDouble(rawDataSet.Rows[i - 1]["RawFuel"]); }
                if (!lastValue) { nextValue = processedFuelValues[i + 1]; } // Convert.ToDouble(rawDataSet.Rows[i + 1]["RawFuel"]); }


                //deal with ignition (ignore off etc)
                bool ignState = Convert.ToBoolean(rawDataSet.Rows[i]["ign"]);
                if (assetConfig.IgnoreWhenIgnOff && !ignState)
                {
                    currentValue = lastIgnitionOnFuelValue;
                }
                else
                {
                    lastIgnitionOnFuelValue = currentValue;
                }

                //save cleaned ignition values
                processedFuelValues[i] = currentValue;

            }

            #endregion
            /////////////////////////////////////////////////////



            /////////////////////////////////////////////////////
            /// Ignition On Ignore N Mins Filter
            /////////////////////////////////////////////////////
            /// Summary: ignores the first N mins after ignition on (to deal with fuel guages that rise slowly after ign on), use [ignoreMinsAfterIgnOn] as variable for configuration
            // todo: if graph starts and ignition is already on, it might mess up start of graph - so get ignition on time and work out the start
            #region #Ignition on mins filter

            if (ignoreMinsAfterIgnOn > 0)
            {
                //double firstRawValue = Convert.ToDouble(rawDataSet.Rows[0]["RawFuel"]);
                //double lastRawValueToKeep = Convert.ToDouble(rawDataSet.Rows[0]["RawFuel"]);
                bool lastRecordIgnitionStatus = false;
                DateTime ignitionOnTime = DateTime.Now.AddYears(-10);



                //check if igntion is already on
                if (Convert.ToBoolean(rawDataSet.Rows[0]["ign"]))
                {

                    //ignition is already  on, this is a pain as we have to get see if its within the [ignoreMinsAfterIgnOn] range
                    //todo: firstRawValue =  select rawFuelvalue from commontable, where igntion is off, order by datetime desc (do this in c#)
                    //todo: work out how long: secondsAfterIgnitionOn = ?

                    //DateTime igntionOnTime = fuelHelper.GetTimeIgnitionWasSwitchedOn(Convert.ToDateTime(rawDataSet.Rows[0]["date"])
                    //if(igntionOnMins > (datediffmins(igntionOnTime, currentrecord)
                    //{
                    //   the ignition hasn't been on for long enough jump ahead and get starting value
                    //}
                    //else
                    // the ignition has been on for long enough now so use current value?
                }



                for (int i = 0; i < processedFuelValues.Count; i++)
                {

                    if (Convert.ToBoolean(rawDataSet.Rows[i]["ign"]) && !lastRecordIgnitionStatus)
                    {
                        //just turned igntion on
                        ignitionOnTime = Convert.ToDateTime(rawDataSet.Rows[i]["date"]);


                        //jump ahead for n mins and update these values.
                        for (int j = 0; j + i < processedFuelValues.Count; j++)
                        {
                             TimeSpan timeSinceIgnOn = Convert.ToDateTime(rawDataSet.Rows[j + i]["date"]) - ignitionOnTime;

                            if (timeSinceIgnOn.TotalMinutes < ignoreMinsAfterIgnOn)
                            {
                                if (i  > 0)
                                    processedFuelValues[j + i] = processedFuelValues[i - 1];
                                else
                                    processedFuelValues[j + i] = processedFuelValues[0];

                            }

                        }
                    }
                }
            }

            #endregion
            /////////////////////////////////////////////////////


            /////////////////////////////////////////////////////
            /// Single spike remover
            /////////////////////////////////////////////////////
            #region #Single spike remover
            double singleSpikeRemover_Threshold = 10;
            if (spikeRemover_Threshold != 0)
            {
                singleSpikeRemover_Threshold = spikeRemover_Threshold;//override value with what is specified
            }
            for (int i = 0; i < processedFuelValues.Count; i++)
            {
                if (i + 2 < processedFuelValues.Count)
                {
                    //deal with outOfRange
                    if (Math.Abs(processedFuelValues[i] - processedFuelValues[i + 1]) > singleSpikeRemover_Threshold) //spikeRemover_Threshold) //differential between 2 points is high
                    {
                        if (Math.Abs(processedFuelValues[i] - processedFuelValues[i + 2]) <= 1)//differential between next (3rd) point returns within range so probably a spike...so clean!
                        {
                            processedFuelValues[i + 1] = processedFuelValues[i]; //flatten the single spike
                        }
                    }
                }
            }
            #endregion


            /////////////////////////////////////////////////////
            /// Spike Remover 
            /// todo: make spikeRemoverSamplePoints config option
            /////////////////////////////////////////////////////
            /// Summary: looks for extreme up or down over many [spikeRemoverSamplePoints] data points and flattens the data into its own object, (makes averaging look cleaner as you dont get the humps)
            /// 
            #region #Prolonged spike remover



            for (int i = 0; i < processedFuelValues.Count; i++)
            {
                //deal with outOfRange
                bool firstValue = i == 0;
                bool lastValue = i == processedFuelValues.Count - 1;
                double currentValue = processedFuelValues[i];
                double previousValue = currentValue;
                double nextValue = currentValue;
                if (!firstValue) { previousValue = processedFuelValues[i - 1]; }
                if (!lastValue) { nextValue = processedFuelValues[i + 1]; }

                //detect the single spike here
                if (Math.Abs(previousValue - currentValue) > spikeRemover_Threshold)
                {

                    //jump ahead by [spikeRemoverSamplePoints] points and see if spike returns back to nominal value
                    for (int j = 0; j < spikeRemover_SamplePoints; j++)
                    {

                        if (i + j < processedFuelValues.Count) //make sure its not out of range
                        {
                            //check future value to see if it returns to minor difference
                            if (Math.Abs(previousValue - processedFuelValues[i + j]) < spikeRemover_ReturnDiff)
                            {
                                //SPIKE DETECTED: woohoo its returned to minor difference, time to despike..... so remove these spikes from dict

                                for (int k = i; k < (i + j); k++)
                                {
                                    processedFuelValues[k] = previousValue;//go back through all the items in dict and set as despiked value.

                                }

                            }
                        }
                    }
                }
            }


            #endregion
            /////////////////////////////////////////////////////




            /////////////////////////////////////////////////////
            /// Averaging mean filter -  build cleanedFuelList (after de-spiking the averaging should look a lot cleaner)
            /////////////////////////////////////////////////////
            /// Summary: cycle through each raw data point (average if enabled) and add to cleanedFuelList
            #region Averaging - mean filter
                   

            for (int i = 0; i < processedFuelValues.Count; i++)
            {
                #region raw varibles               
                // double rawPoint = rawFuelValues[i]; //Convert.ToDouble(rawDataSet.Rows[i]["RawFuel"]);
                //double averagedPoint = Convert.ToDouble(rawDataSet.Rows[i]["RawFuel"]);
                //double processedPoint = ignitionRemoverValues[i];
                var speed = Convert.ToDouble(rawDataSet.Rows[i]["spid"]);
                var ignState = Convert.ToInt32(rawDataSet.Rows[i]["ign"]);
                DateTime gpsDateTime = Convert.ToDateTime(rawDataSet.Rows[i]["date"]);


                #endregion

                FuelDataSet fuelItem = new FuelDataSet();

                //For each take n points forward and do the average
                //var lastRawWithIgntionOn = rawPoint;


                // if SamplePointsAveraging > 1 then averaging will kick in here,
                // if ignoreWhenIgnOff - it will use the previous value
                for (int j = 0; j < meanFilter_SamplePointsAveraging; j++)
                {

                    if (i + j < processedFuelValues.Count)
                    {
                        data.Add(processedFuelValues[i + j]);

                    }

                }


                fuelItem.Avg = data.Median(); //just one value if not looking ahead to average
                fuelItem.Raw = rawFuelValues[i];
                fuelItem.DeSpiked = data.Median();
                //fuelItem.DeSpiked = ignitionRemoverValues[i];
                fuelItem.Date = gpsDateTime;
                fuelItem.Speed = speed;
                fuelItem.Ign = ignState;               

                fuelItem.Location = Convert.ToString(rawDataSet.Rows[i]["vTextMessage"]);
                fuelItem.ipkCommanTrackingID = Convert.ToInt64(rawDataSet.Rows[i]["ipkCommanTrackingID"]);


                fuelItem.Lat = Convert.ToString(rawDataSet.Rows[i]["Lat"]);
                fuelItem.Lon = Convert.ToString(rawDataSet.Rows[i]["Lon"]);


                cleanedFuelList.Add(fuelItem);


                data.Clear();
            }

            #endregion
            /////////////////////////////////////////////////////







            /////////////////////////////////////////////////////
            /// build cleanedFuelList (after de-spiking the averaging should look a lot cleaner)
            /////////////////////////////////////////////////////
            #region oldAveraging
            /// Summary: cycle through each raw data point (average if enabled) and add to cleanedFuelList
            //for (int i = 0; i < rawDataCount; i++)
            //{
            //    #region raw varibles               
            //    double rawPoint = Convert.ToDouble(rawDataSet.Rows[i]["RawFuel"]);
            //    double averagedPoint = Convert.ToDouble(rawDataSet.Rows[i]["RawFuel"]);
            //    var speed = Convert.ToDouble(rawDataSet.Rows[i]["spid"]);
            //    var ignState = Convert.ToInt32(rawDataSet.Rows[i]["ign"]);               
            //    DateTime gpsDateTime = Convert.ToDateTime(rawDataSet.Rows[i]["date"]);


            //    if (long.TryParse(Convert.ToString(rawDataSet.Rows[i]["EngineHours"]), out _))
            //        lastEngineHours = Convert.ToInt64(rawDataSet.Rows[i]["EngineHours"]);


            //    if (double.TryParse(Convert.ToString(rawDataSet.Rows[i]["odo"]), out _))
            //        last_odo = Convert.ToDouble(rawDataSet.Rows[i]["odo"]);


            //    if (first_odo < 0.0 && last_odo >= 0.0)
            //        first_odo = last_odo;

            //    if (firstEngineHours < 0 && lastEngineHours >= 0)
            //        firstEngineHours = lastEngineHours;
            //    #endregion

            //    FuelDataSet fuelItem = new FuelDataSet();

            //    //For each take n points forward and do the average
            //    var lastRawWithIgntionOn = rawPoint;


            //    // if SamplePointsAveraging > 1 then averaging will kick in here,
            //    // if ignoreWhenIgnOff - it will use the previous value
            //    for (int j = 0; j < SamplePointsAveraging; j++)
            //    {

            //        if (i + j < rawDataSet.Rows.Count)
            //        {
            //            bool currentlyIgnOn = Convert.ToBoolean(rawDataSet.Rows[i + j]["ign"]);
            //            if (currentlyIgnOn)
            //            {
            //                lastRawWithIgntionOn = Convert.ToDouble(rawDataSet.Rows[i + j]["RawFuel"]);
            //            }

            //            if (assetConfig.IgnoreWhenIgnOff && !currentlyIgnOn)
            //            {
            //                data.Add(lastRawWithIgntionOn);
            //            }
            //            else
            //            {
            //                data.Add(Convert.ToDouble(rawDataSet.Rows[i + j]["RawFuel"]));
            //            }

            //        }

            //    }

            //    averagedPoint = data.Median(); //just one value if not looking ahead to average


            //    fuelItem.Avg = averagedPoint;
            //    fuelItem.Raw = rawPoint;
            //    //fuelItem.DeSpiked = averagedPoint;
            //    fuelItem.DeSpiked = deSpikedValues[i];
            //    fuelItem.Date = gpsDateTime;
            //    fuelItem.Speed = speed;
            //    fuelItem.Ign = ignState;
            //    fuelItem.Location = Convert.ToString(rawDataSet.Rows[i]["vTextMessage"]);
            //    fuelItem.ipkCommanTrackingID = Convert.ToInt64(rawDataSet.Rows[i]["ipkCommanTrackingID"]);

            //    cleanedFuelList.Add(fuelItem);


            //    data.Clear();
            //}
            #endregion
            /////////////////////////////////////////////////////




            /////////////////////////////////////////////////////
            // find refills and drains and create chunks
            List<RefillDrainChunks> findChunks = ProcessSensorIncreaseDecrease(cleanedFuelList, assetConfig);
            /////////////////////////////////////////////////////

            //each chunk will be added to a partial list and then added to big one at end to stop the averaging on refills / drains


            //List<FuelDataSet> partialGraphDataPointsList = new List<FuelDataSet>();
            //todo what if chunks are 0

            //List<int> idsToNotAverage = new List<int>();
            Dictionary<int, int> dictIdsNotToAverage = new Dictionary<int, int>();


            for (int ii = 0; ii < findChunks.Count; ii++)
            //foreach (var c in findChunks) {
            {

                for (int iii = findChunks[ii].ReadingsStartIndex; iii <= findChunks[ii].ReadingsEndIndex; iii++)
                {
                    //idsToNotAverage.Add(iii);
                    dictIdsNotToAverage[iii] = findChunks[ii].ReadingsEndIndex;
                }


            }
            /////////////////////////////////////////////////////




            //todo if engine hours is screwed up, then work out manually.

            //////////////////////////////////////////////////////////////////////////////////////////////////////
            /////NOTE - REFILLS AND DRAINS ALREADY PROCESSED, SO BELOW EFFECTS BLUE LINE ONLY
            //////////////////////////////////////////////////////////////////////////////////////////////////////




            /////////////////////////////////////////////////////
            /// // big spikeRemover filter - but forward slashes the refills
            /////////////////////////////////////////////////////


            Queue<double> spikeRemoverQ = new Queue<double>();
            int fuelPointsCount = cleanedFuelList.Count();
            int fuelPointer = 0;
            var isFirstRow = true;

            foreach (var item in cleanedFuelList)
            {

                double raw = item.Raw;

                double avg = item.Avg;

                DateTime myDate = item.Date;

                FuelDataSet fuelItem = new FuelDataSet();


                if (dictIdsNotToAverage.TryGetValue(fuelPointer, out int useThisId))
                {

                    spikeRemoverQ.Clear();
                    spikeRemoverQ.Enqueue(cleanedFuelList[useThisId].DeSpiked);

                }

                //bool dontAverageItem = idsToNotAverage.Any(item => item == fuelPointer);

                //maintain 10 items in queue
                if (spikeRemoverQ.Count > 21)
                {
                    spikeRemoverQ.Dequeue();
                }


                double spikeRemoverAvg;
                //If you see a refill or drain stop averaging
                var fiveAhead = cleanedFuelList.AsEnumerable().Take(5);
                double increase = 0;
                foreach (var itemAhead in fiveAhead)
                {
                    //if (itemAhead.Avg > item.Avg)
                    //{
                    increase = itemAhead.Avg - item.Avg;
                    //}
                }
                if (increase > 10)
                {
                    //spikeRemoverQ.Clear();
                    //spikeRemoverQ.Enqueue(raw);
                    //spikeRemoverAvg = raw;
                }
                else
                {
                    //spikeRemoverAvg = spikeRemoverQ.Sum() / spikeRemoverQ.Count();
                }
                spikeRemoverAvg = spikeRemoverQ.Sum() / spikeRemoverQ.Count();


                // if this is the first datapoint neglect spikeremover on avg property 

                fuelItem.Avg = spikeRemoverAvg;
                fuelItem.Raw = raw;
                fuelItem.DeSpiked = isFirstRow ? avg : spikeRemoverAvg;
                fuelItem.Ign = item.Ign;
                fuelItem.Date = item.Date;
                fuelItem.Speed = item.Speed;
                fuelItem.Location = item.Location;
                fuelItem.ipkCommanTrackingID = item.ipkCommanTrackingID;

                // added this to fix parse error on client json 
                if (Double.IsNaN(fuelItem.Avg) || Double.IsInfinity(fuelItem.Avg))
                    fuelItem.Avg = 0;

                if (Double.IsNaN(fuelItem.DeSpiked) || Double.IsInfinity(fuelItem.DeSpiked))
                    fuelItem.DeSpiked = 0;



                graphDataPointsList.Add(fuelItem);
                //partialGraphDataPointsList.Add(fuelItem);

                spikeRemoverQ.Enqueue(avg);

                fuelPointer++;
                isFirstRow = false;
            }
            /////////////////////////////////////////////////////



            var _lst_FuelGraphItems = new List<El_FuelGraphItem>();

            foreach (var item in graphDataPointsList)
            {
                _lst_FuelGraphItems.Add(new El_FuelGraphItem
                {
                    date = item.Date,
                    AF = item.DeSpiked,
                    RF = item.Raw,
                    spid = item.Speed,
                    ign = item.Ign
                });
            }


            var WrappedGraphItemListData = new El_FuelUIObj
            {
                FuelItems = _lst_FuelGraphItems,
                TotalDistance = _ObjectSource.TotalDistance,
                TotalEngineHours = _ObjectSource.TotalEngineHours,
                FuelUsed = 0.0,
                FuelConsumpt = 0.0, //economy
                TotalDrains = 0.0, // total drains 
                TotalRefills = 0.0, //total refills
                ifkFuelMeasurementUnit = 2,//unit 
                maxValue = 200,  //the max point on the graph 
                minValue = 0,  //the max point on the graph 
            };



            //Refuel  & refills  

            var r_List = new List<EL_FuelItem>();

            var d_List = new List<EL_FuelItem>();


            foreach (var chunk in findChunks)
            {
                var r = new EL_FuelItem();

                var d = new EL_FuelItem();


                if (chunk.IsRefill)
                {
                    r.dGPSDateTime = chunk.EventDateTime;

                    r.IsRefuel = 1;

                    r.Filled = UserSettings.ConvertFuelToXX(_EL_Fuel.ifkFuelMeasurementUnit, chunk.ChangeAmount);

                    r.InitialFuel = UserSettings.ConvertFuelToXX(_EL_Fuel.ifkFuelMeasurementUnit, chunk.ReadingsInitialFuel);

                    r.FinalFuel = UserSettings.ConvertFuelToXX(_EL_Fuel.ifkFuelMeasurementUnit, chunk.ReadingsFinalFuel);

                    r.vTextMessage = string.IsNullOrWhiteSpace(chunk.Location) ? "Location" : chunk.Location;

                    r.rowid = chunk.ipkCommanTrackingID;


                    r.Lat = chunk.Lat;

                    r.Lon = chunk.Lon;

                    r_List.Add(r);

                    WrappedGraphItemListData.TotalRefills += chunk.ChangeAmount;


                }
                else
                {
                    d.dGPSDateTime = chunk.EventDateTime;

                    d.Drained = UserSettings.ConvertFuelToXX(_EL_Fuel.ifkFuelMeasurementUnit, chunk.ChangeAmount);

                    d.InitialFuel = UserSettings.ConvertFuelToXX(_EL_Fuel.ifkFuelMeasurementUnit, chunk.ReadingsInitialFuel);

                    d.FinalFuel = UserSettings.ConvertFuelToXX(_EL_Fuel.ifkFuelMeasurementUnit, chunk.ReadingsFinalFuel);

                    d.vTextMessage = string.IsNullOrWhiteSpace(chunk.Location) ? "Location" : chunk.Location;

                    d.rowid = chunk.ipkCommanTrackingID;

                    d.Lat = chunk.Lat;

                    d.Lon = chunk.Lon;

                    d_List.Add(d);

                    WrappedGraphItemListData.TotalDrains += chunk.ChangeAmount;
                }

            }


            var fuel_item_count = cleanedFuelList.Count();

            if (fuel_item_count > 0)
            {
                double _fuelUsed = ((cleanedFuelList[0].DeSpiked - cleanedFuelList[fuel_item_count - 1].DeSpiked) + WrappedGraphItemListData.TotalRefills);

                //////
                /// dealing with user defined measurement units 
                ///


                WrappedGraphItemListData.FuelUsed = _fuelUsed;

                if (_EL_Fuel.Tracker_Type == 4)  ///static device type 
                {
                    if (WrappedGraphItemListData.FuelUsed == 0)  // to avoid  dividing a zero which results in an Infinity
                        WrappedGraphItemListData.FuelConsumpt = 0;
                    else
                    {
                        if (WrappedGraphItemListData.TotalEngineHours > 0)
                            WrappedGraphItemListData.FuelConsumpt = WrappedGraphItemListData.FuelUsed / WrappedGraphItemListData.TotalEngineHours;
                    }
                }
                else
                {

                    var total_distance = WrappedGraphItemListData.TotalDistance;

                    if (WrappedGraphItemListData.FuelUsed == 0)  // to avoid  dividing a zero which results in an Infinity
                        WrappedGraphItemListData.FuelConsumpt = 0;
                    else
                    {
                        if (_EL_Fuel.ifkFuelMeasurementUnit == 1) // miles/gallon
                        {

                            WrappedGraphItemListData.FuelUsed *= 0.264172;

                            WrappedGraphItemListData.TotalRefills *= 0.264172;

                            WrappedGraphItemListData.TotalDrains *= 0.264172;

                            total_distance *= 0.621371;

                            WrappedGraphItemListData.FuelConsumpt = total_distance / WrappedGraphItemListData.FuelUsed;

                        }
                        else if (_EL_Fuel.ifkFuelMeasurementUnit == 3) // L/100KM
                        {
                            WrappedGraphItemListData.FuelConsumpt = (100 * WrappedGraphItemListData.FuelUsed) / total_distance;

                        }
                        else //KM/L
                        {
                            WrappedGraphItemListData.FuelConsumpt = total_distance / WrappedGraphItemListData.FuelUsed;

                        }


                    }

                }


            }

            var _RefuelListWrapped = new El_FuelUIObj
            {

                ifkFuelMeasurementUnit = 1
            };

            _RefuelListWrapped.Refuels = r_List;

            //drains 
            ///=============================================================
            //same as above so 

            var _DrainListListWrapped = new El_FuelUIObj { };

            _DrainListListWrapped.Drains = d_List;          

            _returnObj.Add("GraphItem", WrappedGraphItemListData);

            _returnObj.Add("DrainsItem", _DrainListListWrapped);

            _returnObj.Add("RefillsItem", _RefuelListWrapped);


            return _returnObj;

        }


        public static List<Hashtable> GetFuelGraph(EL_Fuel eL_Fuel, DataTable dt_mapping_details)
        {
            var dateTuple = Bal_Fuel.GetUserTimeCalenderWeekHelper(eL_Fuel);

            eL_Fuel.startDate = dateTuple.Item1.ToString("yyyy-MM-dd HH:mm:ss");

            eL_Fuel.endDate = dateTuple.Item2.ToString("yyyy-MM-dd HH:mm:ss");

            var defaults = Bal_Fuel.GetAssetConfigSettings(eL_Fuel);

         

            eL_Fuel.Tracker_Type = defaults.TrackerType;


            var MultipleFuelDetails = new List<Hashtable>();

            foreach (DataRow details in dt_mapping_details.Rows)
            {


                var result = new Hashtable();

                eL_Fuel.iAnalogNumber = Convert.ToInt32(details["iAnalogNumber"]);
                eL_Fuel.sensorType = Convert.ToInt32(details["SensorType"]);
                eL_Fuel.canBusType = Convert.ToInt32(details["ifkCanBusType"]);
                eL_Fuel.oneWireID = Convert.ToString(details["DeviceId1Wire"]);
                eL_Fuel.MappedName = Convert.ToString(details["vName"]);
                eL_Fuel.AnalogType = Convert.ToInt32(details["AnalogType"]);

                if (!((eL_Fuel.sensorType == 7 && eL_Fuel.canBusType == 1) || eL_Fuel.AnalogType == 1))
                    continue;

                var dtData = new Bal_Fuel().GetFuelData(eL_Fuel);

                result = Bal_Fuel.GetGraphData_AssetInfo(eL_Fuel, dtData);

                var GraphItem = result["GraphItem"] as El_FuelUIObj;

                GraphItem.MappedName = eL_Fuel.MappedName;

                GraphItem.TrackerType = defaults.TrackerType;

                GraphItem.minValue = defaults.Min;

                var user_max = defaults.Max;

                var _data_max = 0.0;


                if (GraphItem.FuelItems.Any())
                    _data_max = GraphItem.FuelItems.Max(n => n.AF);

                if (_data_max > user_max)
                {
                    GraphItem.maxValue = _data_max + 5;
                }

                if (_data_max < user_max)
                {
                    GraphItem.maxValue = user_max + 5;
                }

                //javascript 60 S
                if (_data_max < 60 && user_max < 60)
                {
                    GraphItem.maxValue = 60;
                }

                GraphItem.ifkFuelMeasurementUnit = eL_Fuel.ifkFuelMeasurementUnit;


                MultipleFuelDetails.Add(result);
            }



            return MultipleFuelDetails;

        }


        private static List<RefillDrainChunks> ProcessSensorIncreaseDecrease(List<FuelDataSet> FuelPointsList, El_SensorConfig config)
        {
            List<RefillDrainChunks> refillAndDrainsList = new List<RefillDrainChunks>();
            try
            {


                if (FuelPointsList.Count > 0)
                {
                    bool sudden_IsRising = false;
                    bool sudden_IsDropping = false;

                    int sudden_IsRisingCount = 0;
                    int sudden_IsDroppingCount = 0;

                    double sudden_IsRisingStartingValue = 0;
                    double sudden_IsDroppingStartingValue = 0;

                    int sudden_IsRisingStartedRecord = 0;
                    int sudden_IsDroppingStartedRecord = 0;




                    //foreach (var item in FuelPointsList) {
                    for (var i = 0; i < FuelPointsList.Count; i++)
                    {
                        //Console.WriteLine("Amount is {0} and type is {1}", myMoney[i].amount, myMoney[i].type);

                        //double average = sensorReadings.ReadingsUnits.Average(x => Convert.ToDouble(x));
                        //double sumOfSquaresOfDifferences = sensorReadings.ReadingsUnits.Select(val => (val - average) * (val - average)).Sum();
                        //double sigmaOfReadings = Math.Sqrt(sumOfSquaresOfDifferences / sensorReadings.ReadingsUnits.Count);

                        double currentReading = FuelPointsList[i].DeSpiked;
                        double previousReading = currentReading;
                        if (i > 0)
                        {
                            previousReading = FuelPointsList[i - 1].DeSpiked;
                        }
                        //string strPreviousReading = (FuelPointsList[i - 1].DeSpiked.ToString() ?? FuelPointsList[i].DeSpiked.ToString());
                        //double previousReading = double.Parse(strPreviousReading);

                        double suddenDecreaseDifference = previousReading - currentReading;
                        double suddenIncreaseDifference = currentReading - previousReading; //if +ve value, refuelling must have been done

                        if (suddenIncreaseDifference < 0)
                        {
                            suddenIncreaseDifference = 0;
                        }
                        if (suddenDecreaseDifference < 0)
                        {
                            suddenDecreaseDifference = 0;
                        }

                        ////////////////////////////////////
                        // set SuddenDropThreshold
                        ////////////////////////////////////
                        double eventSuddenDropThreshold = config.EventSuddenDecreaseValue == 0 ? 10 : config.EventSuddenDecreaseValue;
                        ////////////////////////////////////


                        ////////////////////////////////////
                        // set SuddenIncreaseThreshold
                        ////////////////////////////////////
                        double eventSuddenIncreaseThreshold = config.EventSuddenIncreaseValue == 0 ? 10 : config.EventSuddenIncreaseValue;
                        ////////////////////////////////////


                        ////////////////////////////////////
                        // NEW check sudden increase
                        ////////////////////////////////////
                        //if (suddenIncreaseDifference >= eventSuddenIncreaseThreshold && eventSuddenIncreaseThreshold > 0 && !sudden_IsRising && position.VehicleSpeed == 0)



                        // first check for slooow rises and mark this as a slow riser
                        double slowRiseTotal = 0;
                        if (!sudden_IsRising) { 
                            for (int iSlow = 0; iSlow < 20; iSlow++)
                            {
                                if (i + iSlow < FuelPointsList.Count && iSlow!=0) //make sure its not out of range
                                {
                                    slowRiseTotal += (FuelPointsList[i + iSlow].DeSpiked - FuelPointsList[i + iSlow - 1].DeSpiked);
                                }
                            }
                        
                            if (slowRiseTotal >= eventSuddenIncreaseThreshold && eventSuddenIncreaseThreshold > 0) {   
                                suddenIncreaseDifference = slowRiseTotal;
                            }

                        }

                       



                        if (suddenIncreaseDifference >= eventSuddenIncreaseThreshold && eventSuddenIncreaseThreshold > 0 && !sudden_IsRising)//&& position.VehicleSpeed == 0)
                        {
                            //Log (first) instance of sudden raise
                            ////create new event, first instance of increase

                            sudden_IsRising = true;
                            sudden_IsRisingCount += 1;
                            sudden_IsRisingStartingValue = previousReading;
                            sudden_IsRisingStartedRecord = i;

                            //cant be dropping if rising so update these here
                            sudden_IsDroppingCount = 0;
                            sudden_IsDropping = false;
                            sudden_IsDroppingStartingValue = 0;
                        }
                        else if (sudden_IsRising && suddenIncreaseDifference > 0)
                        {
                            //still rising
                            sudden_IsRisingCount += 1;

                            //cant be dropping if rising so update these here
                            sudden_IsDroppingCount = 0;
                            sudden_IsDropping = false;
                            sudden_IsDroppingStartingValue = 0;
                        }
                        else if (sudden_IsRising && suddenIncreaseDifference <= 0)
                        {
                            //stopped rising, so save the event.
                            //but first check if any of the next records are rising just in case

                            //todo this will error if i+6 is greater than .count of current data set, use the fiveahead approach instead
                            //var fiveAhead = cleanedFuelList.AsEnumerable().Take(5);
                            //double increase = 0;
                            //foreach (var itemAhead in fiveAhead)
                            //{
                            //     increase = itemAhead.Avg - item.Avg;
                            // }
                            if (
                                FuelPointsList[i + 1].DeSpiked > currentReading
                                || FuelPointsList[i + 2].DeSpiked > currentReading
                                || FuelPointsList[i + 3].DeSpiked > currentReading
                                || FuelPointsList[i + 4].DeSpiked > currentReading
                                || FuelPointsList[i + 5].DeSpiked > currentReading
                                || FuelPointsList[i + 6].DeSpiked > currentReading
                                )
                            {
                                //still rising so dont mark as end of refill
                                sudden_IsRisingCount += 1;

                                //cant be dropping if rising so update these here
                                sudden_IsDroppingCount = 0;
                                sudden_IsDropping = false;
                                sudden_IsDroppingStartingValue = 0;

                            }
                            else
                            {


                                double episodeRefillValue = currentReading - sudden_IsRisingStartingValue;

                                if (episodeRefillValue >= eventSuddenIncreaseThreshold)
                                {

                                    RefillDrainChunks refillAndDrainsItem = new RefillDrainChunks();



                                    refillAndDrainsItem.EventDateTime = FuelPointsList[sudden_IsRisingStartedRecord].Date;
                                    refillAndDrainsItem.ChangeAmount = episodeRefillValue;
                                    refillAndDrainsItem.IsRefill = true;
                                    refillAndDrainsItem.ReadingsCount = sudden_IsRisingCount;
                                    refillAndDrainsItem.ReadingsStartIndex = sudden_IsRisingStartedRecord;
                                    refillAndDrainsItem.ReadingsEndIndex = i - 1; //todo check if this is right, we may have to read forward more to work this out 
                                    refillAndDrainsItem.ReadingsInitialFuel = sudden_IsRisingStartingValue;
                                    refillAndDrainsItem.ReadingsFinalFuel = currentReading;


                                    // added by rono 
                                    refillAndDrainsItem.Location = FuelPointsList[i].Location;
                                    refillAndDrainsItem.ipkCommanTrackingID = FuelPointsList[i].ipkCommanTrackingID;
                                    refillAndDrainsItem.Lat = FuelPointsList[i].Lat;
                                    refillAndDrainsItem.Lon = FuelPointsList[i].Lon;

                                    refillAndDrainsList.Add(refillAndDrainsItem);

                                    //    //todo here we must add the logic for rono
                                    //    position.EventName = "Sudden Increase of " + episodeRefillValue + " taken over " + sudden_IsRisingCount + " readings";
                                    //position.ReportID = 813;
                                    //position.additionalEventInfo = episodeRefillValue.ToString();
                                    //position.dGPSDateTime = sudden_IsRisingStartedRecord.dGPSDateTime;


                                }
                                else
                                {
                                    //this is where tmc device must have hit and not generated the increase
                                    var x = "";
                                }
                                sudden_IsRising = false;
                                sudden_IsRisingCount = 0;
                                sudden_IsRisingStartingValue = 0;
                            }

                        }
                        ////////////////////////////////////






                        ////////////////////////////////////
                        // NEW check sudden drains
                        ////////////////////////////////////

                        //if (sigmaOfReadings >= eventSuddenDropThreshold && eventSuddenDropThreshold > 0 && !sensorReadings.Sudden_IsDropping)
                        if (suddenDecreaseDifference >= eventSuddenDropThreshold && eventSuddenDropThreshold > 0 && !sudden_IsDropping)//&& position.VehicleSpeed == 0)
                        {
                            //Log (first?) instance of sudden drop
                            //create new event, first instance of increase

                            sudden_IsDropping = true;
                            sudden_IsDroppingCount += 1;
                            sudden_IsDroppingStartingValue = previousReading;
                            sudden_IsDroppingStartedRecord = i;

                            //cant be raising if dropping so update these here
                            sudden_IsRisingCount = 0;
                            sudden_IsRising = false;
                            sudden_IsRisingStartingValue = 0;
                        }
                        else if (sudden_IsDropping && suddenDecreaseDifference > 0)
                        {
                            //still dropping
                            sudden_IsDroppingCount += 1;

                            //cant be raising if dropping so update these here
                            sudden_IsRisingCount = 0;
                            sudden_IsRising = false;
                            sudden_IsRisingStartingValue = 0;
                        }
                        //else if (sudden_IsDropping && (suddenDecreaseDifference <= 0 || (suddenDecreaseDifference <= 1.5 && position.VehicleSpeed > 15))) ///check for speed here to figure out rate of drop
                        else if (sudden_IsDropping && (suddenDecreaseDifference <= 0)) //|| (suddenDecreaseDifference <= 1.5 && position.VehicleSpeed > 15))) ///check for speed here to figure out rate of drop
                        {
                            //stopped dropping

                            double episodeDrainValue = sudden_IsDroppingStartingValue - currentReading;

                            if (episodeDrainValue >= eventSuddenDropThreshold)
                            {
                                //todo here we must add the logic for rono
                                RefillDrainChunks refillAndDrainsItem = new RefillDrainChunks();


                                refillAndDrainsItem.EventDateTime = FuelPointsList[sudden_IsDroppingStartedRecord].Date;
                                refillAndDrainsItem.ChangeAmount = episodeDrainValue;
                                refillAndDrainsItem.IsRefill = false;
                                refillAndDrainsItem.ReadingsCount = sudden_IsDroppingCount;
                                refillAndDrainsItem.ReadingsStartIndex = sudden_IsDroppingStartedRecord;
                                refillAndDrainsItem.ReadingsEndIndex = i - 1; //todo check if this is right, we may have to read forward more to work this out 
                                refillAndDrainsItem.ReadingsInitialFuel = sudden_IsDroppingStartingValue;
                                refillAndDrainsItem.ReadingsFinalFuel = currentReading;


                                // added by rono 
                                refillAndDrainsItem.Location = FuelPointsList[i].Location;
                                refillAndDrainsItem.ipkCommanTrackingID = FuelPointsList[i].ipkCommanTrackingID;
                                refillAndDrainsItem.Lat = FuelPointsList[i].Lat;
                                refillAndDrainsItem.Lon = FuelPointsList[i].Lon;

                                refillAndDrainsList.Add(refillAndDrainsItem);

                                //position.EventName = "Sudden Decrease of " + episodeDrainValue + " taken over " + sudden_IsDroppingCount + " readings";
                                //position.ReportID = 812;
                                //position.additionalEventInfo = episodeDrainValue.ToString();
                                //position.dGPSDateTime = sudden_IsDroppingStartedRecord.dGPSDateTime;



                            }
                            sudden_IsDropping = false;
                            sudden_IsDroppingCount = 0;
                            sudden_IsDroppingStartingValue = 0;


                        }
                        ////////////////////////////////////


                    }

                }
            }
            catch (Exception ex)
            {
                //Helper.WriteMessageToConsoleOrLog($"ProcessSuddenIncreaseDecreaseRange:  ", ex, MessageType.CriticalError, false, true, true);
            }
            finally
            {

            }
            return refillAndDrainsList;
        }

        //public double GetLastIgnitionOnSensorValue(long DeviceId, int SensorId, DateTime gpsDateTime)
        //{

        //    var x = DAL_Fuel.
        //}


        public Hashtable FuelInfo(El_Report _El_Report)
        {


            int Reportid = _El_Report.ifkReportId, UserId = _El_Report.UserId;

            string StrTimeZoneID = _El_Report.TimeZoneID;

            var _DAL_Reports = new DAL_Reports();

            var ds = _DAL_Reports.getAnalogData_FromDb(UserId, Reportid, 2, 0,1);

            var rptFuelModel = new FuelModel();

            var eL_Fuel = new EL_Fuel();


            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                rptFuelModel = new FuelModel
                {

                    Asset = dr["Asset"].ToString(),
                    startDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["startDate"]), StrTimeZoneID).ToString("dd MMM yyyy   HH:mm:ss "),// .ToString("yyyy-MM-dd HH:mm:ss"),
                    endDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["endDate"]), StrTimeZoneID).ToString("dd MMM yyyy   HH:mm:ss "),
                    DateOfQuery = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, StrTimeZoneID).ToString("dd MMM yyyy HH:mm:ss"),
                    Logo = dr["vLogo"].ToString(),
                    ifkDeviceId = Convert.ToInt32(dr["ifkDeviceId"]),
                    SensorType = Convert.ToInt32(dr["SensorType"]),
                    iAnalogNumber = Convert.ToInt32(dr["iAnalogNumber"]),
                    ifkCanBusType = Convert.ToInt32(dr["ifkCanBusType"]),
                    Tracker_Type = Convert.ToInt32(dr["tracker_type"]),

                };

                eL_Fuel = new EL_Fuel
                {
                    ifkAssetID = rptFuelModel.ifkDeviceId,
                    sensorType = rptFuelModel.SensorType,
                    iAnalogNumber = rptFuelModel.iAnalogNumber,
                    canBusType = rptFuelModel.ifkCanBusType,
                    Operation = 3,
                    startDate = Convert.ToString(dr["startDate"]),
                    endDate = Convert.ToString(dr["endDate"]),
                    Tracker_Type = rptFuelModel.Tracker_Type
                };
            }


            eL_Fuel.ifkUserID = _El_Report.UserId;

            eL_Fuel.TimeZoneID = _El_Report.TimeZoneID;

            eL_Fuel.ifkFuelMeasurementUnit = _El_Report.ifkFuelMeasurementUnit;

            var defaults = GetAssetConfigSettings(eL_Fuel);

            var result = GetGraphData_AssetInfo(eL_Fuel);

            var GraphItem = result["GraphItem"] as El_FuelUIObj;

            GraphItem.ifkFuelMeasurementUnit = _El_Report.ifkFuelMeasurementUnit;

            if (!_El_Report.IsExport)
            {
                GraphItem.minValue = defaults.Min;

                var user_max = defaults.Max;

                var _data_max = 0.0;

                if (GraphItem.FuelItems.Any())
                    _data_max = GraphItem.FuelItems.Max(n => n.RF);

                if (_data_max > user_max)
                {
                    GraphItem.maxValue = _data_max + 5;
                }

                if (_data_max < user_max)
                {
                    GraphItem.maxValue = user_max + 5;
                }

                //javascript 60 
                if (_data_max < 60 && user_max < 60)
                {
                    GraphItem.maxValue = 60;
                }



                var fuelList = new List<FuelModel>();

                foreach (var item in GraphItem.FuelItems)
                    fuelList.Add(new FuelModel
                    {
                        Date = item.date,
                        FuelData = item.RF,
                        ReportID = 0,
                        grphColor = "#5CB2DA",
                        AvgFuel = item.AF
                    });




                rptFuelModel.max = GraphItem.maxValue;
                rptFuelModel.min = GraphItem.minValue;
                rptFuelModel.FuelList = fuelList;
            }

            rptFuelModel.Distance = GraphItem.TotalDistance;
            rptFuelModel.Consumption = GraphItem.FuelConsumpt;
            rptFuelModel.Drain = GraphItem.TotalDrains;
            rptFuelModel.Refill = GraphItem.TotalRefills;
            rptFuelModel.FuelUsed = GraphItem.FuelUsed;
            rptFuelModel.TotalEngineHours = GraphItem.TotalEngineHours;

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                if (Convert.ToInt32(item["id"]) == Convert.ToInt32(ds.Tables[2].Rows[0]["AnalogSensorID"]))
                {
                    rptFuelModel.Title = Convert.ToString(item["vName"]);
                    rptFuelModel.analogType = Convert.ToString(item["vUnitText"]);
                    rptFuelModel.analogType = Convert.ToString(item["vUnitText"]);
                }

            }



            var json = JsonConvert.SerializeObject(rptFuelModel, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            result["reportHeader"] = rptFuelModel;

            return result;

        }



        public FuelModel FuelInfo(El_Report _El_Report, EL_FuelAssetItem _EL_FuelAssetItem = null)
        {
            var rptFuelModel = new FuelModel();

            try
            {
                var eL_Fuel = new EL_Fuel();

                rptFuelModel = _EL_FuelAssetItem.FuelModelItem;

                eL_Fuel = _EL_FuelAssetItem.FuelItem;

                eL_Fuel.ifkUserID = _El_Report.UserId;

                eL_Fuel.TimeZoneID = _El_Report.TimeZoneID;

                eL_Fuel.ifkFuelMeasurementUnit = _El_Report.ifkFuelMeasurementUnit;

                var defaults = GetAssetConfigSettings(eL_Fuel);

                var result = GetGraphData_AssetInfo(eL_Fuel);

                var GraphItem = result["GraphItem"] as El_FuelUIObj;


                rptFuelModel.Distance = GraphItem.TotalDistance;
                rptFuelModel.Consumption = GraphItem.FuelConsumpt;
                rptFuelModel.Drain = GraphItem.TotalDrains;
                rptFuelModel.Refill = GraphItem.TotalRefills;
                rptFuelModel.FuelUsed = GraphItem.FuelUsed;
                rptFuelModel.TotalEngineHours = GraphItem.TotalEngineHours;
                rptFuelModel.Device = _EL_FuelAssetItem.DeviceDetailsMaster;


               
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Fuel.cs", "FuelInfo(a,b)", ex.Message  + ex.StackTrace);
            }
            finally
            {

            }
            return rptFuelModel;

        }

    }

    public class IgnitionChunk
    {
        public bool TripIsFinished;
        public List<RpmFuelPoint> Readings { get; set; }
        public DateTime startFueldate { get; set; }
        public DateTime endFueldate { get; set; }
        public double consumption { get; set; }

        public void CalculateConsumption()
        {
            Readings = new List<RpmFuelPoint>();
        }
    }
    public class FuelQueueHelper
    {
        public bool ignitionStatus { get; set; }
        public double CurrentPointValue { get; set; }
        public Queue<double> ReadingsUnitsAveraged { get; set; }
        public int AvgCount { get; set; }
        public Queue<double> IgnitionOnQueue { get; set; }
        public int ignitionQCount { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public DateTime NextStartTime { get; set; }
        public List<double> IgnitionOnAvaraged { get; set; }

        public bool TripFinished { get; set; }

        public bool AllowedToContinue { get; set; }

        public Queue<double> SampleBeforeStart { get; set; }

    }
    public class RpmFuelPoint
    {
        public double AVGFuel;
        public double Fuel;
        public double RPM;
    }




}
