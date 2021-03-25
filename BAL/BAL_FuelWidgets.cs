using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.EntityLayer;
using WLT.DataAccessLayer.DAL;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;
using System.Reflection;

namespace WLT.BusinessLogic.BAL
{
    public class BAL_FuelWidgets
    {

        //public List<El_FuelWidgets> GetHighestServiceCost(El_FuelWidgets El_FuelWidgets)
        //{
        //    DataSet ds = new DataSet();
        //    DAL_FuelWidgets DAL_FuelWidgets = new DAL_FuelWidgets();
        //    ds = DAL_FuelWidgets.GetHighestServiceCost(El_FuelWidgets);

        //    List<El_FuelWidgets> list = new List<El_FuelWidgets>();

        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            list.Add(new El_FuelWidgets { AssetName = Convert.ToString(dr["AssetName"]), DriverName = Convert.ToString(dr["DriverName"]), TotalCost = Convert.ToDouble(dr["TotalCost"]), vLogo = Convert.ToString(dr["vLogo"]) });
        //        }

        //    }

        //    return list;
        //}
        public static Tuple<DateTime, DateTime> calenderTimeHelper(int i, string TimeZoneId)
        {    //beginning Date instantiation
            DateTime dStartDate = new DateTime();
            //Get users Today date
            var tempDate = DateTime.UtcNow;



            DateTime CurrentDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(tempDate, TimeZoneId);
            //Create A starting Date for that month
            if (i == 1 || i == 0)
            {
                i = 1;
                //Default to the First day 1st
                dStartDate = new DateTime(CurrentDate.Year, CurrentDate.Month, i);
            }
            else
            {
                dStartDate = CurrentDate.AddMonths(-(i - 1));
                dStartDate = new DateTime(dStartDate.Year, dStartDate.Month, 1);
            }
            CurrentDate = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, 23, 59, 59);
            dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(dStartDate, TimeZoneId);
            CurrentDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(CurrentDate, TimeZoneId);



            return new Tuple<DateTime, DateTime>(dStartDate, CurrentDate);

        }



        #region new logic
        public El_FuelWidgets GetTopConsumers(El_FuelWidgets El_FuelWidgets, string TimeZoneId)
        {
            var ds = new DataSet();

            var _CalenderHelperObj = new CalenderHelper(El_FuelWidgets.MonthValue, El_FuelWidgets.RangeDateType, TimeZoneId);

            El_FuelWidgets.StartTime = _CalenderHelperObj.utcStartTime;

            El_FuelWidgets.EndDate = _CalenderHelperObj.utcEndTime;

            DAL_FuelWidgets DAL_FuelWidgets = new DAL_FuelWidgets();


            ds = DAL_FuelWidgets.Get_WidgetTopConsumers(El_FuelWidgets);


            var widgetRecordList = new El_FuelWidgets();

            var dsTableCount = 0;

            var lstO_ = new El_FuelWidgets();

            foreach (DataTable dt in ds.Tables)
            {
                if (dsTableCount == 0)
                {
                    var _total_consumed = 00.00;

                    foreach (DataRow dr in dt.Rows)
                    {
                        var ifkAssetId = Convert.ToInt32(dr["AssignedId"]);
                        var vpkDeviceID = Convert.ToInt64(dr["ImeiNumber"]);
                        var ifkDeviceID = Convert.ToInt32(dr["AssignedId"]);
                        var ifkFuelMeasurementUnit = Convert.ToInt32(dr["ifkFuelMeasurementUnit"]);

                        lstO_.FuelUnit = ifkFuelMeasurementUnit == 1 ? "gallons" : "Ltrs";

                        lstO_.ConsFuelUnit = ifkFuelMeasurementUnit == 1 ? "mpg" : ifkFuelMeasurementUnit == 2 ? "kms/Ltr" : "Ltrs/100kms";

                        
                        var rows = ds.Tables[1].Select("ImeiNumber=" + vpkDeviceID + "");

                        var filterByDriver = rows.AsEnumerable().GroupBy(r => r.Field<int?>("ifkDriverID"));

                        try {
                            var filterByDriverDictionary = filterByDriver.ToDictionary(x => x.Key ?? 0, y => y.ToList());

                            foreach (var group in filterByDriverDictionary)
                            {
                                double distance = 0.0;

                                double consumption = 0.0;

                                var fuelUsage = 0.0;

                                var DriverName = "Driver No Assigned ";

                                CalculateFuelUsage(group.Value.ToArray(), ifkFuelMeasurementUnit, ref fuelUsage, ref consumption, ref distance, ref DriverName);



                                lstO_.ListWidgets.Add(new El_FuelWidgets
                                {

                                    AssetName = Convert.ToString(dr["AssetName"]),
                                    FuelConsumed = Math.Round(fuelUsage, 2),
                                    Distance = UserSettings.ConvertKMsToXxOdoMeter(El_FuelWidgets.measurementUnit, distance.ToString(), true, 2),
                                    Econ = Math.Round(consumption, 2).ToString(),
                                    DriverName = DriverName,
                                    vLogo = Convert.ToString(dr["logo"])

                                });

                                _total_consumed += Convert.ToDouble(fuelUsage);
                            }

                        }
                        catch (Exception ex)
                        {
                            var message = ex.Message;
                        }

                       
                    }
                }
                dsTableCount++;
            }



            lstO_.ListWidgets = lstO_.ListWidgets.OrderByDescending(n => n.FuelConsumed).ToList();

            return lstO_;
        }


