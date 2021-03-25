using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
   public class Bal_BasicDailyOverviewReport
    {
        public string TimeZoneID { get; set; }
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
        //        public List<clsPopulateTripSummary> FilterPerDay(List<clsPopulateTripSummary> finalArray, DateTime start, DateTime end, int dayChunkSize)
        //        {
        //            var filtered = new List<clsPopulateTripSummary>();

        //            var Days = SplitDateRange(start, end, 1);

        //            var _DAL_BasicOverviewReport = new DAL_BasicOverviewReport();

        //            var vpkDeviceID = finalArray[0].VpkDeviceID; 

        //            try
        //            {

        //                foreach (var Day in Days)
        //                {
        //                    var startTimeUTC = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(Day.Item1, TimeZoneID);
        //                    var endTimeUTC = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(Day.Item2, TimeZoneID);

        //                    List<clsPopulateTripSummary> TripsInBetween = new List<clsPopulateTripSummary>(),
        //                        TripsBeginningYesterday = new List<clsPopulateTripSummary>(),
        //                        TripsOverflowingToNextDay  = new List<clsPopulateTripSummary>();

        //                    // filter this list to specific days based on their overlap 
        //                      TripsInBetween = finalArray.Where(n => n.StartDate >= Day.Item1 && n.EndDate <= Day.Item2).ToList();
        //                      TripsBeginningYesterday = finalArray.Where(n => n.StartDate < Day.Item1 && n.EndDate.Date == Day.Item2.Date).ToList();
        //                      TripsOverflowingToNextDay = finalArray.Where(n => n.EndDate > Day.Item2 && n.StartDate.Date == Day.Item1.Date).ToList();

        //                    var _trip = new clsPopulateTripSummary();

        //                    var _duration = 0.0;

        //                    var _totalDistance = 0.0;

        //                    var noOfTripsBetween = TripsInBetween.Count();

        //                    var noOfTripsYesterday = TripsBeginningYesterday.Count();

        //                    var noOfTripsNextDay = TripsOverflowingToNextDay.Count();

        //                    if (noOfTripsBetween != 0 ||
        //                        noOfTripsYesterday != 0 ||
        //                        noOfTripsNextDay != 0)
        //                    {

        //                        _duration += TripsInBetween.Sum(n => n.Duration.TotalMilliseconds);

        //                        _totalDistance += TripsInBetween.Sum(n => n.Distance);

        //                        if (noOfTripsYesterday == 0) // no trip  that started yesteday so default to today's first trip 
        //                        {
        //                            _trip = TripsInBetween.FirstOrDefault();
        //                        }

        //                        if (noOfTripsNextDay == 0)  // mark this as the last trip of that day
        //                        {
        //                            var lastTrip = TripsInBetween.LastOrDefault();

        //                            if (lastTrip == null)  // no trips in between the day
        //                            {                             
        //                                if (noOfTripsYesterday>0)
        //                                {
        //                                    var dt = _DAL_BasicOverviewReport.BasicDailyOverViewReportFetch(1, startTimeUTC, endTimeUTC, vpkDeviceID).Tables[0];

        //                                    if (dt.Rows.Count > 0)
        //                                    {
        //                                        _trip.StartDate = Day.Item1;
        //                                        _trip.StartOdometer = Convert.ToDouble(dt.Rows[0]["vOdometer"]);
        //                                        _trip.LocationStart = Convert.ToString(dt.Rows[0]["vTextMessage"]);
        //                                        _trip.IdStart = Convert.ToInt64(dt.Rows[0]["ipkCommanTrackingID"]);



        //                                        var diffBetweenOrinalandCurrentStartTime = _trip.EndDate - _trip.StartDate;
        //                                        var mileageDiff = _trip.EndOdometer - _trip.StartOdometer;


        //                                        _trip.Duration = diffBetweenOrinalandCurrentStartTime;
        //                                        _trip.Distance = mileageDiff;
        //                                    }
        //                                    else
        //                                    {
        //                                        continue;
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                _trip.EndDate = lastTrip.EndDate;
        //                                _trip.EndOdometer = lastTrip.EndOdometer;
        //                                _trip.LocationEnd = lastTrip.LocationEnd;
        //                                _trip.IdEnd = lastTrip.IdEnd;
        //                                _trip.Duration = TimeSpan.FromMilliseconds(_duration);
        //                                _trip.Distance = _totalDistance;
        //                            }
        //                        }

        //                        foreach (var Trip in TripsBeginningYesterday)
        //                        {

        //                            var dt = _DAL_BasicOverviewReport.BasicDailyOverViewReportFetch(1, startTimeUTC, Trip.EndDateUtc, Trip.VpkDeviceID).Tables[0];

        //                            if (dt.Rows.Count > 0)
        //                            {
        //                                _trip.StartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dt.Rows[0]["dgpsDateTime"]), TimeZoneID);
        //                                _trip.StartOdometer = Convert.ToDouble(dt.Rows[0]["vOdometer"]);
        //                                _trip.LocationStart = Convert.ToString(dt.Rows[0]["vTextMessage"]);
        //                                _trip.IdStart = Convert.ToInt64(dt.Rows[0]["ipkCommanTrackingID"]);
        //                                _trip.VpkDeviceID = Trip.VpkDeviceID;


        //                                var diffBetweenOrinalandCurrentStartTime = _trip.StartDate - Trip.StartDate;
        //                                var mileageDiff = Trip.EndOdometer - _trip.StartOdometer;


        //                                _trip.Duration = TimeSpan.FromMilliseconds(_duration) + (Trip.Duration - diffBetweenOrinalandCurrentStartTime);

        //                                _trip.Distance = _totalDistance + mileageDiff;

        //                            }
        //                            else
        //                            {



        //                                _trip.StartDate = _trip.EndDate.Date;
        //                                _trip.StartOdometer = _trip.StartOdometer;
        //                                _trip.LocationStart = " Unavailable ";
        //                                _trip.IdStart = _trip.IdEnd;
        //                                _trip.VpkDeviceID = Trip.VpkDeviceID;

        //                                _trip.Duration = TimeSpan.FromMilliseconds(0);
        //                                _trip.Distance = 0;

        //                            }
        //                        }

        //                        foreach (var Trip in TripsOverflowingToNextDay)
        //                        {
        //                            var dt = _DAL_BasicOverviewReport.BasicDailyOverViewReportFetch(1, Trip.StartDateUtc, endTimeUTC, Trip.VpkDeviceID).Tables[0];

        //var                         _tripWasNull = false;

        //                            if (_trip == null)
        //                            {
        //                                _trip = Trip; //make this this trip default
        //                                _tripWasNull = true;
        //                            }

        //                            if (dt.Rows.Count > 0)
        //                            {
        //                                _trip.EndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dt.Rows[0]["dgpsDateTime"]), TimeZoneID);
        //                                _trip.EndOdometer = Convert.ToDouble(dt.Rows[0]["vOdometer"]);
        //                                _trip.LocationEnd = Convert.ToString(dt.Rows[0]["vTextMessage"]);
        //                                _trip.IdEnd = Convert.ToInt64(dt.Rows[0]["ipkCommanTrackingID"]);
        //                                _trip.VpkDeviceID = Trip.VpkDeviceID;

        //                                if (_tripWasNull)
        //                                {
        //                                    // customize start of trip details 
        //                                    var diffBetweenOrinalandCurrentEndTime = _trip.EndDate - _trip.StartDate;
        //                                    var mileageDiff = _trip.EndOdometer - _trip.StartOdometer;

        //                                    _trip.Duration = diffBetweenOrinalandCurrentEndTime;
        //                                    _trip.Distance = mileageDiff;

        //                                }
        //                                else
        //                                {
        //                                    // is trip was initialised 
        //                                    var diffBetweenOrinalandCurrentEndTime = Trip.EndDate - _trip.EndDate;
        //                                    var mileageDiff = _trip.EndOdometer - Trip.StartOdometer;

        //                                    _trip.Duration = TimeSpan.FromMilliseconds(_duration) + (Trip.Duration - diffBetweenOrinalandCurrentEndTime);
        //                                    _trip.Distance = _totalDistance + mileageDiff;
        //                                }

        //                            }
        //                            else
        //                            {
        //                                // no data was found in the future of this trip 
        //                                _trip.EndDate = Day.Item2;
        //                                _trip.EndOdometer = _trip.EndOdometer;
        //                                _trip.LocationEnd = " Unavailable ";
        //                                _trip.IdEnd = _trip.IdEnd;
        //                                _trip.VpkDeviceID = Trip.VpkDeviceID;
        //                                _trip.Duration = TimeSpan.FromMilliseconds(0);
        //                                _trip.Distance = 0;


        //                            }
        //                        }

        //                        filtered.Add(_trip);
        //                    }



        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //               LogError.RegisterErrorInLogFile( "Bal_BasicDailyOverviewReport.cs.cs", "FilterPerDay()", ex.Message  + ex.StackTrace);

        //            }
        //            return filtered;
        //        }
        public List<clsPopulateTripSummary> FilterPerDay(DataSet ds, DateTime _rangeStartDate, DateTime _rangeEndDate, int iCompanyid, long VpkDeviceID)
        {
            Trips trips = new Trips();

            var filtered = new List<clsPopulateTripSummary>();

            var Days = SplitDateRange(_rangeStartDate, _rangeEndDate, 1);

            var _DAL_BasicOverviewReport = new DAL_BasicOverviewReport();

            try
            {

                foreach (var Day in Days)
                {
                    var startTimeUTC = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(Day.Item1, TimeZoneID);
                    var endTimeUTC = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(Day.Item2, TimeZoneID);


                    var table = new DataTable();

                    if (ds.Tables.Count > 0)
                    {
                        table = ds.Tables[0].Copy();

                        table.DefaultView.RowFilter = " (dGPSDateTime >= #" + startTimeUTC.ToString("MM/dd/yyyy HH:mm:ss") + "# And dGPSDateTime <= #" + endTimeUTC.ToString("MM/dd/yyyy  HH:mm:ss") + "# ) ";


                        table = table.DefaultView.ToTable();

                    }

                    var _thisDaysDataset = new DataSet();

                    _thisDaysDataset.Tables.Add(table);

                    var CleansummeryList = trips.CleanDirtyTripSummary(_thisDaysDataset, iCompanyid, VpkDeviceID, Day.Item1, Day.Item2, TimeZoneID, false);

                    List<clsPopulateTripSummary> finalArray = CleansummeryList.OrderBy(n => n.StartDate).ToList();


                    List<clsPopulateTripSummary> TripsInBetween = new List<clsPopulateTripSummary>(),
                        TripsBeginningYesterday = new List<clsPopulateTripSummary>(),
                        TripsOverflowingToNextDay = new List<clsPopulateTripSummary>();

                    // filter this list to specific days based on their overlap 
                    TripsInBetween = finalArray.Where(n => n.StartDate >= Day.Item1 && n.EndDate <= Day.Item2).ToList();
                    TripsBeginningYesterday = finalArray.Where(n => n.StartDate < Day.Item1 && n.EndDate.Date == Day.Item2.Date).ToList();
                    TripsOverflowingToNextDay = finalArray.Where(n => n.EndDate > Day.Item2 && n.StartDate.Date == Day.Item1.Date).ToList();

                    var _trip = new clsPopulateTripSummary();

                    var _duration = 0.0;

                    var _totalDistance = 0.0;

                    var noOfTripsBetween = TripsInBetween.Count();

                    var noOfTripsYesterday = TripsBeginningYesterday.Count();

                    var noOfTripsNextDay = TripsOverflowingToNextDay.Count();

                    if (noOfTripsBetween != 0 ||
                        noOfTripsYesterday != 0 ||
                        noOfTripsNextDay != 0)
                    {

                        _duration += TripsInBetween.Sum(n => n.Duration.TotalMilliseconds);

                        _totalDistance += TripsInBetween.Sum(n => n.Distance);

                        if (noOfTripsYesterday == 0) // no trip  that started yesteday so default to today's first trip 
                        {
                            _trip = TripsInBetween.FirstOrDefault();
                        }

                        if (noOfTripsNextDay == 0)  // mark this as the last trip of that day
                        {
                            var lastTrip = TripsInBetween.LastOrDefault();

                            if (lastTrip == null)  // no trips in between the day
                            {
                                if (noOfTripsYesterday > 0)
                                {
                                    var dt = _DAL_BasicOverviewReport.BasicDailyOverViewReportFetch(1, startTimeUTC, endTimeUTC, VpkDeviceID).Tables[0];

                                    if (dt.Rows.Count > 0)
                                    {
                                        _trip.StartDate = Day.Item1;
                                        _trip.StartOdometer = Convert.ToDouble(dt.Rows[0]["vOdometer"]);
                                        _trip.LocationStart = Convert.ToString(dt.Rows[0]["vTextMessage"]);
                                        _trip.IdStart = Convert.ToInt64(dt.Rows[0]["ipkCommanTrackingID"]);



                                        var diffBetweenOrinalandCurrentStartTime = _trip.EndDate - _trip.StartDate;
                                        var mileageDiff = _trip.EndOdometer - _trip.StartOdometer;


                                        _trip.Duration = diffBetweenOrinalandCurrentStartTime;
                                        _trip.Distance = mileageDiff;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                _trip.EndDate = lastTrip.EndDate;
                                _trip.EndOdometer = lastTrip.EndOdometer;
                                _trip.LocationEnd = lastTrip.LocationEnd;
                                _trip.IdEnd = lastTrip.IdEnd;
                                _trip.Duration = TimeSpan.FromMilliseconds(_duration);
                                _trip.Distance = _totalDistance;
                            }
                        }

                        foreach (var Trip in TripsBeginningYesterday)
                        {

                            var dt = _DAL_BasicOverviewReport.BasicDailyOverViewReportFetch(1, startTimeUTC, Trip.EndDateUtc, Trip.VpkDeviceID).Tables[0];

                            if (dt.Rows.Count > 0)
                            {
                                _trip.StartDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dt.Rows[0]["dgpsDateTime"]), TimeZoneID);
                                _trip.StartOdometer = Convert.ToDouble(dt.Rows[0]["vOdometer"]);
                                _trip.LocationStart = Convert.ToString(dt.Rows[0]["vTextMessage"]);
                                _trip.IdStart = Convert.ToInt64(dt.Rows[0]["ipkCommanTrackingID"]);
                                _trip.VpkDeviceID = Trip.VpkDeviceID;


                                var diffBetweenOrinalandCurrentStartTime = _trip.StartDate - Trip.StartDate;
                                var mileageDiff = Trip.EndOdometer - _trip.StartOdometer;


                                _trip.Duration = TimeSpan.FromMilliseconds(_duration) + (Trip.Duration - diffBetweenOrinalandCurrentStartTime);

                                _trip.Distance = _totalDistance + mileageDiff;

                            }
                            else
                            {



                                _trip.StartDate = _trip.EndDate.Date;
                                _trip.StartOdometer = _trip.StartOdometer;
                                _trip.LocationStart = " Unavailable ";
                                _trip.IdStart = _trip.IdEnd;
                                _trip.VpkDeviceID = Trip.VpkDeviceID;

                                _trip.Duration = TimeSpan.FromMilliseconds(0);
                                _trip.Distance = 0;

                            }
                        }

                        foreach (var Trip in TripsOverflowingToNextDay)
                        {
                            var dt = _DAL_BasicOverviewReport.BasicDailyOverViewReportFetch(1, Trip.StartDateUtc, endTimeUTC, Trip.VpkDeviceID).Tables[0];

                            var _tripWasNull = false;

                            if (_trip == null)
                            {
                                _trip = Trip; //make this this trip default
                                _tripWasNull = true;
                            }

                            if (dt.Rows.Count > 0)
                            {
                                _trip.EndDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(Convert.ToDateTime(dt.Rows[0]["dgpsDateTime"]), TimeZoneID);
                                _trip.EndOdometer = Convert.ToDouble(dt.Rows[0]["vOdometer"]);
                                _trip.LocationEnd = Convert.ToString(dt.Rows[0]["vTextMessage"]);
                                _trip.IdEnd = Convert.ToInt64(dt.Rows[0]["ipkCommanTrackingID"]);
                                _trip.VpkDeviceID = Trip.VpkDeviceID;

                                if (_tripWasNull)
                                {
                                    // customize start of trip details 
                                    var diffBetweenOrinalandCurrentEndTime = _trip.EndDate - _trip.StartDate;
                                    var mileageDiff = _trip.EndOdometer - _trip.StartOdometer;

                                    _trip.Duration = diffBetweenOrinalandCurrentEndTime;
                                    _trip.Distance = mileageDiff;

                                }
                                else
                                {
                                    // is trip was initialised 
                                    var diffBetweenOrinalandCurrentEndTime = Trip.EndDate - _trip.EndDate;
                                    var mileageDiff = _trip.EndOdometer - Trip.StartOdometer;

                                    _trip.Duration = TimeSpan.FromMilliseconds(_duration) + (Trip.Duration - diffBetweenOrinalandCurrentEndTime);
                                    _trip.Distance = _totalDistance + mileageDiff;
                                }

                            }
                            else
                            {
                                // no data was found in the future of this trip 
                                _trip.EndDate = Day.Item2;
                                _trip.EndOdometer = _trip.EndOdometer;
                                _trip.LocationEnd = " Unavailable ";
                                _trip.IdEnd = _trip.IdEnd;
                                _trip.VpkDeviceID = Trip.VpkDeviceID;
                                _trip.Duration = TimeSpan.FromMilliseconds(0);
                                _trip.Distance = 0;


                            }
                        }



                        _trip.Distance = _trip.EndOdometer - _trip.StartOdometer;

                        var dto = _DAL_BasicOverviewReport.BasicDailyOverViewReportFetch(2, startTimeUTC, endTimeUTC, VpkDeviceID).Tables[0];

                        if (dto.Rows.Count > 0)
                            _trip.Distance = Convert.ToInt64(dto.Rows[0]["distance"]);


                        filtered.Add(_trip);
                    }



                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_BasicDailyOverviewReport.cs.cs", "FilterPerDay()", ex.Message + ex.StackTrace);

            }
            return filtered;
        }


    }
}
