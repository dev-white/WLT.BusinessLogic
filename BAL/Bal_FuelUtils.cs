using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.EntityLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
   public static class Bal_FuelUtils
    {
        public  static double? GetFuelValue(string json_value, int sensorType, int type)
        {
            double? FuelValue = 0.0;

         try
            {
                if (sensorType != 7 && json_value != string.Empty)
                {
                    var elAnalogSensor = JsonConvert.DeserializeObject<EL_AnalogSensorData>(json_value);

                    if (sensorType == 1)
                    {

                        if (type == 1)
                            FuelValue = elAnalogSensor.An1;


                        if (type == 2)
                            FuelValue = elAnalogSensor.An2;

                        if (type == 3)
                            FuelValue = elAnalogSensor.An3;


                        if (type == 4)
                            FuelValue = elAnalogSensor.An4;

                        if (type == 5)
                            FuelValue = elAnalogSensor.An5;

                        if (type == 6)
                            FuelValue = elAnalogSensor.An6;


                    }

                    if (sensorType == 2)
                    {
                        FuelValue = type == 1 ? elAnalogSensor.S1 : elAnalogSensor.S2;
                    }
                    if (sensorType == 3)
                    {
                        //FuelValue = elAnalogSensor.OneW;
                    }
                }
                else if (sensorType == 7 && json_value != string.Empty)
                {
                    var canBus = JsonConvert.DeserializeObject<EL_OBDData>(json_value);

                    if ((type == 1 || type == 4) &&
                        !string.IsNullOrEmpty(canBus.FuelL))
                    {
                        FuelValue = Convert.ToDouble(canBus.FuelL.Split(' ')[0]);
                    }

                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_FuelUtils.cs", "GetFuelValue", "", ex.Message + ex.StackTrace + ex.InnerException.Message);

            }

            return Convert.ToDouble(FuelValue);

        }

        public static double? GetFuelValue(DataRow row, int sensorType, int type)
        {
            var json_value = string.Empty;

            double? FuelValue = 0.0;

            try
            {
                if (sensorType != 7 && Convert.ToString(row["SensorData"]) != string.Empty)
                {
                    json_value = Convert.ToString(row["SensorData"]);

                    var elAnalogSensor = JsonConvert.DeserializeObject<EL_AnalogSensorData>(json_value);

                    if (sensorType == 1)
                    {
                        if (type == 1)
                            FuelValue = elAnalogSensor.An1;


                        if (type == 2)
                            FuelValue = elAnalogSensor.An2;

                        if (type == 3)
                            FuelValue = elAnalogSensor.An3;


                        if (type == 4)
                            FuelValue = elAnalogSensor.An4;

                        if (type == 5)
                            FuelValue = elAnalogSensor.An5;

                        if (type == 6)
                            FuelValue = elAnalogSensor.An6;
                    }

                    if (sensorType == 2)
                    {
                        FuelValue = type == 1 ? elAnalogSensor.S1 : elAnalogSensor.S2;
                    }
                    if (sensorType == 3)
                    {
                        //FuelValue = elAnalogSensor.OneW;
                    }
                }
                else if (sensorType == 7 && Convert.ToString(row["ObdData"]) != string.Empty)
                {
                    json_value = Convert.ToString(row["ObdData"]);

                    var canBus = JsonConvert.DeserializeObject<EL_OBDData>(json_value);

                    if ((type == 1 || type == 4) &&
                        !string.IsNullOrEmpty(canBus.FuelL))
                    {
                        FuelValue = Convert.ToDouble(canBus.FuelL.Split(' ')[0]);
                    }

                }
            }
          
            catch ( Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_FuelUtils.cs", "GetFuelValue", "", ex.Message + ex.StackTrace + ex.InnerException.Message);

            }

            return FuelValue;

        }
    }
}