        public void CalculateFuelUsage(DataRow[] rows, int ifkFuelMeasurementUnit, ref double fuelUsage, ref double consumption, ref double distance, ref string DriverName )
        {


            int SensorType = 0, AnalogType = 0;

            double doubleOut = 0.0;

            var _Refills = 0.0;

            var InnitialFuelValue = 0.0;

            var LastFuelValue = 0.0;

            var InnitialOdometerValue = 0.0;

            var LastOdometerValue = 0.0;


            foreach (DataRow row in rows)
            {

                SensorType = Convert.ToInt32(row["SensorType"]);

                AnalogType = Convert.ToInt32(row["AnalogType"]);

                var rowType = Convert.ToInt32(row["rowType"]);

                DriverName = Convert.ToString(row["DriverName"]);

                var fuel_Value = 0.0;

                if (SensorType == 7 && Convert.ToString(row["rawdata"]) != "")
                {
                    var sensorObj = JsonConvert.DeserializeObject<EL_OBDData>(row["rawdata"].ToString());

                    fuel_Value = sensorObj.FuelL == null || string.IsNullOrEmpty((sensorObj.FuelL)) ? 0.0 : Convert.ToDouble(sensorObj.FuelL.Split(' ')[0]);



                }
                if (SensorType == 1 && Convert.ToString(row["rawdata"]) != "")
                {
                    var sensorObj = JsonConvert.DeserializeObject<EL_AnalogSensorData>(row["rawdata"].ToString());

                    PropertyInfo prop = sensorObj.GetType().GetProperty("An" + AnalogType);

                    fuel_Value = Convert.ToDouble(prop.GetValue(sensorObj, null));

                }

                _Refills += Double.TryParse(Convert.ToString(row["additionalEventInfo"]), out doubleOut) ? Convert.ToDouble(row["additionalEventInfo"]) : 0;

                if (rowType == 1)
                {
                    InnitialFuelValue = fuel_Value;

                    InnitialOdometerValue = Double.TryParse(Convert.ToString(row["vOdometer"]), out doubleOut) ? Convert.ToDouble(row["vOdometer"]) : 0;
                }

                if (rowType == -1)
                {
                    LastFuelValue = fuel_Value;

                    LastOdometerValue = Double.TryParse(Convert.ToString(row["vOdometer"]), out doubleOut) ? Convert.ToDouble(row["vOdometer"]) : 0;
                }


            }


            fuelUsage = _Refills + (InnitialFuelValue - LastFuelValue);

            distance = (LastOdometerValue - InnitialOdometerValue);



            if (ifkFuelMeasurementUnit == 1) //miles per gallon
            {
                var gallons = (fuelUsage * 0.2641720524);

                var miles = (distance * 0.62137);

                consumption = gallons == 0 ? .0 : (miles / gallons);

                fuelUsage = gallons;
            }

            if (ifkFuelMeasurementUnit == 2)  // kms/litre            
                consumption = fuelUsage == 0 ? distance : (distance / fuelUsage);


            if (ifkFuelMeasurementUnit == 3) //Liters per 100kms    
                consumption = (fuelUsage * 100) / distance;






        }


