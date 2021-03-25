using System;
using System.Collections.Generic;
using WLT.DataAccessLayer.DAL;
using Newtonsoft.Json;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.BAL
{
    public class Bal_CurfewViolationReport
    {

        List<clsTrips> Trips { get; set; }
        public DateTime StartDate { get; set; }

        public string TimeZoneID { get; set; }


        public List<clsTrips> ViolatingTrips { get; set; }
        public  DateTime EndDate { get; set; }

       public  Bal_CurfewViolationReport(List<clsPopulateTripSummary> __trips)
        {           
            Trips = JsonConvert.DeserializeObject<List<clsTrips>>(JsonConvert.SerializeObject(__trips));
            ViolatingTrips = new List<clsTrips>();
        }
        public static IEnumerable<Tuple<DateTime, DateTime>> SplitDateRange(DateTime start, DateTime end, int dayChunkSize)
        {
            DateTime chunkEnd;
            while ((chunkEnd = start.AddDays(dayChunkSize)) < end)
            {
                yield return Tuple.Create(start, chunkEnd.AddSeconds(-1));
                start = chunkEnd;
            }
            yield return Tuple.Create(start, end);
        }
       
        public List<clsTrips>  GetAllCurfewViolationTrips(string _curfewTimeStart, string _curfewTimeEnd, int dfd = 0)
        {
            var Days = SplitDateRange(StartDate,EndDate,1);

            var a = DateTime.ParseExact(_curfewTimeStart, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);

            var b = DateTime.ParseExact(_curfewTimeEnd, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                      

            foreach (var Day in Days)
            {

                var CurfewStart = Day.Item1.Date.AddHours(a.Hour).AddMinutes(a.Minute).AddSeconds(a.Second);

                var CurfewEnd = Day.Item1.Date.AddHours(b.Hour).AddMinutes(b.Minute).AddSeconds(b.Second);

                //var _ValidTrips = Trips.Where(n => n.StartDate >= CurfewStart && n.EndDate <= CurfewEnd);

                // ViolatingTrips = Trips.Where(p => !_ValidTrips.Any(p2 => p2.IdStart == p.IdStart)).ToList();                       
                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat( CurfewStart,TimeZoneID),UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat( CurfewEnd,TimeZoneID), Trips[0].VpkDeviceID);
                              

                foreach (var trip in Trips)
                {
                    
                    if (CurfewStart > trip.StartDate)
                    {
                        if (trip.EndDate >= CurfewStart) // check if end date is greater than start curfew time
                        {
                            trip.ExcessTimeBeforeCurfew = (CurfewStart - trip.StartDate).TotalSeconds;

                            trip.ExcessDistanceBeforeCurfew = Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]) - trip.StartOdometer;
                            
                        }
                        else
                        {
                            trip.ExcessTimeBeforeCurfew = trip.Duration.TotalSeconds;

                            trip.ExcessDistanceBeforeCurfew = trip.Distance;
                        }

                      
                    }
                                       

                    if (trip.EndDate > CurfewEnd)
                    {
                        if (trip.StartDate <= CurfewEnd)
                        {
                            trip.ExcessTimeAfterCurfew = (trip.EndDate - CurfewEnd).TotalSeconds;
                            
                            trip.ExcessDistanceAfterCurfew = trip.EndOdometer - Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]);                            
                        }
                        else
                        {
                            trip.ExcessTimeAfterCurfew = trip.Duration.TotalSeconds;

                            trip.ExcessDistanceBeforeCurfew = trip.Distance;
                        }
                      
                    }



                    if (trip.ExcessTimeBeforeCurfew > 0 || trip.ExcessTimeAfterCurfew >0 )
                      ViolatingTrips.Add(trip);


                    
                }

            }
            return  ViolatingTrips;


        }

        public List<clsTrips> GetAllCurfewViolationTrips(string _curfewTimeStart, string _curfewTimeEnd)
        {
            var Days = SplitDateRange(StartDate, EndDate, 1);

            var a = DateTime.ParseExact(_curfewTimeStart, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);

            var b = DateTime.ParseExact(_curfewTimeEnd, "H:mm:ss", null, System.Globalization.DateTimeStyles.None);


            foreach (var Day in Days)
            {

                var CurfewStart = Day.Item1.Date.AddHours(a.Hour).AddMinutes(a.Minute).AddSeconds(a.Second);

                var CurfewEnd = Day.Item1.Date.AddHours(b.Hour).AddMinutes(b.Minute).AddSeconds(b.Second);

                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(CurfewStart, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(CurfewEnd, TimeZoneID), Trips[0].VpkDeviceID);

            

                var _tripCount = Trips.Count;

                if (__details.Tables.Count > 0 && __details.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < _tripCount; i++)
                    {

                        //check if this trip falls in the middle of time range;
                        if (Trips[i].StartDate >= CurfewStart && Trips[i].EndDate <= CurfewEnd || !isWithinTodaysBounds(Trips[i], Day.Item1.Date))
                            continue;

                        // this trip has its start date violating the lower limit curfew time 
                        if (Trips[i].StartDate < CurfewStart)
                        {
                            //we need to get the odometer and start date at that start date // after which we calculate the distance and duration
                            Trips[i].ExcessTimeBeforeCurfew = (CurfewStart - Trips[i].StartDate).TotalSeconds;
                            Trips[i].ExcessDistanceBeforeCurfew = Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]) - Trips[i].StartOdometer;
                            Trips[i].CurfewViolationTypeStart = 1;
                            Trips[i].CurfewViolationMessage = "Trip started outside of allowed hours \n  Trip ended inside of allowed hours";

                        }

                        // this trip has its enddate violating the upper limit curfew time
                        if (Trips[i].EndDate > CurfewEnd)
                        {
                            //we need to get the odometer and start date at that end date // after which we calculate the distance and duration
                            Trips[i].ExcessTimeAfterCurfew = (Trips[i].EndDate - CurfewEnd).TotalSeconds;
                            Trips[i].ExcessDistanceAfterCurfew = Trips[i].EndOdometer - Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]);
                            Trips[i].CurfewViolationTypeEnd = 1;
                            Trips[i].CurfewViolationMessage = "Trip started inside of allowed hours  \n Trip ended outside of allowed hours";

                        }


                        // this option is for a scenerio where we  all dates in a trip are on curfew/Meaning they all are violated/This trip streches the curfew period
                        if (Trips[i].StartDate < CurfewStart && Trips[i].EndDate > CurfewEnd)
                        {
                            // get the odometer of the ignition on  for the curfew date then calculate the distance
                            Trips[i].ExcessTimeBeforeCurfew = (CurfewStart - Trips[i].StartDate).TotalSeconds;

                            Trips[i].ExcessDistanceBeforeCurfew = Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]) - Trips[i].StartOdometer;


                            // get the odometer of the ignition off  for the curfew date then calculate the distance
                            Trips[i].ExcessTimeAfterCurfew = (Trips[i].EndDate - CurfewEnd).TotalSeconds;

                            Trips[i].ExcessDistanceAfterCurfew = Trips[i].EndOdometer - Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]);

                            Trips[i].CurfewViolationTypeStart = 1;

                            Trips[i].CurfewViolationTypeEnd = 1;

                            Trips[i].CurfewViolationMessage = "Trip started outside of allowed hours  \n Trip ended outside of allowed hours";
                        }



                        // this option is for a scenerio where we  all dates in a trip are on curfew/Meaning they all are violated/This trip does not stretch the curfew period
                        //trips are before working hours
                        if ((Trips[i].StartDate < CurfewStart && Trips[i].EndDate < CurfewStart))
                        {
                            //use the objects duration and distance as the time and distance  difference respectively
                            Trips[i].CurfewViolationMessage = "Trip started outside of allowed hours  \n Trip ended outside of allowed hours";

                            Trips[i].CurfewViolationTypeStart = 1;
                            Trips[i].CurfewViolationTypeEnd = 1;


                            Trips[i].ExcessTimeBeforeCurfew = Trips[i].Duration.TotalSeconds;
                            Trips[i].ExcessDistanceBeforeCurfew = Trips[i].Distance;
                        }
                        //trips are after working hours
                        if (Trips[i].StartDate > CurfewEnd && Trips[i].EndDate > CurfewEnd)
                        {

                            //use the objects duration and distance as the time and distance  difference respectively
                            Trips[i].CurfewViolationTypeStart = 1;
                            Trips[i].CurfewViolationTypeEnd = 1;
                            Trips[i].CurfewViolationMessage = "Trip started outside of allowed hours  \n Trip ended outside of allowed hours";

                            Trips[i].ExcessTimeAfterCurfew = Trips[i].Duration.TotalSeconds;
                            Trips[i].ExcessDistanceAfterCurfew = Trips[i].Distance;
                        }

                        ViolatingTrips.Add(Trips[i]);
                    }
                }
            }
            return ViolatingTrips;


        }
        private bool isWithinTodaysBounds(clsTrips _trip, DateTime _Date)
        {
            if (_trip.StartDate.Date == _Date || _trip.StartDate.Date == _Date)
                return true;

            return false;
        }
    }
   

    public class clsTrips : clsPopulateTripSummary
    {
        public double ExcessTimeBeforeCurfew { get; set; } // seconds
        public double ExcessTimeAfterCurfew { get; set; }  // seconds


        public double ExcessDistanceBeforeCurfew { get; set; }

        public double ExcessDistanceAfterCurfew { get; set; }

        public int CurfewViolationTypeStart { get; set; }
        public int CurfewViolationTypeEnd { get; set; }

        public string CurfewViolationMessage { get; set; }

        public clsTrips(){

            }

    }
}
