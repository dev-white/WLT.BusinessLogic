using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace WLT.BusinessLogic
{
    public static class UserSettings
    {
        
        public static String ConvertLocalDateTimeToUTCDateTime(DateTime utcTime, string localTimeZone)
        {

            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, localTimeZone, "UTC");

            string x = localTime.ToString("yyyy/MM/dd HH:mm:ss");
            return localTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static DateTime ConvertLocalDateTimeToUTCDateTime_DateFormat(DateTime utcTime, string localTimeZone)
        {          
            DateTime   localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(utcTime.Ticks, DateTimeKind.Unspecified), localTimeZone, "UTC");
                            
            return localTime;
        }
        public static String ConvertUTCDateTimeToLocalDateTime(DateTime utcTime, string localTimeZone)       {          
           

            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, "UTC", localTimeZone);

            return localTime.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public static String ConvertUTCDateTimeToProperLocalDateTime(DateTime utcTime, string localTimeZone)
        {
             

            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, "UTC", localTimeZone);

            return localTime.ToString("d MMM yy HH:mm"); 
        }
        public static String ConvertUTCDateTimeToProperLocalDateTimeLong(DateTime utcTime, string localTimeZone)
        {


            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, "UTC", localTimeZone);
            return localTime.ToString("dd/MM/yy HH:mm:ss");
          
        }

        public static String ConvertUTCDateTimeToLocalTime(DateTime utcTime, string localTimeZone)
        {


            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, "UTC", localTimeZone);

            return localTime.ToString("HH:mm:ss");
        }


        public static String ConvertUTCDateTimeToShortDate(DateTime utcTime, string localTimeZone)
        {


            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, "UTC", localTimeZone);

            return localTime.ToString("dd MMMM yyyy");
        }

        public static String ConvertUTCDateTimeToShortLocalDateTime(DateTime utcTime, string localTimeZone)
        {


            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(utcTime, "UTC", localTimeZone);

            return localTime.ToString("dd/MM/yyyy");
        }
        public static DateTime ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime utcTime, string localTimeZone)
        {
            DateTime localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(new DateTime(utcTime.Ticks, DateTimeKind.Unspecified), "UTC", localTimeZone);
            
            return localTime;
        }
        public static String ConvertKMsToXx(int? SpeedTypeId, string Kilometers, bool appendUnitName, int decimalPointsToUse)
        {
            if (Kilometers == "")
            {
                Kilometers = "0";
            }
            double kilometers1 = Convert.ToDouble(Kilometers, CultureInfo.InvariantCulture);
           
            if (kilometers1 >= 0)
            {
                string unitName = "";
                if (appendUnitName)
                {
                    unitName = GetSpeedUnitName(SpeedTypeId);
                }

                if (SpeedTypeId == 1)
                {   //Kilometers
                    decimal Kmh = decimal.Round(Convert.ToDecimal(Kilometers), decimalPointsToUse);
                    return Kmh + unitName;
                }
                else if (SpeedTypeId == 2)
                {   //MPH
                    kilometers1 = kilometers1 * 0.62137;
                    decimal Mph = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Mph) + unitName);

                }
                else if (SpeedTypeId == 3)
                {   //Knots
                    kilometers1 = kilometers1 * 0.539956803;
                    decimal Knots = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Knots) + unitName);
                }
                else { return Kilometers + unitName; }
            }
            else
            {
                return Convert.ToString(decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse));
            }

        }

        public static String ConvertKMsToXx(int? SpeedTypeId, string Kilometers, bool appendUnitName)
        {
            if (Kilometers == "")
            {
                Kilometers = "0";
            }
            double kilometers1 = Convert.ToDouble(Kilometers, CultureInfo.InvariantCulture);

            if (kilometers1 >= 0)
            {
                string unitName = "";
                if (appendUnitName)
                {
                    unitName = GetSpeedUnitName(SpeedTypeId);
                }

                if (SpeedTypeId == 1)
                {   //Kilometers
                    decimal Kmh = Convert.ToDecimal(Kilometers);
                    return Kmh + unitName;
                }
                else if (SpeedTypeId == 2)
                {   //MPH
                    kilometers1 = kilometers1 * 0.62137;
                    decimal Mph = Convert.ToDecimal(kilometers1);
                    return (Convert.ToString(Mph) + unitName);

                }
                else if (SpeedTypeId == 3)
                {   //Knots
                    kilometers1 = kilometers1 * 0.539956803;
                    decimal Knots = Convert.ToDecimal(kilometers1);
                    return (Convert.ToString(Knots) + unitName);
                }
                else { return Kilometers + unitName; }
            }
            else
            {
                return Kilometers;
            }

        }

        public static double ConvertUnitTovalues(int? UnitType ) {
           
            //miles
             if (UnitType == 2)
            {
                return 0.62137;
            }
             //knots
            else if (UnitType == 3)
            {
                return 0.539956803;
            }
             //kms (e.t.c)
            else
            {
                return 1;
            }
                            
        }
        public static String ConvertXxToKms(int SpeedTypeId, string Kilometers, bool appendUnitName, int decimalPointsToUse)
        {
            if (Kilometers == "")
            {
                Kilometers = "0";
            }
            double kilometers1 = Convert.ToDouble(Kilometers, CultureInfo.InvariantCulture);

            if (kilometers1 >= 0)
            {
                string unitName = "";
                if (appendUnitName)
                {
                    unitName = GetSpeedUnitName(SpeedTypeId);
                }

                if (SpeedTypeId == 1)
                {   //Kilometers
                    decimal Kmh = decimal.Round(Convert.ToDecimal(Kilometers), decimalPointsToUse);
                    return Kmh + unitName;
                }
                else if (SpeedTypeId == 2)
                {   //MPH
                    kilometers1 = kilometers1 / 0.62137;
                    decimal Mph = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Mph) + unitName);

                }
                else if (SpeedTypeId == 3)
                {   //Knots
                    kilometers1 = kilometers1 / 0.539956803;
                    decimal Knots = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Knots) + unitName);
                }
                else { return Kilometers + unitName; }
            }
            else
            {
                return Convert.ToString(decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse));
            }

        }

        public static String GetSpeedUnitName(int? SpeedTypeId)
        {
            if (SpeedTypeId == 1)
            {   //Kilometers
                return " km/h";
            }
            else if (SpeedTypeId == 2)
            {   //MPH
                return " mph";
            }
            else if (SpeedTypeId == 3)
            {   //Knots
                return " knots";
            }
            else { return " km/h"; }
        }

        public static String GetOdometerUnitName(int? UnitTypeId)
        {
            if (UnitTypeId == 1)
            {   //Kilometers
                return " km";
            }
            else if (UnitTypeId == 2)
            {   //MPH
                return " miles";
            }
            else if (UnitTypeId == 3)
            {   //Knots
                return " NM";
            }
            else { return " km"; }
        }

        public static String ConvertToKMs(int SpeedTypeId, string Kilometers, bool appendUnitName, int decimalPointsToUse)
        {
            if (Kilometers == "")
            {
                Kilometers = "0";
            }
            double kilometers1 = Convert.ToDouble(Kilometers, CultureInfo.InvariantCulture);

            if (kilometers1 >= 0)
            {
                string unitName = "";
                if (appendUnitName)
                {
                    unitName = GetSpeedUnitName(SpeedTypeId);
                }

                if (SpeedTypeId == 1)
                {   //Kilometers
                    decimal Kmh = decimal.Round(Convert.ToDecimal(Kilometers), decimalPointsToUse);
                    return Kmh + unitName;
                }
                else if (SpeedTypeId == 2)
                {   //MPH
                    kilometers1 = kilometers1 * 0.62137;
                    decimal Mph = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Mph) + unitName);

                }
                else if (SpeedTypeId == 3)
                {   //Knots
                    kilometers1 = kilometers1 * 0.539956803;
                    decimal Knots = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Knots) + unitName);
                }
                else { return Kilometers + unitName; }
            }
            else
            {
                return Convert.ToString(decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse));
            }

        }

     
        public static String ConvertKMsToxxCategory(int measurementsUnit, string FirstValue,string  SecondVale)
        {      
            string  Category  ="";

            int mUnit = Convert.ToInt32(measurementsUnit);

            string First = ConvertKMsToXx(mUnit, FirstValue, false, 0);

            if (SecondVale == "-1")
            {

              //  Category = First + " - Over" + GetSpeedUnitName(mUnit);
                Category = First + " - Over" ;
            }
            else
            {
                string Second = ConvertKMsToXx(mUnit, SecondVale, false, 0);

                //Category = First + " - " + Second + GetSpeedUnitName(mUnit);
                Category = First + " - " + Second ;
            }


            return Category;
        }
      

        public static String ConvertKMsToXxOdoMeter(int? SpeedTypeId, string Kilometers, bool appendUnitName, int decimalPointsToUse)
        {
            if (string.IsNullOrEmpty(Kilometers))
            {
               
                Kilometers = "0";
            }

            if (SpeedTypeId == null)
            {
                SpeedTypeId = 1;

            }

            double kilometers1 = Convert.ToDouble(Kilometers, CultureInfo.InvariantCulture);

            if (kilometers1 >= 0)
            {
                string unitName = "";
                if (appendUnitName)
                {
                    unitName = GetOdometerUnitName(SpeedTypeId);
                }

                if (SpeedTypeId == 1)
                {   //Kilometers
                    decimal Kmh = decimal.Round(Convert.ToDecimal(Kilometers), decimalPointsToUse);
                    return Kmh + unitName;
                }
                else if (SpeedTypeId == 2)
                {   //MPH
                    kilometers1 = kilometers1 * 0.62137;
                    decimal Mph = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Mph) + unitName);

                }
                else if (SpeedTypeId == 3)
                {   //Knots
                    kilometers1 = kilometers1 * 0.539956803;
                    decimal Knots = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Knots) + unitName);
                }
                else { return Kilometers + unitName; }
            }
            else
            {
                return Convert.ToString(decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse));
            }

        }

        public static String ConvertKMsToXxOdoMeterNegative(int? SpeedTypeId, string Kilometers, bool appendUnitName, int decimalPointsToUse)
        {
            if (string.IsNullOrEmpty(Kilometers))
            {

                Kilometers = "0";
            }

            if (SpeedTypeId == null)
            {
                SpeedTypeId = 1;

            }

               double kilometers1 = Convert.ToDouble(Kilometers, CultureInfo.InvariantCulture);

          
                string unitName = "";

                if (appendUnitName)
                {
                    unitName = GetOdometerUnitName(SpeedTypeId);
                }

                if (SpeedTypeId == 1)
                {   //Kilometers
                    decimal Kmh = decimal.Round(Convert.ToDecimal(Kilometers), decimalPointsToUse);
                    return Kmh + unitName;
                }
                else if (SpeedTypeId == 2)
                {   //MPH
                    kilometers1 = kilometers1 * 0.62137;
                    decimal Mph = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Mph) + unitName);

                }
                else if (SpeedTypeId == 3)
                {   //Knots
                    kilometers1 = kilometers1 * 0.539956803;
                    decimal Knots = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                    return (Convert.ToString(Knots) + unitName);
                }
                else
               { return Kilometers + unitName; }
          

        }

        public static String Get_MPG(int MeasurementID, string Kilometers, bool appendUnitName, int decimalPointsToUse)
        {
          
                if (Kilometers == "")
                {
                    Kilometers = "0";
                }
                double kilometers1 = Convert.ToDouble(Kilometers, CultureInfo.InvariantCulture);

                if (kilometers1 >= 0)
                {
                    string unitName = "";
                    if (appendUnitName)
                    {
                        unitName = GetOdometerUnitName(MeasurementID);
                    }

                    if (MeasurementID == 1)
                    {   //Kilometers
                        decimal Kmh = decimal.Round(Convert.ToDecimal(Kilometers), decimalPointsToUse);
                        return Kmh + unitName + "/Litre";
                    }
                    else if (MeasurementID == 2)
                    {   //MPH
                        kilometers1 = kilometers1 * 0.62137;
                        decimal Mph = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                        return (Convert.ToString(Mph) + unitName + "/Litre");

                    }
                    else if (MeasurementID == 3)
                    {   //Knots
                        kilometers1 = kilometers1 * 0.539956803;
                        decimal Knots = decimal.Round(Convert.ToDecimal(kilometers1), decimalPointsToUse);
                        return (Convert.ToString(Knots) + unitName + "/Litre");
                    }
                    else { return Kilometers + unitName + "/Litre"; }
                }
                else
                {
                    return Kilometers + "/Litre";
                }

          
        }


        public static string  GetFuelUnit (int ifkFuelMeasurementUnit)
        {
            if (ifkFuelMeasurementUnit == 1) // miles/gallon
               return   " Gallons";
            else 
                return " Liters";
        }
        public static string GetFuelConsumption(int ifkFuelMeasurementUnit)
        {
            if (ifkFuelMeasurementUnit == 1) // miles/gallon
                return " MPG";
            if (ifkFuelMeasurementUnit == 3) // miles/gallon
                return " L/100KM";
            else
                return " KM/L";
        }
        public static string ConvertFuelUnitXX(int ifkFuelMeasurementUnit,  double  fuel_value, bool appendUnitName, int decimalPointsToUse)
        {
            var returnString = "";

            if (ifkFuelMeasurementUnit == 1) // miles            
                fuel_value *= 0.264172;

            fuel_value = Math.Round(fuel_value, decimalPointsToUse);

            if (appendUnitName)
                returnString = $" { fuel_value } {GetFuelUnit(ifkFuelMeasurementUnit)} ";

            return returnString;
        }

        public static double ConvertFuelToXX(int ifkFuelMeasurementUnit, double fuel_value)
        {
            

            if (ifkFuelMeasurementUnit == 1) // Gallons            
                fuel_value *= 0.264172;
            

            return fuel_value;
        }
        public static string ConvertLitersToXx(int? MeasurementID , string Liters , bool appendUnitName, int decimalPointsToUse)
        {
            string unitName = "";
            
            if (string.IsNullOrEmpty(Liters))
            {
                Liters = "0";
            }
            var Liters1 = Convert.ToDouble(Liters);

            if (MeasurementID == null)
            {
                MeasurementID = 2;
            }

            if (MeasurementID == 1)
            {
                var gallons = Liters1 * 0.264172;

                unitName = " Gallons";

                Liters1 = Math.Round(gallons, decimalPointsToUse);
            }
            else
            {
                unitName = " Liters";

                Liters1 = Math.Round(Liters1, decimalPointsToUse);

            }

            if (!appendUnitName) unitName = "";


            return $"{Liters1}{unitName}";

        }


        public static double ConvertRoadSpeedKilometersToMiles(double kilometers)
        {
            switch (kilometers)
            {
                case 24:
                    return 15;
                case 32:
                    return 20;
                case 40:
                    return 25;
                case 48:
                    return 30;
                case 49:
                    return 30;
                case 50:
                    return 30;
                case 64:
                    return 40;
                case 72:
                    return 45;
                case 80:
                    return 50;
                case 89:
                    return 55;
                case 97:
                    return 60;
                case 100:
                    return 60;
                case 113:
                    return 70;
                case 116:
                    return 70;
                case 121:
                    return 75;
                case 129:
                    return 80;
                case 130:
                    return 80;
                case 137:
                    return 80;
                default:
                    return (kilometers * 0.621371192);
            }

        }
    }
}