        public El_FuelWidgets GetMostFuelDrains(El_FuelWidgets El_FuelWidgets, string TimeZoneId)
        {
            var ds = new DataSet();

            var _CalenderHelperObj = new CalenderHelper(El_FuelWidgets.MonthValue, El_FuelWidgets.RangeDateType, TimeZoneId);

            El_FuelWidgets.StartTime = _CalenderHelperObj.utcStartTime;

            El_FuelWidgets.EndDate = _CalenderHelperObj.utcEndTime;

            DAL_FuelWidgets DAL_FuelWidgets = new DAL_FuelWidgets();


            ds = DAL_FuelWidgets.Get_WidgetTopFuelDrainers(El_FuelWidgets);


            var widgetRecordList = new El_FuelWidgets();

            var dsTableCount = 0;

            var ifkFuelMeasurementUnit = 0;

            var lstO_ = new El_FuelWidgets();

            foreach (DataTable dt in ds.Tables)
            {
                if (dsTableCount == 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var ifkAssetId = Convert.ToInt32(dr["AssignedId"]);
                        var vpkDeviceID = Convert.ToInt64(dr["ImeiNumber"]);
                        var ifkDeviceID = Convert.ToInt32(dr["AssignedId"]);
                         ifkFuelMeasurementUnit = Convert.ToInt32(dr["ifkFuelMeasurementUnit"]);

                        var rows = ds.Tables[1].Select("ImeiNumber=" + vpkDeviceID + "");

                        var filterByDriver = rows.AsEnumerable().GroupBy(r => r.Field<int?>("ifkDriverID"));

                        try {

                            var filterByDriverDictionary = filterByDriver.ToDictionary(x => x.Key ?? 0, y => y.ToList());

                            foreach (var group in filterByDriverDictionary)
                            {
                                var _rows = group.Value.ToArray();

                                var drains = GetCombinedReading(_rows, ifkFuelMeasurementUnit);

                                lstO_.FuelUnit = ifkFuelMeasurementUnit == 1 ? "gallons" : "Ltrs";

                                lstO_.ConsFuelUnit = ifkFuelMeasurementUnit == 1 ? "mpg" : ifkFuelMeasurementUnit == 2 ? "kms/Ltr" : "Ltrs/100kms";

                                lstO_.ListWidgets.Add(new El_FuelWidgets
                                {
                                    AssetName = Convert.ToString(dr["AssetName"]),
                                    Total = drains,
                                    DriverName = _rows.Count()>0? Convert.ToString(_rows[0]["DriverName"]):"",
                                    vLogo = Convert.ToString(dr["logo"]),
                                    NumberOfDrains = rows.Count()
                                });

                                
                                lstO_.TotalConsumed += drains;
                            }


                        }
                        catch(Exception ex)
                        {

                        }

                    }
                }
                dsTableCount++;
            }

