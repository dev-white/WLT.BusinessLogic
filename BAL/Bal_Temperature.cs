using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using WLT.DataAccessLayer;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_Temperature
    {

        string f_strConnectionString = AppConfiguration.Getwlt_WebAppConnectionString();
        public Hashtable GetTemperatureReportInfo_Original(El_Report _El_Report)
        {
            var ds = new DataSet();

            var reportDetailsData = new Hashtable();

            try
            {
                SqlParameter[] param = new SqlParameter[3];


                param[0] = new SqlParameter("@ipkReportId", SqlDbType.BigInt);
                param[0].Value = _El_Report.ReportId;

                param[1] = new SqlParameter("@reportTypeId", SqlDbType.BigInt);
                param[1].Value = _El_Report.ReportTypeID;

                param[2] = new SqlParameter("@UserId", SqlDbType.BigInt);
                param[2].Value = _El_Report.UserId;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_report_temperature", param);

                var _table_index = 0;

                var criteria = new EL_Temperatue();

                var _lstEL_Temperatue = new List<EL_Temperatue>();

                foreach (DataTable dt in ds.Tables)
                {


                    if (_table_index == 0)  /// for the graph
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            criteria.sensor_type = Convert.ToInt32(dr["SensorType"]);

                            criteria.analog_type = Convert.ToInt32(dr["AnalogType"]);

                            criteria.unit_text = Convert.ToString(dr["vUnitText"]);

                            criteria.min = Convert.ToInt32(dr["iMinValue"] == DBNull.Value ? 0 : dr["iMinValue"]);

                            criteria.max = Convert.ToInt32(dr["iMaxValue"] == DBNull.Value ? 100 : dr["iMaxValue"]);
                        }

                    }

                    if (_table_index == 1)  /// this body section of reports 
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            //{"OneW":[{"V":"28.900000000000002","Id":"3531125814275320360"}]}

                            if (!string.IsNullOrEmpty(Convert.ToString(dr["sensordata"])))
                            {                            
                                var analog_reading = new EL_AnalogSensorData();

                                try
                                {
                                    analog_reading = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(dr["sensordata"]));
                                }
                                catch (Exception ex)
                                {

                                }


                                double temp_reading = 0.0;

                                if (analog_reading != null && criteria.analog_type == 2)
                                {
                                    if (analog_reading.OneW != null)
                                        foreach (var item in analog_reading.OneW)
                                            temp_reading = Math.Round(Convert.ToDouble(item.V), 2);


                                }

                                var local_date = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["dgpsdatetime"]), _El_Report.TimeZoneID);

                                var event_id = int.TryParse(Convert.ToString(dr["vreportid"]), out _) ? Convert.ToInt32(dr["vreportid"]) : 0;

                                var additionalEventInfo = double.TryParse(Convert.ToString(dr["additionaleventinfo"]), out _) ? Convert.ToDouble(dr["additionaleventinfo"]) : 0;


                                _lstEL_Temperatue.Add(new EL_Temperatue
                                {
                                    reading = temp_reading,
                                    date = local_date,
                                    location = Convert.ToString(dr["vtextmessage"]),
                                    event_id = event_id,
                                    event_amount = event_id == 812 ? additionalEventInfo : event_id == 813 ? additionalEventInfo : 0,

                                }
                                ); ;
                            }

                        }

                    }

                    if (_table_index == 2)  /// this header section details 
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _El_Report.ReportName = Convert.ToString(dr["vreportname"]);
                            _El_Report.GeneratedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _El_Report.TimeZoneID);
                            _El_Report.dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["firstdate"]), _El_Report.TimeZoneID);
                            _El_Report.dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["lastdate"]), _El_Report.TimeZoneID);
                            _El_Report.intMeasurementUnit = Convert.ToInt32(dr["measurement_code"]);
                            _El_Report.AssetName = Convert.ToString(dr["vasset"]);
                            _El_Report.CompanyLogo = Convert.ToString(dr["vlogo"]);
                        }

                    }

                    _table_index++;
                }

                reportDetailsData.Add("header", _El_Report);


                reportDetailsData.Add("body_content", _lstEL_Temperatue);


            }
            catch (Exception ex)
            {
             LogError.RegisterErrorInLogFile("GetTemperatureReportInfo", "Bal_Temperature()", ex.Message  + ex.StackTrace);

            }

            return reportDetailsData;
        }


        public Hashtable GetTemperatureReportInfo(El_Report _El_Report)
        {
            var ds = new DataSet();

            var reportDetailsData = new Hashtable();

            try
            {
                SqlParameter[] param = new SqlParameter[3];


                param[0] = new SqlParameter("@ipkReportId", SqlDbType.BigInt);
                param[0].Value = _El_Report.ReportId;

                param[1] = new SqlParameter("@reportTypeId", SqlDbType.BigInt);
                param[1].Value = _El_Report.ReportTypeID;

                param[2] = new SqlParameter("@UserId", SqlDbType.BigInt);
                param[2].Value = _El_Report.UserId;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_report_temperature", param);

                var _table_index = 0;

                var eL_Fuellst = new List<EL_Fuel>();

                var criteria = new EL_Temperatue();

                   var lstEL_Temperatue =   new List<EL_Temperatue>();

                foreach (DataTable dt in ds.Tables)
                {
                    if (_table_index == 0)  /// for the graph
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            criteria.sensor_type = Convert.ToInt32(dr["SensorType"]);
                            criteria.analog_type = Convert.ToInt32(dr["AnalogType"]);
                            criteria.unit_text = Convert.ToString(dr["vUnitText"]);
                            criteria.min = Convert.ToInt32(dr["iMinValue"] == DBNull.Value ? 0 : dr["iMinValue"]);
                            criteria.max = Convert.ToInt32(dr["iMaxValue"] == DBNull.Value ? 100 : dr["iMaxValue"]);


                            var eL_Fuel = new EL_Fuel();

                            eL_Fuel.iAnalogNumber = Convert.ToInt32(dr["iAnalogNumber"]);
                            eL_Fuel.sensorType = Convert.ToInt32(dr["SensorType"]);
                            eL_Fuel.canBusType = Convert.ToInt32(dr["ifkCanBusType"]);
                            eL_Fuel.oneWireID = Convert.ToString(dr["DeviceId1Wire"]);
                            eL_Fuel.MappedName = Convert.ToString(dr["vName"]);
                            eL_Fuel.AnalogType = Convert.ToInt32(dr["AnalogType"]);

                            eL_Fuellst.Add(eL_Fuel);


                        }

                    }

                    if (_table_index == 1)  /// this body section of reports 
                    {
                        //loop throug mappings 

                        try
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                if (string.IsNullOrEmpty(Convert.ToString(dr["sensordata"]))
                                    && string.IsNullOrEmpty(Convert.ToString(dr["ObdData"])))
                                    continue;



                                var _tempReport = new EL_Temperatue();

                                var event_id = int.TryParse(Convert.ToString(dr["vreportid"]), out _) ? Convert.ToInt32(dr["vreportid"]) : 0;


                                foreach (var item in eL_Fuellst)
                                {
                                    var additionalEventInfo = double.TryParse(Convert.ToString(dr["additionaleventinfo"]), out _) ? Convert.ToDouble(dr["additionaleventinfo"]) : 0;

                                 
                                    var _TValue = new TValue();

                                  
                                    var Attributes =   JsonConvert.DeserializeObject<TType>(Convert.ToString(dr["Attributes"]));

                                    if (Attributes is { })                                    
                                        if (Attributes.Type== item.iAnalogNumber)
                                        {
                                            if (event_id == 812 || event_id == 813)
                                                _TValue.Event_Amount = Math.Round(additionalEventInfo,2);
                                        

                                        }


                                    _TValue.Value = DecipherValue(item, dr) ?? 0.0; 

                                    _TValue.Event_Id = event_id;                                   

                                    _tempReport.TemprVl.Add(Regex.Replace(item.MappedName, @"\s+", "_"), _TValue);
                                }

                                var local_date = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["dgpsdatetime"]), _El_Report.TimeZoneID);                                                              
                             
                                _tempReport.reading = 0.0;

                                _tempReport.date = local_date;

                                _tempReport.location = Convert.ToString(dr["vtextmessage"]);

                                lstEL_Temperatue.Add(_tempReport);
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    if (_table_index == 2)  /// this header section details 
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _El_Report.ReportName = Convert.ToString(dr["vreportname"]);
                            _El_Report.GeneratedDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _El_Report.TimeZoneID);
                            _El_Report.dStartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["firstdate"]), _El_Report.TimeZoneID);
                            _El_Report.dEndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["lastdate"]), _El_Report.TimeZoneID);
                            _El_Report.intMeasurementUnit = Convert.ToInt32(dr["measurement_code"]);
                            _El_Report.AssetName = Convert.ToString(dr["vasset"]);
                            _El_Report.CompanyLogo = Convert.ToString(dr["vlogo"]);
                        }

                    }

                    _table_index++;
                }

                reportDetailsData.Add("header", _El_Report);


                reportDetailsData.Add("body_content", lstEL_Temperatue);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("GetTemperatureReportInfo", "Bal_Temperature()", ex.Message  + ex.StackTrace);

            }

            return reportDetailsData;
        }

        public Hashtable GetTemperatureDashboardInfo(El_Report _El_Report)
        {
            var ds = new DataSet();

            var reportDetailsData = new Hashtable();

            try
            {
                SqlParameter[] param = new SqlParameter[6];


                param[0] = new SqlParameter("@clientid", SqlDbType.BigInt);
                param[0].Value = _El_Report.ClientId;

                param[1] = new SqlParameter("@vassetlist_csv", SqlDbType.VarChar);
                param[1].Value = _El_Report.asset_id;

                param[2] = new SqlParameter("@master_mapping_id_csv", SqlDbType.VarChar);
                param[2].Value = _El_Report.IO_Mapping_CSV;

                param[3] = new SqlParameter("@dstartdate", SqlDbType.DateTime);
                param[3].Value = _El_Report.dStartDate.ToString("yyyy-MM-dd HH:mm:ss");

                param[4] = new SqlParameter("@denddate", SqlDbType.DateTime);
                param[4].Value = _El_Report.dEndDate.ToString("yyyy-MM-dd HH:mm:ss");


                param[5] = new SqlParameter("@MapAllTemprSnsrs", SqlDbType.Int);
                param[5].Value = _El_Report.MapAllTemprSnsrs;


                ds = SqlHelper.ExecuteDataset(f_strConnectionString, CommandType.StoredProcedure, "sp_dashboard_temperature", param);

                var _table_index = 0;

                var eL_Fuellst = new List<EL_Fuel>();

                var criteria = new EL_Temperatue();

                var lstEL_Temperatue = new List<EL_Temperatue>();

                foreach (DataTable dt in ds.Tables)
                {
                    if (_table_index == 0)  /// for the graph
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            criteria.sensor_type = Convert.ToInt32(dr["SensorType"]);
                            criteria.analog_type = Convert.ToInt32(dr["AnalogType"]);
                            criteria.unit_text = Convert.ToString(dr["vUnitText"]);
                            criteria.min = Convert.ToInt32(dr["iMinValue"] == DBNull.Value ? 0 : dr["iMinValue"]);
                            criteria.max = Convert.ToInt32(dr["iMaxValue"] == DBNull.Value ? 100 : dr["iMaxValue"]);


                            var eL_Fuel = new EL_Fuel();

                            eL_Fuel.iAnalogNumber = Convert.ToInt32(dr["iAnalogNumber"]);
                            eL_Fuel.sensorType = Convert.ToInt32(dr["SensorType"]);
                            eL_Fuel.canBusType = Convert.ToInt32(dr["ifkCanBusType"]);
                            eL_Fuel.oneWireID = Convert.ToString(dr["DeviceId1Wire"]);
                            eL_Fuel.MappedName = Convert.ToString(dr["vName"]);
                            eL_Fuel.AnalogType = Convert.ToInt32(dr["AnalogType"]);

                            eL_Fuellst.Add(eL_Fuel);


                        }

                    }

                    if (_table_index == 1)  /// this body section of reports 
                    {
                        //loop throug mappings 

                        try
                        {
                            foreach (DataRow dr in dt.Rows)
                            {

                                if (string.IsNullOrEmpty(Convert.ToString(dr["sensordata"]))
                                    && string.IsNullOrEmpty(Convert.ToString(dr["ObdData"])))
                                    continue;



                                var _tempReport = new EL_Temperatue();

                                var event_id = int.TryParse(Convert.ToString(dr["vreportid"]), out _) ? Convert.ToInt32(dr["vreportid"]) : 0;


                                foreach (var item in eL_Fuellst)
                                {
                                    var additionalEventInfo = double.TryParse(Convert.ToString(dr["additionaleventinfo"]), out _) ? Convert.ToDouble(dr["additionaleventinfo"]) : 0;


                                    var _TValue = new TValue();


                                    var Attributes = JsonConvert.DeserializeObject<TType>(Convert.ToString(dr["Attributes"]));

                                    if (Attributes is { })
                                        if (Attributes.Type == item.iAnalogNumber)
                                        {
                                            if (event_id == 812 || event_id == 813)
                                                _TValue.Event_Amount = Math.Round(additionalEventInfo, 2);


                                        }

                                    _TValue.Value = DecipherValue(item, dr) ?? 0.0;

                                    _TValue.Event_Id = event_id;

                                    _tempReport.TemprVl.Add(Regex.Replace(item.MappedName, @"\s+", "_"), _TValue);
                                }

                                var local_date = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dr["dgpsdatetime"]), _El_Report.TimeZoneID);

                                _tempReport.reading = 0.0;

                                _tempReport.date = local_date;

                                _tempReport.location = Convert.ToString(dr["vtextmessage"]);

                                lstEL_Temperatue.Add(_tempReport);
                            }

                        }
                        catch (Exception ex)
                        {

                        }

                    }                

                    _table_index++;
                }             


                reportDetailsData.Add("body_content", lstEL_Temperatue);


            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("GetTemperatureReportInfo", "Bal_Temperature()", ex.Message  + ex.StackTrace);

            }

            return reportDetailsData;
        }


        public double? DecipherValue(EL_Fuel _EL_Fuel , DataRow drSource)
        {

            int iAnalogNumber = _EL_Fuel.iAnalogNumber, SensorType = _EL_Fuel.sensorType;


            double? returnValue = 0.0;

            if (SensorType == 1)
            {
                var strSensordata = Convert.ToString(drSource["sensordata"]);

                if (string.IsNullOrEmpty(strSensordata))                
                    return returnValue;

              var   analog_reading = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(strSensordata));

                if (iAnalogNumber == 1)
                {  //"Analog Input 1";
                    returnValue = analog_reading.An1;
                }
                else if (iAnalogNumber == 2)
                {
                    returnValue = analog_reading.An2;
                }
                else if (iAnalogNumber == 3)
                {
                    returnValue = analog_reading.An3;
                }
                else if (iAnalogNumber == 4)
                {
                    returnValue = analog_reading.An4;
                }
                else if (iAnalogNumber == 5)
                {
                    returnValue = analog_reading.An5;
                }
                else if (iAnalogNumber == 6)
                {
                    returnValue = analog_reading.An6;
                }
            }
            else if (SensorType == 2)
            {

                var strSensordata = Convert.ToString(drSource["sensordata"]);

                if (string.IsNullOrEmpty(strSensordata))
                    return returnValue;

                var analog_reading = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(strSensordata));
                // "Serial ";
                if (iAnalogNumber == 1)
                {
                    returnValue = analog_reading.S1;
                }
                else if (iAnalogNumber == 2)
                {
                    returnValue = analog_reading.S2;
                }
            }
            else if (SensorType == 3)
            {
                //"1 Wire";
                var strSensordata = Convert.ToString(drSource["sensordata"]);

                if (string.IsNullOrEmpty(strSensordata))
                    return returnValue;

                var analog_reading = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(strSensordata));

                if (analog_reading.OneW != null)
                {
                    var OneWireValue = analog_reading.OneW.FirstOrDefault(p => p.Id == _EL_Fuel.oneWireID);

                    if (OneWireValue is { })
                        returnValue = Math.Round(Convert.ToDouble(OneWireValue.V), 2);
                    else returnValue = 0.0;
                }                       
                
            }
            else if (SensorType == 4)
            {
                // "Bluetooth Temperature

                var strSensordata = Convert.ToString(drSource["sensordata"]);

                if (string.IsNullOrEmpty(strSensordata))
                    return returnValue;


                var analog_reading = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(strSensordata));

                if (iAnalogNumber == 1)
                {
                    //"Bluetooth Temperature 1";
                    returnValue = analog_reading.B1t;
                }
                else if (iAnalogNumber == 2)
                {
                    // "Bluetooth Temperature 2";
                    returnValue =  analog_reading.B2t;
                }
                else if (iAnalogNumber == 3)
                {
                    //"Bluetooth Temperature 3";
                    returnValue  = analog_reading.B3t;
                }
                else if (iAnalogNumber == 4)
                {
                    // "Bluetooth Temperature 4";
                    returnValue  = analog_reading.B4t;
                }

            }
            else if (SensorType == 5)
            {

                // "Bluetooth Humidity

                var strSensordata = Convert.ToString(drSource["sensordata"]);

                if (string.IsNullOrEmpty(strSensordata))
                    return returnValue;


                var analog_reading = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(strSensordata));


                if (iAnalogNumber == 1)
                {
                    returnValue = analog_reading.B1h;
                }
                else if (iAnalogNumber == 2)
                {
                    returnValue = analog_reading.B2h;
                }
                else if (iAnalogNumber == 3)
                {
                    returnValue = analog_reading.B3h;
                }
                else if (iAnalogNumber == 4)
                {
                    returnValue = analog_reading.B4h;
                }
            }
            else if (SensorType == 6)
            {
               // "Bluetooth Battery ";

                var strSensordata = Convert.ToString(drSource["sensordata"]);

                if (string.IsNullOrEmpty(strSensordata))
                    return returnValue;


                var analog_reading = JsonConvert.DeserializeObject<EL_AnalogSensorData>(Convert.ToString(strSensordata));


                if (iAnalogNumber == 1)
                {
                    //returnValue = analog_reading.bt;
                }
                else if (iAnalogNumber == 2)
                {
                  //  returnValue = "Bluetooth Battery 2";
                }
                else if (iAnalogNumber == 3)
                {
                  //  returnValue = "Bluetooth Battery 3";
                }
                else if (iAnalogNumber == 4)
                {
                   // returnValue = "Bluetooth Battery 4";
                }
            }
            else if (SensorType == 7)
            {

                //"CANBUS Data";
                var strObdData = Convert.ToString(drSource["ObdData"]);    

                var canBus = JsonConvert.DeserializeObject<EL_OBDData>(strObdData);

                if ((iAnalogNumber == 1 || iAnalogNumber == 4) &&
                    !string.IsNullOrEmpty( Convert.ToString(canBus.B1t)))
                {
                    returnValue = Convert.ToDouble(canBus.B1t);
                    
                }
               
            }


            return returnValue;
        }

    }
}