            lstO_.TotalDrained = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, lstO_.TotalConsumed.ToString(),true,2);

            lstO_.ListWidgets = lstO_.ListWidgets.OrderByDescending(n => n.Total).ToList();

            return lstO_;
        }

        public El_FuelWidgets GetFleetRefuels(El_FuelWidgets El_FuelWidgets, string TimeZoneId)
        {

            var ds = new DataSet();

            var _CalenderHelperObj = new CalenderHelper(El_FuelWidgets.MonthValue, El_FuelWidgets.RangeDateType, TimeZoneId);

            El_FuelWidgets.StartTime = _CalenderHelperObj.utcStartTime;

            El_FuelWidgets.EndDate = _CalenderHelperObj.utcEndTime;

            DAL_FuelWidgets DAL_FuelWidgets = new DAL_FuelWidgets();

            // importtant to get devices to loop through 
            El_FuelWidgets.Operation = 0;

            ds = DAL_FuelWidgets.GetFleetRefuelsAndDrains(El_FuelWidgets);

            var widgetRecordList = new El_FuelWidgets();

            var lstO_ = new El_FuelWidgets();

            int ifkFuelMeasurementUnit = 2;

            foreach (DataTable dt in ds.Tables)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    var ifkAssetId = Convert.ToInt32(dr["AssignedId"]);

                    El_FuelWidgets.vpkDeviceID = Convert.ToInt64(dr["ImeiNumber"]);

                    var ifkDeviceID = Convert.ToInt32(dr["AssignedId"]);

                    var SensorType = Convert.ToInt32(dr["SensorType"]);

                    El_FuelWidgets.AnalogueNo = Convert.ToInt32(dr["iAnalogNumber"]);

                    ifkFuelMeasurementUnit = Convert.ToInt32(dr["ifkFuelMeasurementUnit"]);

                    El_FuelWidgets.Operation = 3;      // important as it shares one  data access layer method call

                    var dsRefuels = DAL_FuelWidgets.GetFleetRefuelsAndDrains(El_FuelWidgets);

                    lstO_.FuelUnit = ifkFuelMeasurementUnit == 1 ? "gallons" : "Ltrs";


                    foreach (DataTable _refuelTable in dsRefuels.Tables)
                        foreach (DataRow itemRow in _refuelTable.Rows)
                        {
                            var filled = Convert.ToDouble(itemRow["additionalEventInfo"]);

                            var fuelAfterstr = Convert.ToString(itemRow["FuelAfter"]);

                            var fuelAfter = fuelAfterstr == null || string.IsNullOrEmpty((fuelAfterstr)) ? 0.0 : Convert.ToDouble(fuelAfterstr.Split(' ')[0]);

                            var fuelBeforeRefill = (fuelAfter - filled);

                            lstO_.Total += filled;

                            try
                            {
                                lstO_.ListWidgets.Add(new El_FuelWidgets
                                {
                                    AssetName = Convert.ToString(dr["AssetName"]),
                                    InitialFuel = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, fuelBeforeRefill.ToString(), true, 2),
                                    FinalFuel = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, fuelAfter.ToString(), true, 2),
                                    WhenRefueled = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(itemRow["dGpsDatetime"]), TimeZoneId).ToString("dd MMM yy HH:mm:ss"),
                                    DriverName = Convert.ToString(itemRow["DriverName"]),
                                    vLogo = Convert.ToString(dr["logo"]),
                                    Filled = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, filled.ToString(), true, 2),
                                    Location = Convert.ToString(itemRow["vTextMessage"]),
                                });
                            }
                            catch (Exception ex)
                            {
                                var message = ex.Message;
                            }

                        }

                }


            }

            lstO_.TotalFilled = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, lstO_.Total.ToString(), true, 2);

            lstO_.ListWidgets = lstO_.ListWidgets.OrderByDescending(n => n.WhenRefueled).ToList();

            return lstO_;
        }

        public El_FuelWidgets GetFleetDrains(El_FuelWidgets El_FuelWidgets, string TimeZoneId)
        {

            var ds = new DataSet();

            var _CalenderHelperObj = new CalenderHelper(El_FuelWidgets.MonthValue, El_FuelWidgets.RangeDateType, TimeZoneId);

            El_FuelWidgets.StartTime = _CalenderHelperObj.utcStartTime;

            El_FuelWidgets.EndDate = _CalenderHelperObj.utcEndTime;

            DAL_FuelWidgets DAL_FuelWidgets = new DAL_FuelWidgets();

            // importtant to get devices to loop through 
            El_FuelWidgets.Operation = 0;

            ds = DAL_FuelWidgets.GetFleetRefuelsAndDrains(El_FuelWidgets);

            var widgetRecordList = new El_FuelWidgets();

            var lstO_ = new El_FuelWidgets();

            int ifkFuelMeasurementUnit = 2;

            foreach (DataTable dt in ds.Tables)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    var ifkAssetId = Convert.ToInt32(dr["AssignedId"]);

                    El_FuelWidgets.vpkDeviceID = Convert.ToInt64(dr["ImeiNumber"]);

                    var ifkDeviceID = Convert.ToInt32(dr["AssignedId"]);

                    var SensorType = Convert.ToInt32(dr["SensorType"]);

                    El_FuelWidgets.AnalogueNo = Convert.ToInt32(dr["iAnalogNumber"]);

                    ifkFuelMeasurementUnit = Convert.ToInt32(dr["ifkFuelMeasurementUnit"]);

                    El_FuelWidgets.Operation = 4;  // important as it shares one  data access layer method call

                    var dsRefuels = DAL_FuelWidgets.GetFleetRefuelsAndDrains(El_FuelWidgets);

                    lstO_.FuelUnit = ifkFuelMeasurementUnit == 1 ? "gallons" : "Ltrs";

                    lstO_.ConsFuelUnit = ifkFuelMeasurementUnit == 1 ? "mpg" : ifkFuelMeasurementUnit == 2 ? "kms/Ltr" : "Ltrs/100kms";

                    foreach (DataTable _refuelTable in dsRefuels.Tables)
                        foreach (DataRow itemRow in _refuelTable.Rows)
                        {
                            var drained = Convert.ToDouble(itemRow["additionalEventInfo"]);

                            var fuelAfterstr = Convert.ToString(itemRow["FuelAfter"]);

                            var fuelAfter = fuelAfterstr == null || string.IsNullOrEmpty((fuelAfterstr)) ? 0.0 : Convert.ToDouble(fuelAfterstr.Split(' ')[0]);

                            var FuelBeforeDrain = (fuelAfter + drained);

                            lstO_.Total += drained;

                            try
                            {
                                lstO_.ListWidgets.Add(new El_FuelWidgets
                                {
                                    AssetName = Convert.ToString(dr["AssetName"]),
                                    InitialFuel = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, FuelBeforeDrain.ToString(), true, 2),
                                    FinalFuel = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, fuelAfter.ToString(), true, 2),
                                    WhenDrained = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(itemRow["dGpsDatetime"]), TimeZoneId).ToString("dd MMM yy HH:mm:ss"),
                                    DriverName = Convert.ToString(itemRow["DriverName"]),
                                    vLogo = Convert.ToString(dr["logo"]),
                                    Drained = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, drained.ToString(), true, 2),
                                    Location = Convert.ToString(itemRow["vTextMessage"]),
                                });
                            }
                            catch (Exception ex)
                            {
                                var message = ex.Message;
                            }

                        }

                }


            }

            lstO_.TotalDrained = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, lstO_.Total.ToString(), true, 2);

            lstO_.ListWidgets = lstO_.ListWidgets.OrderByDescending(n => n.WhenDrained).ToList();

            return lstO_;
        }


        public El_FuelWidgets GetFleetFuelStats(El_FuelWidgets El_FuelWidgets, string TimeZoneId)
        {
            var ds = new DataSet();

            var _CalenderHelperObj = new CalenderHelper(El_FuelWidgets.MonthValue, El_FuelWidgets.RangeDateType, TimeZoneId);

            El_FuelWidgets.StartTime = _CalenderHelperObj.utcStartTime;

            El_FuelWidgets.EndDate = _CalenderHelperObj.utcEndTime;

            DAL_FuelWidgets DAL_FuelWidgets = new DAL_FuelWidgets();

            ds = DAL_FuelWidgets.Get_WidgetFuelStats(El_FuelWidgets);

            var lstO_ = new El_FuelWidgets();

            int ifkFuelMeasurementUnit = 2;

            var tableCount = 0;

            double doubleOut, overalDistance = 0.0;

            double AlldevicesRefillSummed = .0, AllDevicesDrainsSummed = .0;

            foreach (DataTable dt in ds.Tables)
            {
                if (tableCount == 0)
                    foreach (DataRow dr in dt.Rows)
                    {
                        var ifkAssetId = Convert.ToInt32(dr["AssignedId"]);

                        El_FuelWidgets.vpkDeviceID = Convert.ToInt64(dr["ImeiNumber"]);

                        var ifkDeviceID = Convert.ToInt32(dr["AssignedId"]);

                        var SensorType = Convert.ToInt32(dr["SensorType"]);

                        El_FuelWidgets.AnalogueNo = Convert.ToInt32(dr["iAnalogNumber"]);

                        ifkFuelMeasurementUnit = Convert.ToInt32(dr["ifkFuelMeasurementUnit"]);


                        var rows = ds.Tables[1].Select("ImeiNumber=" + El_FuelWidgets.vpkDeviceID + "");

                        double firstOdoemeterReading = .0, lastOdometerReading = .0, totalDeviceRefills = .0, totalDeviceDrain = .0;

                        foreach (var row in rows)
                        {
                            var rowType = Convert.ToInt32(row["RowType"]);

                            if (rowType == 0)  //The first lower bound of the  date range 
                            {
                                if (Convert.ToInt32(row["vReportID"]) == 813)
                                {
                                    totalDeviceRefills += Double.TryParse(Convert.ToString(row["additionalEventInfo"]), out doubleOut) ? Convert.ToDouble(row["additionalEventInfo"]) : 0;
                                    lstO_.NumberOfFills += 1;
                                }
                                if (Convert.ToInt32(row["vReportID"]) == 812)
                                {
                                    totalDeviceDrain += Double.TryParse(Convert.ToString(row["additionalEventInfo"]), out doubleOut) ? Convert.ToDouble(row["additionalEventInfo"]) : 0;
                                    lstO_.NumberOfDrains += 1;
                                }
                            }

                            if (rowType == 1)  //The first lower bound of the  date range                         
                                firstOdoemeterReading = Double.TryParse(Convert.ToString(row["vOdometer"]), out doubleOut) ? Convert.ToDouble(row["vOdometer"]) : 0;

                            if (rowType == -1)  //The first lower bound of the  date range                         
                                lastOdometerReading = Double.TryParse(Convert.ToString(row["vOdometer"]), out doubleOut) ? Convert.ToDouble(row["vOdometer"]) : 0;

                        }


                        overalDistance += (lastOdometerReading - firstOdoemeterReading);

                        AlldevicesRefillSummed += totalDeviceRefills;

                        AllDevicesDrainsSummed += totalDeviceDrain;

                    }



                tableCount++;

            }

            lstO_.TotalDrained = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, AllDevicesDrainsSummed.ToString(), true, 2);
            lstO_.TotalFilled = UserSettings.ConvertLitersToXx(ifkFuelMeasurementUnit, AlldevicesRefillSummed.ToString(), true, 2);
            lstO_.TotalDistance = UserSettings.ConvertKMsToXxOdoMeter(El_FuelWidgets.measurementUnit, overalDistance.ToString(), true, 2);

            return lstO_;
        }


        public El_FuelWidgets GetMostEfficient(El_FuelWidgets El_FuelWidgets, string TimeZoneId)
        {
            var ds = new DataSet();

            var _CalenderHelperObj = new CalenderHelper(El_FuelWidgets.MonthValue, El_FuelWidgets.RangeDateType, TimeZoneId);

            El_FuelWidgets.StartTime = _CalenderHelperObj.utcStartTime;

            El_FuelWidgets.EndDate = _CalenderHelperObj.utcEndTime;

            DAL_FuelWidgets DAL_FuelWidgets = new DAL_FuelWidgets();


            ds = DAL_FuelWidgets.Get_WidgetMostEfficientConsumers(El_FuelWidgets);

            var widgetRecordList = new El_FuelWidgets();

            var dsTableCount = 0;

            var lstO_ = new El_FuelWidgets();

            foreach (DataTable dt in ds.Tables)
            {
                if (dsTableCount == 0)
                {
                    var _total_consumed = 00.00;

                    foreach (DataRow dr in dt.Rows)
                    {
                        var ifkAssetId = Convert.ToInt32(dr["AssignedId"]);
                        var vpkDeviceID = Convert.ToInt64(dr["ImeiNumber"]);
                        var ifkDeviceID = Convert.ToInt32(dr["AssignedId"]);
                        var ifkFuelMeasurementUnit = Convert.ToInt32(dr["ifkFuelMeasurementUnit"]);
                       

                        var rows = ds.Tables[1].Select("ImeiNumber=" + vpkDeviceID + "");

                        var filterByDriver = rows.AsEnumerable().GroupBy(r => r.Field<int?>("ifkDriverID"));

                        try
                        {
                            var filterByDriverDictionary = filterByDriver.ToDictionary(x => x.Key ?? 0, y => y.ToList());

                            foreach( var group in filterByDriverDictionary)
                            {
                                double distance = 0.0;

                                double consumption = 0.0;

                                var fuelUsage = 0.0;

                                var DriverName = "No driver assigned";

                                CalculateFuelUsage(rows, ifkFuelMeasurementUnit, ref fuelUsage, ref consumption, ref distance, ref DriverName);

                                lstO_.FuelUnit = ifkFuelMeasurementUnit == 1 ? "gallons" : "Ltrs";

                                lstO_.ConsFuelUnit = ifkFuelMeasurementUnit == 1 ? "mpg" : ifkFuelMeasurementUnit == 2 ? "kms/Ltr" : "Ltrs/100kms";

                                lstO_.ListWidgets.Add(new El_FuelWidgets
                                {

                                    AssetName = Convert.ToString(dr["AssetName"]),
                                    FuelConsumed = Math.Round(fuelUsage, 2),
                                    Distance = UserSettings.ConvertKMsToXxOdoMeter(El_FuelWidgets.measurementUnit, distance.ToString(), true, 2),
                                    Econ = Math.Round(consumption, 2).ToString(),
                                    DriverName = DriverName,
                                    vLogo = Convert.ToString(dr["logo"]),
                                    Consumption = Math.Round(consumption, 2)

                                });
                                _total_consumed += Convert.ToDouble(fuelUsage);
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                                               
                    }
                }
                dsTableCount++;
            }



            lstO_.ListWidgets = lstO_.ListWidgets.OrderBy(n => n.Consumption).ToList();

            return lstO_;
        }

        public double GetCombinedReading(DataRow[] rows, int ifkFuelMeasurementUnit)
        {
            var _Readings = 0.0;

            var _double = 0.0;

            foreach (var row in rows)
            {
                _Readings += Double.TryParse(Convert.ToString(row["additionalEventInfo"]), out _double) ? Convert.ToDouble(row["additionalEventInfo"]) : 0;

            }

            if (ifkFuelMeasurementUnit == 1) //miles per gallon
            {
                var gallons = (_Readings * 0.2641720524);

                _Readings = gallons;
            }


            return Math.Round(_Readings, 2);
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

        #endregion
    }


    public class CalenderHelper
    {

        public DateTime utcStartTime { get; set; }
        public DateTime utcEndTime { get; set; }

        public CalenderHelper()
        {
        }

        public CalenderHelper(int timeIdentifier, int Replacer, string timeZoneId)
        {
            var userTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, timeZoneId);

            var UserEndDate = new DateTime(userTime.Year, userTime.Month, userTime.Day, 23, 59, 59);

            if (timeIdentifier == -2) // today
            {
                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date, timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);

            }

            if (timeIdentifier == -1) // yesterday 
            {
                var yesterdayEnd = UserEndDate.Date.AddDays(-1);

                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(yesterdayEnd.Date, timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(yesterdayEnd, timeZoneId);
            }

            if (timeIdentifier == 1) // this week // with monday as the first day 
            {
                var firstWeekDayUserTime = GetFirstDayOfWeek(UserEndDate, System.Globalization.CultureInfo.CurrentCulture);

                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(firstWeekDayUserTime.Date, timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
            }

            if (timeIdentifier == 0) // last 7 days 
            {

                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date.AddDays(-6), timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
            }

            if (timeIdentifier == 2) // previous week 
            {
                var firstWeekDayUserTime = GetFirstDayOfWeek(UserEndDate, System.Globalization.CultureInfo.CurrentCulture);

                var previousWeekStartTime = GetFirstDayOfWeek(firstWeekDayUserTime.AddSeconds(-1), System.Globalization.CultureInfo.CurrentCulture);

                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(previousWeekStartTime.Date, timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(firstWeekDayUserTime.AddSeconds(-1), timeZoneId);
            }

            if (timeIdentifier == 3) // last  14 days  
            {
                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date.AddDays(-13), timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
            }

            if (timeIdentifier == 4) // last  30  days  
            {
                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date.AddDays(-30), timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
            }
            if (timeIdentifier == 5) // current Month
            {
                var dateFirst = new DateTime(UserEndDate.Year, UserEndDate.Month, 1);

                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(dateFirst, timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
            }

            if (timeIdentifier == 6) // previous month
            {
                var lastMonthEnd = new DateTime(UserEndDate.Year, UserEndDate.Month, 1).AddSeconds(-1);

                var dateFirstPrevious = new DateTime(lastMonthEnd.Year, lastMonthEnd.Month, 1);

                utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(dateFirstPrevious, timeZoneId);

                utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(lastMonthEnd, timeZoneId);
            }


            if (Replacer == 1)
            {
                if (timeIdentifier == 0)  // today 
                {
                    utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date, timeZoneId);

                    utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
                }

                if (timeIdentifier == 1)  // last 24 hours  
                {
                    utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date, timeZoneId);

                    utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
                }

                if (timeIdentifier == 3)  // last 3 Days  
                {
                    utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date.AddDays(-2), timeZoneId);

                    utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
                }

                if (timeIdentifier == 7)  // last 1 week   
                {
                    utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date.AddDays(-6), timeZoneId);

                    utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
                }
                if (timeIdentifier == 14)  // last 2 week   
                {
                    utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date.AddDays(-13), timeZoneId);

                    utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
                }
                if (timeIdentifier == 21)  // last 21 days    
                {
                    utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date.AddDays(-20), timeZoneId);

                    utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
                }
                if (timeIdentifier == 28)  // last 28 days    
                {
                    utcStartTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate.Date.AddDays(-27), timeZoneId);

                    utcEndTime = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(UserEndDate, timeZoneId);
                }

            }
        }
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, System.Globalization.CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }

    }
}
