using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WLT.DataAccessLayer.GPSOL;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_GPSOL
{
    public class Trips
    {
        //Method to calculate stopages and return hours,minutes and seconds for each trip stoppage
        //Input parameters device total Event records for the day
        //out key word enables a method to return multiple values

                  


        private static wlt_Config _wlt_AppConfig;

        public static string Connectionstring;
        public Trips()
        {
         

            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            Connectionstring = AppConfiguration.GetAppSettings<wlt_Config>("ConnectionStrings").wlt_WebAppConnectionString;

        }


        private int CalculateStoppages(DateTime startDate, DateTime endDateTime, DateTime nextIgnOnDateTime, string endLocation, bool lastDeviceRecordOfTheday, bool isInProgress, out int minutes, out int seconds)
        {
            TimeSpan StoppageTime = new TimeSpan(0, 0, 0, 0, 0);
            //Local Variables to hold return values
            int hours = 0;
            minutes = 0;
            seconds = 0;

            //TODO Handle a situation Where the trip is in Progress

            //StoppageTime = nextIgnOnDateTime.Subtract(startDate);

            StoppageTime = endDateTime.Subtract(startDate);

            hours = StoppageTime.Hours;
            minutes = StoppageTime.Minutes;
            seconds = StoppageTime.Seconds;


            ////This is the last  Record of the day
            //if (lastDeviceRecordOfTheday == true && endDateTime != null)
            //{
            //    StoppageTime = DateTime.Now.Subtract(endDateTime);

            //    hours = StoppageTime.Hours;
            //    minutes = StoppageTime.Minutes;
            //    seconds = StoppageTime.Seconds;
            //}


            //if (deviceEventCount == 1 && endDateTime != null)
            //{
            //    StoppageTime = DateTime.Now.Subtract(endDateTime);

            //    hours = StoppageTime.Hours;
            //    minutes = StoppageTime.Minutes;
            //    seconds = StoppageTime.Seconds;

            //}
            ////Several records exits indicate their stoppages
            //else if (deviceEventCount > 1 && lastDeviceRecordOfTheday == false && endDateTime != null)
            //{
            //    StoppageTime = nextIgnOnDateTime.Subtract(startDate);

            //    hours = StoppageTime.Hours;
            //    minutes = StoppageTime.Minutes;
            //    seconds = StoppageTime.Seconds;
            //}


            return hours;
        }



        public List<clsPopulateTripSummary> RemoveZeroDistanceFromCleanedTripSummary(double distanceToIgnore, List<clsPopulateTripSummary> CleanedTripSummary, string timeZoneId)
        {
            List<clsPopulateTripSummary> lstCleanTripSummaryWithoutZeroDistance = new List<clsPopulateTripSummary>();
            //try
            //  {


            bool LastTripOfDay = true;

            foreach (var listItem in CleanedTripSummary)
            {

                if (listItem.Distance < distanceToIgnore && !listItem.IsInProgress)
                {
                }

                else
                {


                    if (LastTripOfDay)
                    {  //update last trip of day (first record in this instance)
                        listItem.IsLastTripOfTheDay = true;
                        LastTripOfDay = false;
                    }

                    lstCleanTripSummaryWithoutZeroDistance.Add(listItem);



                }


            }




            // }
            //catch (Exception ex)
            //{
            //    LogError.RegisterErrorInLogFile( "MyAccount.cs", "AddStoppagesToCleanedTripSummary", ex.Message  + ex.StackTrace);
            //}

            //may have to reverse order 
            lstCleanTripSummaryWithoutZeroDistance = lstCleanTripSummaryWithoutZeroDistance.OrderByDescending(x => x.StartDateUtc).ToList();

            return lstCleanTripSummaryWithoutZeroDistance;



        }
        public List<clsPopulateTripSummary> GetOnlyStoppagesTripSummary(double distanceToIgnore, List<clsPopulateTripSummary> CleanedTripSummary, string timeZoneId)
        {
            List<clsPopulateTripSummary> lstCleanTripSummaryWithoutZeroDistance = new List<clsPopulateTripSummary>();
            List<clsPopulateTripSummary> DuplicateFreelist = new List<clsPopulateTripSummary>();


            foreach (var listItem in CleanedTripSummary)
            {
                if (listItem.IsStoppageItem)
                {
                    lstCleanTripSummaryWithoutZeroDistance.Add(listItem);
                }

            }
            lstCleanTripSummaryWithoutZeroDistance = lstCleanTripSummaryWithoutZeroDistance.OrderBy(x => x.EndDateUtc).ToList();

            string Temp_ = "";
            DateTime startDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            clsPopulateTripSummary myTempList = new clsPopulateTripSummary();
            foreach (var listItem in lstCleanTripSummaryWithoutZeroDistance)
            {



                if (listItem.LocationStart == Temp_)
                {
                    if (EndDate < listItem.EndDate)
                    {
                        EndDate = listItem.EndDate;
                    }

                    myTempList = new clsPopulateTripSummary(listItem.IdStart, listItem.IdEnd, listItem.StartOdometer, listItem.EndOdometer, startDate, EndDate, listItem.EndDateUtc, listItem.StartDateUtc, Temp_, listItem.LocationEnd, listItem.Distance, listItem.VpkDeviceID, listItem.DeviceDbId, listItem.IsLastTripOfTheDay, listItem.IsInProgress, listItem.IsStoppageItem, listItem.Hours, listItem.Minutes, listItem.Seconds, listItem.TripSelectedDate, listItem.FirstLocationLongtude, listItem.FirstLocationLattitude, 0, 0, listItem.StartSpeed, listItem.StartIgnitionStatus, listItem.EndSpeed, listItem.EndIgnitionStatus, listItem.Duration, listItem.ifkDriverID, listItem.iTripType, listItem.isNextDay);

                }
                else
                {
                    DuplicateFreelist.Add(myTempList);
                    Temp_ = listItem.LocationStart;
                    startDate = listItem.StartDate;
                    EndDate = listItem.EndDate;
                    myTempList = new clsPopulateTripSummary(listItem.IdStart, listItem.IdEnd, listItem.StartOdometer, listItem.EndOdometer, startDate, EndDate, listItem.EndDateUtc, listItem.StartDateUtc, Temp_, listItem.LocationEnd, listItem.Distance, listItem.VpkDeviceID, listItem.DeviceDbId, listItem.IsLastTripOfTheDay, listItem.IsInProgress, listItem.IsStoppageItem, listItem.Hours, listItem.Minutes, listItem.Seconds, listItem.TripSelectedDate, listItem.FirstLocationLongtude, listItem.FirstLocationLattitude, 0, 0, listItem.StartSpeed, listItem.StartIgnitionStatus, listItem.EndSpeed, listItem.EndIgnitionStatus, listItem.Duration, listItem.ifkDriverID, listItem.iTripType, listItem.isNextDay);

                }

            }
            DuplicateFreelist.Add(myTempList);




            DuplicateFreelist.RemoveAll(s => s.StartDate > s.EndDate);
            DateTime DefaultDate = Convert.ToDateTime("01-Jan-0001");
            DuplicateFreelist.RemoveAll(s => s.StartDate == DefaultDate);
            DuplicateFreelist = DuplicateFreelist.OrderByDescending(x => x.EndDateUtc).ToList();
            return DuplicateFreelist;
        }


        public List<clsPopulateTripSummary> AddStoppagesToCleanedTripSummary(List<clsPopulateTripSummary> CleanedTripSummary, string timeZoneId)
        {


            List<clsPopulateTripSummary> lstCleanTripSummaryWithStoppages = new List<clsPopulateTripSummary>();
            try
            {

                int i = 0;

                foreach (var listItem in CleanedTripSummary)
                {
                    //starts with last trip of day


                    //##### set variables #####
                    long _startId = 0;
                    long _endId = 0;

                    DateTime _tripSelectedDate = Convert.ToDateTime(listItem.TripSelectedDate);
                    DateTime _startDate = DateTime.UtcNow;
                    DateTime _startDateUtc = DateTime.UtcNow;
                    DateTime _endDate = DateTime.UtcNow;
                    DateTime _endDateUtc = DateTime.UtcNow;
                    DateTime _nextIgnitionOnTime = DateTime.UtcNow;

                    //Longitude/Lattitude first Locationm 
                    double FirstLocationLongitude = 0;
                    double FirstLocationLattitude = 0;


                    // Longitude/Lattitude first Locationm 
                    double SecondLocationLongitude = 0;
                    double SecondLocationLattitude = 0;


                    string _startLocation = "";
                    string _endLocation = "";

                    double _distance = 0;
                    double _startOdometer = 0;
                    double _endOdometer = 0;

                    long _imeiNumber = listItem.VpkDeviceID;
                    long _deviceDbId = listItem.DeviceDbId;

                    bool _isLastTripOfTheDay = false;
                    bool _isInProgress = false;
                    bool _isStoppageItem = true;
                    int _hours = 0, _minutes = 0, _seconds = 0;

                    int StartSpeed = 10;
                    bool StartIgnitionStatus = false;

                    int EndSpeed = 10;
                    bool EndIgnitionStatus = false;

                    if (CleanedTripSummary.Count >= i)
                    {

                        if (listItem.IsInProgress)
                        {
                            //dont add a stoppage after this
                        }
                        else if (!listItem.IsInProgress && !listItem.IsLastTripOfTheDay)
                        {
                            //not last trip or an in progress trip
                            if (i != 0)
                            {
                                _startId = listItem.IdEnd;
                                _startDate = listItem.EndDate;
                                _startDateUtc = listItem.EndDateUtc;
                                _startLocation = listItem.LocationEnd;
                                _startOdometer = listItem.StartOdometer;
                                _endOdometer = listItem.EndOdometer;

                                FirstLocationLongitude = listItem.FirstLocationLongtude;
                                FirstLocationLattitude = listItem.FirstLocationLattitude;


                                _endId = CleanedTripSummary[i - 1].IdStart;
                                _endDate = CleanedTripSummary[i - 1].StartDate;
                                _endDateUtc = CleanedTripSummary[i - 1].StartDateUtc;
                                _endLocation = CleanedTripSummary[i - 1].LocationStart;



                                SecondLocationLongitude = CleanedTripSummary[i - 1].SecondLocationLongtude;
                                SecondLocationLattitude = CleanedTripSummary[i - 1].SecondLocationLattitude;

                                _nextIgnitionOnTime = CleanedTripSummary[i - 1].StartDateUtc;

                                _isLastTripOfTheDay = false; //needed?
                                _isInProgress = false; //needed?

                                _hours = CalculateStoppages(_startDateUtc, _endDateUtc, _nextIgnitionOnTime, _endLocation, _isLastTripOfTheDay, _isInProgress, out _minutes, out _seconds);


                                // #### Add stoppage to List #####
                                lstCleanTripSummaryWithStoppages.Add(new clsPopulateTripSummary(_startId, _endId, _startOdometer, _endOdometer, _startDate, _endDate, _startDateUtc, _endDateUtc, _startLocation, _endLocation, _distance, _imeiNumber, _deviceDbId, _isLastTripOfTheDay, _isInProgress, true, _hours, _minutes, _seconds, _tripSelectedDate, FirstLocationLongitude, FirstLocationLattitude, SecondLocationLongitude, SecondLocationLattitude, StartSpeed, StartIgnitionStatus, EndSpeed, EndIgnitionStatus, listItem.Duration, listItem.ifkDriverID, listItem.iTripType, listItem.isNextDay));

                            }

                        }
                        else if (!listItem.IsInProgress && listItem.IsLastTripOfTheDay)
                        {
                            //text must say "currently stopped at N
                            _startId = listItem.IdEnd;
                            _startDate = listItem.EndDate;
                            _startDateUtc = listItem.EndDateUtc;
                            _startLocation = listItem.LocationEnd;
                            _startOdometer = listItem.StartOdometer;
                            _endOdometer = listItem.EndOdometer;


                            FirstLocationLongitude = listItem.FirstLocationLongtude;
                            FirstLocationLattitude = listItem.FirstLocationLattitude;


                            _endId = CleanedTripSummary[i].IdEnd;
                            _endLocation = CleanedTripSummary[i].LocationEnd;

                            SecondLocationLongitude = CleanedTripSummary[i].SecondLocationLongtude;
                            SecondLocationLattitude = CleanedTripSummary[i].SecondLocationLattitude;


                            _isLastTripOfTheDay = false; //needed?
                            _isInProgress = false; //needed?


                            //DateTime UsersDate = Convert.ToDateTime(UserSettings.ConvertUTCDateTimeToLocalDateTime(_startDate.Date, timeZoneId));
                            if (_startDate.Date == DateTime.Today)
                            {
                                //################# This might break for different timezones!!! #############################
                                //if its today:
                                _endDate = Convert.ToDateTime(UserSettings.ConvertUTCDateTimeToLocalDateTime(DateTime.UtcNow, timeZoneId)); //set to end of day
                                _endDateUtc = DateTime.UtcNow;  //set to end of day 
                                _nextIgnitionOnTime = DateTime.UtcNow;
                                _hours = CalculateStoppages(_startDateUtc, _endDateUtc, _nextIgnitionOnTime, _endLocation, _isLastTripOfTheDay, _isInProgress, out _minutes, out _seconds);
                            }


                            else
                            {



                                //if its in the past (yesterday or beyond)
                                _endDate = listItem.EndDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59); //set to end of day
                                _endDateUtc = _tripSelectedDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);  //set to end of day 
                                _nextIgnitionOnTime = _tripSelectedDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                                _hours = CalculateStoppages(_startDate, _endDate, _nextIgnitionOnTime, _endLocation, _isLastTripOfTheDay, _isInProgress, out _minutes, out _seconds);


                            }

                            //_hours = CalculateStoppages(_startDate, _endDate, _nextIgnitionOnTime, _endLocation, _isLastTripOfTheDay, _isInProgress, out _minutes, out _seconds);


                            // #### Add stoppage to List #####
                            // lstCleanTripSummaryWithStoppages.Add(new clsPopulateTripSummary(_startId, _endId, _startOdometer, _endOdometer, _startDate, _endDate, _startDateUtc, _endDateUtc, _startLocation, _endLocation, _distance, _imeiNumber, _deviceDbId,  _isLastTripOfTheDay, _isInProgress, true, _hours, _minutes, _seconds, _tripSelectedDate));

                            // if this is a spill over  (stretches to the next day) don't add stoppage record 

                            if (!listItem.isNextDay)
                            {
                                lstCleanTripSummaryWithStoppages.Add(new clsPopulateTripSummary(_startId, _endId, _startOdometer, _endOdometer, _startDate, _endDate, _startDateUtc, _endDateUtc, _startLocation, _endLocation, _distance, _imeiNumber, _deviceDbId, _isLastTripOfTheDay, _isInProgress, true, _hours, _minutes, _seconds, _tripSelectedDate, FirstLocationLongitude, FirstLocationLattitude, SecondLocationLongitude, SecondLocationLattitude, StartSpeed, StartIgnitionStatus, EndSpeed, EndIgnitionStatus, listItem.Duration, listItem.ifkDriverID, listItem.iTripType, listItem.isNextDay));
                            }
                        }


                        i++;

                    }

                    lstCleanTripSummaryWithStoppages.Add(listItem);

                }

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "MyAccount.cs", "AddStoppagesToCleanedTripSummary", ex.Message  + ex.StackTrace);
            }


            return lstCleanTripSummaryWithStoppages;


        }


        public DataSet GetDirtyTripSummary(long deviceDbId, DateTime tripStartDateTimeLocal, DateTime tripEndDateTimeLocal, string timeZoneId, int iTrackerType)
        {
            DataSet dirtyTripSummary = new DataSet();

            DAL_GetTripSummary objTripSummary = new DAL_GetTripSummary();
            DateTime todaysDate = System.DateTime.UtcNow;

            try
            {

                string _strStartDateRange = "";
                string _strEndDateRange = "";

                if (tripStartDateTimeLocal.ToString() == "1/1/0001 12:00:00 AM")
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate.AddHours(24).AddSeconds(-1), timeZoneId);
                }
                else
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripStartDateTimeLocal, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripEndDateTimeLocal, timeZoneId);
                }

                dirtyTripSummary = objTripSummary.GetTripSummary(deviceDbId, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange), iTrackerType);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "Trips.cs", "GetDirtyTripSummary()", ex.Message  + ex.StackTrace);
            }

            return dirtyTripSummary;

        }


        public DataSet GetDirtyTripSummaryForMultipleAssets(string imeiNumbersCsv, DateTime tripStartDateTimeLocal, DateTime tripEndDateTimeLocal, string timeZoneId)
        {
            DataSet dirtyTripSummary = new DataSet();

            DAL_GetTripSummary objTripSummary = new DAL_GetTripSummary();
            DateTime todaysDate = System.DateTime.UtcNow;

            try
            {

                string _strStartDateRange = "";
                string _strEndDateRange = "";

                if (tripStartDateTimeLocal.ToString() == "1/1/0001 12:00:00 AM")
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate.AddHours(24).AddSeconds(-1), timeZoneId);
                }
                else
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripStartDateTimeLocal, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripEndDateTimeLocal, timeZoneId);
                }

                dirtyTripSummary = objTripSummary.GetTripSummaryForMultipleAssets(imeiNumbersCsv, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange));
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "Trips.cs", "GetDirtyTripSummary()", ex.Message  + ex.StackTrace);
            }

            return dirtyTripSummary;

        }

        public DataSet GetDirtyTripSummaryForMultipleAssets(string imeiNumbersCsv, DateTime tripStartDateTimeLocal, DateTime tripEndDateTimeLocal)
        {
            DataSet dirtyTripSummary = new DataSet();

            DAL_GetTripSummary objTripSummary = new DAL_GetTripSummary();
            DateTime todaysDate = System.DateTime.UtcNow;

            try
            {
                dirtyTripSummary = objTripSummary.GetTripSummaryForMultipleAssets(imeiNumbersCsv, tripStartDateTimeLocal, tripEndDateTimeLocal);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "Trips.cs", "GetDirtyTripSummary()", ex.Message  + ex.StackTrace);
            }

            return dirtyTripSummary;

        }

        //These parameters to be passed from interface and session Variable
        public List<clsPopulateTripSummary> CleanDirtyTripSummary(DataSet dirtyTripSummary, int companyId, long deviceDbId, DateTime tripStartDateTimeLocal, DateTime tripEndDateTimeLocal, string timeZoneId, bool includeStoppages)
        {
            //imeiNumber will be passed from where this method will be called from

            List<clsPopulateTripSummary> lstPopulateTripSummary = new List<clsPopulateTripSummary>();
            List<clsPopulateTripSummary> lstSortedList = null;
            //clean trip summary
            //Variable declaration
            bool firstRecord = true;

            bool _isInProgress = false;
            bool _isStoppageItem = false;
            bool _isLastTripOfTheDay = false;
            int currentRecord = 0;
            //int remainingRecords = 1;
            double _startOdometer = 0;
            double _endOdometer = 0;

            long _startId = 0;
            long _endId = 0;
            int _eventId = 0;
            int _tripType = 0;
            int totalRecordCount = 0;
            //initialize date to utc time
            DateTime _startDateLocal = DateTime.UtcNow;
            DateTime _endDateLocal = DateTime.UtcNow;
            DateTime _startDateUtc = DateTime.UtcNow;
            DateTime _endDateUtc = DateTime.UtcNow;

            TimeSpan _duration;

            string _startLocation = "";
            string _endLocation = "";


            //Longitude and Lattitude first Location
            double FirstLocationLongitude = 0;
            double FirstLocationLatitude = 0;


            //Longitude and Lattitude second Location
            double SecondLocationLongtude = 0;
            double SecondLocationLattitude = 0;

            int StartSpeed = 0;
            bool StartIgnitionStatus = false;
            int EndSpeed = 0;
            bool EndIgnitionStatus = false;

            double _distance = 0;
            long _vpkDeviceID = 0;
            long _deviceDbId = deviceDbId;

            int previousEventId = 0;
            int nextEventID = 0;
            double previousOdometerReading = 0.0;
            double currentVdometerReading = 0.0;
            //vars to recieve output paramter values
            int _minutes = 0, _seconds = 0;

            int _hours = 0;
            //define dataset for the trip summary
            DataSet dsTripSummary = new DataSet();
            DataSet dsPreviousIgnOn = new DataSet();
            DataSet dsNextIgnOff = new DataSet();
            DataSet dsDaysFirstInstanceOfbIsIgnition = new DataSet();


            //DAL_GetTripSummary
            DAL_GetTripSummary objTripSummary = new DAL_GetTripSummary();
            //Fill the dataset
            //create an object of clsPopulateTripSummary
            DateTime nextIgnOnDateTime = new DateTime();
            DateTime todaysDate = System.DateTime.UtcNow;


            try
            {

                string _strStartDateRange = "";
                string _strEndDateRange = "";

                if (tripStartDateTimeLocal.ToString() == "1/1/0001 12:00:00 AM")
                {
                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(todaysDate.AddHours(24).AddSeconds(-1), timeZoneId);
                }
                else
                {


                    _strStartDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripStartDateTimeLocal, timeZoneId);
                    _strEndDateRange = UserSettings.ConvertLocalDateTimeToUTCDateTime(tripEndDateTimeLocal, timeZoneId);
                }

                ////get all the 7 and 8's
                //dsTripSummary = objTripSummary.GetTripSummary(imeiNumber, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange));
                dsTripSummary = dirtyTripSummary;



                //these will act as loop counters
                long firstIgnitionOnDatasetId = 0;
                totalRecordCount = dsTripSummary.Tables[0].Rows.Count;
                currentRecord = dsTripSummary.Tables[0].Rows.Count;


                //if Imeis > 1 then loop through each imei first

                //loop through all the 7 and 8's and build trip object with each trip
                for (int i = 0; i < totalRecordCount; i++)
                {

                    bool _isNetxDay = false;
                    _vpkDeviceID = Convert.ToInt64(dsTripSummary.Tables[0].Rows[i]["vpkDeviceID"]);
                    int DriverID = Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["ifkDriverID"]);

                    currentRecord = currentRecord - 1; //we do  -1 instead of +1 as the list object is ordered as most recent trip at top
                    if (currentRecord == 0) //Lastrecord
                    {
                        _isLastTripOfTheDay = true;
                    }


                    if (Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vReportID"]) == 8 && firstRecord == true)
                    {
                        #region firstRecordIsIgnOff

                        if (!_isLastTripOfTheDay)
                        {
                            if (Convert.ToInt32(dsTripSummary.Tables[0].Rows[i + 1]["vReportID"]) == 8)
                            {
                                continue;  //MarkD hack to look at next 8 as we need to take the LAST 8 (so ignore this record)
                                //this wont work very well if you get  8 8 8 7 8 7 8 etc as it will use the 3rd 8 and the first 2 will have missing start of trips, needs a hack to fill in 124's between these first three 8's 

                            }
                        }



                        //variables to be maintained between loops
                        /* 1: startId
                         * 2: Distance(first ordometer reading) help in calculating distance
                         * 3: StartDate
                         * 4: Start Location
                         * */

                        //if trip starts with ign off, get the last ign on ...this will be very rare            
                        //this is last ign on
                        //todo get the 7 from previous day and put in object
                        //update the object
                        //select previous
                        //get previous day start of trip (7)



                        //this returns yesterdays 7 and 8's
                        dsPreviousIgnOn = objTripSummary.GetTripSummary_PreviousDaysIgnitionOn(_vpkDeviceID, Convert.ToDateTime(_strStartDateRange).AddDays(-1), Convert.ToDateTime(_strStartDateRange));


                        //get values of the first row returned
                        if (dsPreviousIgnOn.Tables[0].Rows.Count > 0)
                        {
                            _eventId = Convert.ToInt32(dsPreviousIgnOn.Tables[0].Rows[0]["vReportID"]);

                            if (_eventId == 7)
                            {
                                //use yesterdays igntion on as there was no ign off after it
                                _startId = Convert.ToInt64(dsPreviousIgnOn.Tables[0].Rows[0]["ipkCommanTrackingID"]);
                                _tripType = Convert.ToInt32(dsPreviousIgnOn.Tables[0].Rows[0]["TripType"]);
                                _startDateUtc = Convert.ToDateTime(dsPreviousIgnOn.Tables[0].Rows[0]["dGPSDatetime"]);
                                //_startDateUtc = Convert.ToDateTime(dsPreviousIgnOn.Tables[0].Rows[0]["dGPSDatetime"]);
                                _startLocation = Convert.ToString(dsPreviousIgnOn.Tables[0].Rows[0]["vTextMessage"]);

                                FirstLocationLongitude = Convert.ToDouble(dsPreviousIgnOn.Tables[0].Rows[0]["vlongitude"]);
                                FirstLocationLatitude = Convert.ToDouble(dsPreviousIgnOn.Tables[0].Rows[0]["vLatitude"]);

                                StartSpeed = Convert.ToInt32(dsPreviousIgnOn.Tables[0].Rows[0]["vVehicleSpeed"]);
                                StartIgnitionStatus = Convert.ToBoolean(dsPreviousIgnOn.Tables[0].Rows[0]["bIsIgnitionOn"]);


                                _startOdometer = Convert.ToDouble(dsPreviousIgnOn.Tables[0].Rows[0]["vOdometer"]);
                                previousEventId = (Convert.ToInt16(dsPreviousIgnOn.Tables[0].Rows[0]["vReportID"]));
                                previousOdometerReading = Convert.ToDouble(dsPreviousIgnOn.Tables[0].Rows[0]["vOdometer"]);
                            }
                            else
                            {
                                //the last event of the previous day was an 8....we've lost the ign on, so we need to improvise and get the first instance of bIsIgnition = true and fake this as an ignition on.
                                //get the ID of that 8 (ign off) and look from that point onwards for relevent 124's with bIsIgnition = true
                                //int _idOfThatIgnOffOnPreviousDay = Convert.ToInt32(dsPreviousIgnOn.Tables[0].Rows[0]["ipkCommanTrackingID"]);

                                //get the date time of the 124 with ign on (after yesterdays 8) - then we can look at this point onwards for the first instance of bisgintion on
                                DateTime _124withBIsIgnOn_startDateUtc = Convert.ToDateTime(dsPreviousIgnOn.Tables[0].Rows[0]["dGPSDatetime"]);
                                DateTime ignOffTime = Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i]["dGPSDatetime"]);

                                //DaysFirstInstanceOfbIsIgnition = objTripSummary.GetTripSummary_DaysFirstInstanceOfbIsIgnition(_vpkDeviceID, Convert.ToDateTime(_124withBIsIgnOn_startDateUtc), Convert.ToDateTime(_strEndDateRange));
                                dsDaysFirstInstanceOfbIsIgnition = objTripSummary.GetTripSummary_DaysFirstInstanceOfbIsIgnition(_vpkDeviceID, Convert.ToDateTime(_124withBIsIgnOn_startDateUtc), ignOffTime);

                                //if (dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows.Count == 0)
                                //{

                                //========== TODO - at this point, if there is no record, we should disgard this trip as its bunk! ##############################


                                //look for that bIsIgntionOn on the previous day
                                //dsDaysFirstInstanceOfbIsIgnition = objTripSummary.GetTripSummary_DaysFirstInstanceOfbIsIgnition(_vpkDeviceID, Convert.ToDateTime(_strStartDateRange).AddDays(-1), Convert.ToDateTime(_strEndDateRange));
                                //}
                                //if (dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows.Count == 0)
                                //{
                                //    //look for that bIsIgntionOn on the previous day -2
                                //    dsDaysFirstInstanceOfbIsIgnition = objTripSummary.GetTripSummary_DaysFirstInstanceOfbIsIgnition(_vpkDeviceID, Convert.ToDateTime(_strStartDateRange).AddDays(-2), Convert.ToDateTime(_strEndDateRange));
                                //}
                                //if (dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows.Count == 0)
                                //{
                                //    //look for that bIsIgntionOn on the previous day -3
                                //    dsDaysFirstInstanceOfbIsIgnition = objTripSummary.GetTripSummary_DaysFirstInstanceOfbIsIgnition(_vpkDeviceID, Convert.ToDateTime(_strStartDateRange).AddDays(-3), Convert.ToDateTime(_strEndDateRange));
                                //}

                                //is there a 124 with bIsIgn on after yesterdays 8
                                if (dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows.Count > 0)
                                {

                                    if (Convert.ToDateTime(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["dGPSDatetime"]) < Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i]["dGPSDatetime"]))
                                    {
                                        //use the FIRST instance of bIsIgntion on as the start of the trip FROM CURRENT DAY
                                        _startId = Convert.ToInt64(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["ipkCommanTrackingID"]);
                                        _tripType = Convert.ToInt32(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["TripType"]);
                                        _startDateUtc = Convert.ToDateTime(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["dGPSDatetime"]);
                                        //_startDateUtc = Convert.ToDateTime(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["dGPSDatetime"]);
                                        _startLocation = Convert.ToString(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vTextMessage"]);

                                        FirstLocationLongitude = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vlongitude"]);
                                        FirstLocationLatitude = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vLatitude"]);

                                        StartSpeed = Convert.ToInt32(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vVehicleSpeed"]);
                                        StartIgnitionStatus = Convert.ToBoolean(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["bIsIgnitionOn"]);



                                        previousEventId = (Convert.ToInt16(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vReportID"]));
                                        previousOdometerReading = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vOdometer"]);
                                        _startOdometer = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vOdometer"]);
                                    }
                                    else
                                    {
                                        //get the LAST instance of bIsIgntion on as the start of the trip FROM PREVIOUS DAY
                                        dsDaysFirstInstanceOfbIsIgnition = objTripSummary.GetTripSummary_DaysLastInstanceOfbIsIgnitionAfterIgnitionOff(_vpkDeviceID, Convert.ToDateTime(_strStartDateRange).AddDays(-1), Convert.ToDateTime(_strStartDateRange));

                                        if (dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows.Count > 0)
                                        {
                                            if (Convert.ToDateTime(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["dGPSDatetime"]) < Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i]["dGPSDatetime"]))
                                            {
                                                _startId = Convert.ToInt64(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["ipkCommanTrackingID"]);
                                                _tripType = Convert.ToInt32(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["TripType"]);
                                                _startDateUtc = Convert.ToDateTime(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["dGPSDatetime"]);
                                                //_startDateUtc = Convert.ToDateTime(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["dGPSDatetime"]);
                                                _startLocation = Convert.ToString(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vTextMessage"]);

                                                FirstLocationLongitude = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vlongitude"]);
                                                FirstLocationLatitude = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vLatitude"]);

                                                StartSpeed = Convert.ToInt32(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vVehicleSpeed"]);
                                                StartIgnitionStatus = Convert.ToBoolean(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["bIsIgnitionOn"]);



                                                previousEventId = (Convert.ToInt16(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vReportID"]));
                                                previousOdometerReading = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vOdometer"]);
                                                _startOdometer = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vOdometer"]);
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }


                                    }
                                }

                            }



                        }
                        else
                        {

                          try
                            {
                                //GET FIRST INSTANCE OF IGN STATUS TRUE FOR CURRENT DAY (there are no orphaned 7's from the previous day.)
                                dsDaysFirstInstanceOfbIsIgnition = objTripSummary.GetTripSummary_DaysFirstInstanceOfbIsIgnition(_vpkDeviceID, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange));
                                _startId = Convert.ToInt64(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["ipkCommanTrackingID"]);
                                _tripType = Convert.ToInt32(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["TripType"]);
                                _startDateUtc = Convert.ToDateTime(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["dGPSDatetime"]);
                                //_startDateUtc = Convert.ToDateTime(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["dGPSDatetime"]);
                                _startLocation = Convert.ToString(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vTextMessage"]);

                                FirstLocationLongitude = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vlongitude"]);
                                FirstLocationLatitude = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vLatitude"]);

                                StartSpeed = Convert.ToInt32(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vVehicleSpeed"]);
                                StartIgnitionStatus = Convert.ToBoolean(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["bIsIgnitionOn"]);



                                previousEventId = (Convert.ToInt16(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vReportID"]));
                                previousOdometerReading = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vOdometer"]);
                                _startOdometer = Convert.ToDouble(dsDaysFirstInstanceOfbIsIgnition.Tables[0].Rows[0]["vOdometer"]);

                            }
                            catch (Exception ex)
                            {
                                LogError.RegisterErrorInLogFile("Trips.cs", $"Trips {ex.StackTrace}", "CleanTripSummaryForDate()", ex.Message  + ex.StackTrace);
                            }
                        
                        
                        }

                        #endregion
                    }


                    //TODO - if there is no Ignition Off as last item of day - ####################################################################################################################################################################################################################
                    //Set as current trip (Only for today)
                    else
                    {

                        if (Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vReportID"]) == 7)
                        {
                            #region currentRecordIsIgnOn

                            if (previousEventId == 0) //first ign on of the day
                            {

                                _startId = Convert.ToInt64(dsTripSummary.Tables[0].Rows[i]["ipkCommanTrackingID"]);
                                _tripType = Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["TripType"]);
                                _startDateUtc = Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i]["dGPSDatetime"]);
                                _startLocation = Convert.ToString(dsTripSummary.Tables[0].Rows[i]["vTextMessage"]);

                                FirstLocationLongitude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vlongitude"]);
                                FirstLocationLatitude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vLatitude"]);

                                StartSpeed = Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vVehicleSpeed"]);
                                StartIgnitionStatus = Convert.ToBoolean(dsTripSummary.Tables[0].Rows[i]["bIsIgnitionOn"]);

                                previousOdometerReading = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);
                                previousEventId = (Convert.ToInt16(dsTripSummary.Tables[0].Rows[i]["vReportID"]));
                                _startOdometer = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);

                                //endId is not known at this point thus set to its initial value of 0
                                //LocationEnd is not known at this point,thus empty
                                //only for today

                                if (_isLastTripOfTheDay) //first ign on, no last record, and last trip (so probably in progress trip!)
                                {

                                    /////////////////////////////////////START DATE UTC and StartDate are not processed here!!!!!!!!!!!!!!!!!!!!!
                                    //get current point and use as end Date
                                    //Get the last position report for CurrentTrip/IsInProgress/LastRecord/CurrentDay
                                    DataSet dsDaysLastPosition = new DataSet();
                                    dsDaysLastPosition = objTripSummary.GetTripSummary_CurrentDaysLastPosition(_vpkDeviceID, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange));
                                    if (dsDaysLastPosition.Tables[0].Rows.Count > 0)
                                    {
                                        _endId = Convert.ToInt64(dsDaysLastPosition.Tables[0].Rows[0]["ipkCommanTrackingID"]);
                                        _endLocation = Convert.ToString(dsDaysLastPosition.Tables[0].Rows[0]["vTextMessage"]);

                                        SecondLocationLongtude = Convert.ToDouble(dsDaysLastPosition.Tables[0].Rows[0]["vlongitude"]);
                                        SecondLocationLattitude = Convert.ToDouble(dsDaysLastPosition.Tables[0].Rows[0]["vLatitude"]);

                                        EndSpeed = Convert.ToInt32(dsDaysLastPosition.Tables[0].Rows[0]["vVehicleSpeed"]);
                                        EndIgnitionStatus = Convert.ToBoolean(dsDaysLastPosition.Tables[0].Rows[0]["bIsIgnitionOn"]);


                                        _endDateUtc = Convert.ToDateTime(dsDaysLastPosition.Tables[0].Rows[0]["dGPSDatetime"]);
                                        _endOdometer = Convert.ToDouble(dsDaysLastPosition.Tables[0].Rows[0]["vOdometer"]);

                                        _distance = _endOdometer - previousOdometerReading;

                                    }
                                    else
                                    {
                                        _endDateUtc = _startDateUtc;
                                    }


                                    //TODO - if first trip of day, and current trip, need to test this works #############################################################################################################################################################################################################   
                                    //lstPopulateTripSummary.Add(new clsPopulateTripSummary(_startId, _endId, _startDateLocal, _endDateLocal, _startDateUtc, _endDateUtc, _startLocation, _endLocation, _distance, imeiNumber, _isLastTripOfTheDay, _isInProgress, _isStoppageItem, _hours, _minutes, _seconds, tripSelectedDate));

                                }
                                firstRecord = false;

                                if (!_isLastTripOfTheDay)
                                {
                                    continue; //get next record as this is just the first ignition on
                                }
                            }
                            else if (previousEventId == 7) //another Ign on
                            {
                                //ignore this message  so we can use the previous 7
                                //pass control to the next iteration,Exit the current iteration
                                firstRecord = false;
                                if (!_isLastTripOfTheDay)
                                {
                                    continue;//Ignore below code  and go to top to the next item in loop (hopefully its an 8)
                                }
                            }
                            else if (previousEventId == 8)
                            {
                                //check if is the last record of the day
                                //we are begining a new trip and endid is not known
                                //End date also not known
                                //location end also not known

                                _startId = Convert.ToInt64(dsTripSummary.Tables[0].Rows[i]["ipkCommanTrackingID"]);
                                _tripType = Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["TripType"]);
                                _startDateUtc = Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i]["dGPSDatetime"]);
                                _startLocation = Convert.ToString(dsTripSummary.Tables[0].Rows[i]["vTextMessage"]);


                                FirstLocationLongitude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vlongitude"]);
                                FirstLocationLatitude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vLatitude"]);

                                StartSpeed = Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vVehicleSpeed"]);
                                StartIgnitionStatus = Convert.ToBoolean(dsTripSummary.Tables[0].Rows[i]["bIsIgnitionOn"]);



                                previousOdometerReading = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);
                                previousEventId = (Convert.ToInt16(dsTripSummary.Tables[0].Rows[i]["vReportID"]));
                                _startOdometer = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);

                                //if last record?
                                if (!_isLastTripOfTheDay)
                                {
                                    continue; //Ignore below code  and go to top to the next item in loop (hopefully its an 8) - unless its the last trip of day!
                                }

                            }
                            //continue;

                            #endregion
                        }
                        else if (Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vReportID"]) == 8)
                        {
                            #region currentRecordIsIgnOff

                            if (previousEventId == 8) //use this and the ign off instead of previous 8
                            {
                                //LastIgnitionOnDatasetId has been set to the loop counter

                                //this current message is what we need instead of last 8
                                //overwrite the end of trip on last message in the list
                                //dt.rows.lastmessage.update the ign off
                                //update the object 

                                //update the local variables 
                                currentVdometerReading = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);
                                _distance = currentVdometerReading - previousOdometerReading;   //todo will this work as the previus record is an 8 too? #############################################################################
                                _endId = Convert.ToInt64(dsTripSummary.Tables[0].Rows[i]["ipkCommanTrackingID"]);
                                _endDateUtc = Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i]["dGPSDatetime"]);
                                _endLocation = Convert.ToString(dsTripSummary.Tables[0].Rows[i]["vTextMessage"]);


                                SecondLocationLongtude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vlongitude"]);
                                SecondLocationLattitude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vLatitude"]);

                                EndSpeed = Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vVehicleSpeed"]);
                                EndIgnitionStatus = Convert.ToBoolean(dsTripSummary.Tables[0].Rows[i]["bIsIgnitionOn"]);



                                _endOdometer = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);

                                //skip the rest of the iteration of this loop preserv the value of startId
                                if (!_isLastTripOfTheDay)
                                {
                                    nextEventID = (Convert.ToInt16(dsTripSummary.Tables[0].Rows[i + 1]["vReportID"]));
                                    if (Convert.ToInt16(dsTripSummary.Tables[0].Rows[i]["vReportID"]) != nextEventID)
                                    {
                                        //update the list and previous event id
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                            }
                            else if (previousEventId == 7) //marry the trip (last is 7 and this is 8)
                            {
                                //calculate final distance
                                currentVdometerReading = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);


                                _distance = currentVdometerReading - previousOdometerReading;
                                _endId = Convert.ToInt64(dsTripSummary.Tables[0].Rows[i]["ipkCommanTrackingID"]);
                                _endDateUtc = Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i]["dGPSDatetime"]);
                                _endLocation = Convert.ToString(dsTripSummary.Tables[0].Rows[i]["vTextMessage"]);

                                SecondLocationLongtude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vlongitude"]);
                                SecondLocationLattitude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vLatitude"]);

                                EndSpeed = Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vVehicleSpeed"]);
                                EndIgnitionStatus = Convert.ToBoolean(dsTripSummary.Tables[0].Rows[i]["bIsIgnitionOn"]);



                                _endOdometer = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);
                            }

                            //If is the current trip  is the last record:Dont get next event ID
                            if (_isLastTripOfTheDay != true)
                            {
                                //Do not add the record to the list :
                                //Continue updating the 8
                                nextEventID = (Convert.ToInt16(dsTripSummary.Tables[0].Rows[i + 1]["vReportID"]));
                                if ((Convert.ToInt16(dsTripSummary.Tables[0].Rows[i]["vReportID"])) == nextEventID) //is next ign status the same?
                                {
                                    //
                                    previousEventId = (Convert.ToInt16(dsTripSummary.Tables[0].Rows[i]["vReportID"])); //set the global variable then "continue" and get next record as we'll use that instead
                                    continue;
                                }
                            }
                            //After Adding data to the object then then add it to the list

                            //last thing to do is set event id

                            #endregion
                        }
                    }





                    //##### TODO - why not have this in the other block above #############################################################################################################################
                    //This works only if the trip/starts with an 8/ignition of/
                    //after getting the previous day igntion on/then we end the trip/event with the current ignition off
                    if (Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vReportID"]) == 8 && firstRecord == true)
                    {
                        //Here we end the trip with the current 8
                        currentVdometerReading = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);
                        //update the local variables 

                        _distance = currentVdometerReading - previousOdometerReading;
                        _endId = Convert.ToInt64(dsTripSummary.Tables[0].Rows[i]["ipkCommanTrackingID"]);
                        _endDateUtc = Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i]["dGPSDatetime"]);
                        _endLocation = Convert.ToString(dsTripSummary.Tables[0].Rows[i]["vTextMessage"]);


                        SecondLocationLongtude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vlongitude"]);
                        SecondLocationLattitude = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vLatitude"]);

                        EndSpeed = Convert.ToInt32(dsTripSummary.Tables[0].Rows[i]["vVehicleSpeed"]);
                        EndIgnitionStatus = Convert.ToBoolean(dsTripSummary.Tables[0].Rows[i]["bIsIgnitionOn"]);


                        _endOdometer = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);

                    }
                    previousEventId = (Convert.ToInt16(dsTripSummary.Tables[0].Rows[i]["vReportID"]));
                    //set the last ignition off ID
                    // LastIgnitionOffDatasetId = _ipkCommanTrackingID;
                    //This sets the last ignition on Dataset id for updating trip summary
                    //previousVedometerReading = Convert.ToDouble(dsTripSummary.Tables[0].Rows[i]["vOdometer"]);
                    firstIgnitionOnDatasetId = Convert.ToInt64(dsTripSummary.Tables[0].Rows[i]["ipkCommanTrackingID"]);  ///todo whats this for? ##############################################################
                    if (_isLastTripOfTheDay != true)
                    {
                        if ((Convert.ToInt16(dsTripSummary.Tables[0].Rows[i + 1]["vReportID"])) == 7)
                        {
                            nextIgnOnDateTime = Convert.ToDateTime(dsTripSummary.Tables[0].Rows[i + 1]["dGPSDatetime"]);
                        }
                    }
                    //check if this is the last record 
                    if (_isLastTripOfTheDay)
                    {
                        //is it currently ign on?
                        if (Convert.ToInt16(dsTripSummary.Tables[0].Rows[i]["vReportID"]) == 7)
                        {
                            if (_startDateUtc.Date == DateTime.Today)
                            {
                                //This trip is in progress : Mark it is inProgress
                                _isInProgress = true;


                                //Get the last position report for CurrentTrip/IsInProgress/LastRecord/CurrentDay
                                DataSet dsDaysLastPosition = new DataSet();
                                dsDaysLastPosition = objTripSummary.GetTripSummary_CurrentDaysLastPosition(_vpkDeviceID, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange));
                                if (dsDaysLastPosition.Tables[0].Rows.Count > 0)
                                {
                                    _endId = Convert.ToInt64(dsDaysLastPosition.Tables[0].Rows[0]["ipkCommanTrackingID"]);
                                    _endLocation = Convert.ToString(dsDaysLastPosition.Tables[0].Rows[0]["vTextMessage"]);


                                    SecondLocationLongtude = Convert.ToDouble(dsDaysLastPosition.Tables[0].Rows[0]["vlongitude"]);
                                    SecondLocationLattitude = Convert.ToDouble(dsDaysLastPosition.Tables[0].Rows[0]["vLatitude"]);

                                    EndSpeed = Convert.ToInt32(dsDaysLastPosition.Tables[0].Rows[0]["vVehicleSpeed"]);
                                    EndIgnitionStatus = Convert.ToBoolean(dsDaysLastPosition.Tables[0].Rows[0]["bIsIgnitionOn"]);


                                    _endDateUtc = Convert.ToDateTime(dsDaysLastPosition.Tables[0].Rows[0]["dGPSDatetime"]);
                                    _endOdometer = Convert.ToDouble(dsDaysLastPosition.Tables[0].Rows[0]["vOdometer"]);

                                    _distance = _endOdometer - previousOdometerReading;

                                }
                                else
                                {
                                    _endDateUtc = _startDateUtc;
                                }

                            }
                            else
                            {
                                //hack to get last record working ok without crashing ######

                                dsNextIgnOff = objTripSummary.GetTripSummary_NextsDaysIgnitionOff(_vpkDeviceID, Convert.ToDateTime(_strEndDateRange), Convert.ToDateTime(_strEndDateRange).AddDays(1));

                                //sp_GetTripSummary_NextDaysIgnitionOff
                                //if (check if first record tomorrow is a 8 - looks for instance of 7 and 8)
                                //{ use the data from the 8
                                //else
                                // use the below

                                if (dsNextIgnOff.Tables.Count > 0 && dsNextIgnOff.Tables[0].Rows.Count > 0)
                                {



                                    _endId = Convert.ToInt64(dsNextIgnOff.Tables[0].Rows[0]["ipkCommanTrackingID"]);
                                    _endLocation = Convert.ToString(dsNextIgnOff.Tables[0].Rows[0]["vTextMessage"]);


                                    SecondLocationLongtude = Convert.ToDouble(dsNextIgnOff.Tables[0].Rows[0]["vlongitude"]);
                                    SecondLocationLattitude = Convert.ToDouble(dsNextIgnOff.Tables[0].Rows[0]["vLatitude"]);

                                    EndSpeed = Convert.ToInt32(dsNextIgnOff.Tables[0].Rows[0]["vVehicleSpeed"]);
                                    EndIgnitionStatus = Convert.ToBoolean(dsNextIgnOff.Tables[0].Rows[0]["bIsIgnitionOn"]);


                                    _endDateUtc = Convert.ToDateTime(dsNextIgnOff.Tables[0].Rows[0]["dGPSDatetime"]);
                                    _endOdometer = Convert.ToDouble(dsNextIgnOff.Tables[0].Rows[0]["vOdometer"]);

                                    _distance = _endOdometer - previousOdometerReading;

                                    _isNetxDay = true;

                                }
                                else
                                {

                                    _endDateUtc = _startDateUtc;
                                    _endId = _startId;
                                    _endLocation = "End of trip missing";

                                    SecondLocationLongtude = 0;
                                    SecondLocationLattitude = 0;


                                    _distance = 0;
                                    _endOdometer = 0;


                                    //end hack ###########################
                                    //continue; // this essentially deletes trip

                                    ////TODO - do this propertly ###########################################################################################################################################
                                    ////see if first record tomorrow is 8
                                    //else
                                    //    //get last instance of bisign on for today
                                    //else
                                    //    //get last instance of bisign on before the first 7 tomorrow
                                    //else
                                    //    //use last position of the current day
                                    //else
                                    //    //discard

                                }




                            }
                        }
                    }


                    //Convert the trip start/end datetimes to users local timezone
                    _startDateLocal = Convert.ToDateTime(UserSettings.ConvertUTCDateTimeToLocalDateTime(_startDateUtc, timeZoneId));
                    _endDateLocal = Convert.ToDateTime(UserSettings.ConvertUTCDateTimeToLocalDateTime(_endDateUtc, timeZoneId));


                    _duration = _endDateLocal - _startDateLocal;



                    //add the packaged trip to the LIST object
                    if (_startId != 0)
                    {
                        // lstPopulateTripSummary.Add(new clsPopulateTripSummary(_startId, _endId, _startOdometer, _endOdometer, _startDateLocal, _endDateLocal, _startDateUtc, _endDateUtc, _startLocation, _endLocation, _distance, _vpkDeviceID, _deviceDbId, _isLastTripOfTheDay, _isInProgress, _isStoppageItem, _hours, _minutes, _seconds, tripStartDateTimeLocal));


                        //Lat/Long Reading
                        lstPopulateTripSummary.Add(new clsPopulateTripSummary(_startId, _endId, _startOdometer, _endOdometer, _startDateLocal, _endDateLocal, _startDateUtc, _endDateUtc, _startLocation, _endLocation, _distance, _vpkDeviceID, _deviceDbId, _isLastTripOfTheDay, _isInProgress, _isStoppageItem, _hours, _minutes, _seconds, tripStartDateTimeLocal, FirstLocationLongitude, FirstLocationLatitude, SecondLocationLongtude, SecondLocationLattitude, StartSpeed, StartIgnitionStatus, EndSpeed, EndIgnitionStatus, _duration, DriverID, _tripType, _isNetxDay));


                        _isLastTripOfTheDay = false;
                        firstRecord = false;
                    }

                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "Trips.cs", "CleanTripSummary()", ex.Message  + ex.StackTrace);

                LogError.RegisterErrorInLogFile("Trips.cs", ex.Message + ex.StackTrace, JsonConvert.SerializeObject(new {  companyId, deviceDbId, tripStartDateTimeLocal, tripEndDateTimeLocal, timeZoneId, includeStoppages }));
            }
            //Order the list by startdate column - to ensure that trip on progress is always the first in the list
            lstSortedList = lstPopulateTripSummary.OrderByDescending(x => x.StartDateUtc).ToList();

            return lstSortedList;
        }

        public DataSet GetPreliminaryInfo(int User_id, int report_criteria_id, int flag, int report_type_id, int Operation = 0)
        {
            DataSet ds = new DataSet();
          
            using (SqlConnection con = new SqlConnection(Connectionstring))
            {

                con.Open();
                SqlCommand sqlCmnd = new SqlCommand("Trip_ReportBasicData_Retrieval", con);
                try
                {
                    sqlCmnd.CommandTimeout = 300;
                    sqlCmnd.Parameters.Add("@Operation", SqlDbType.Int).Value = Operation;
                    sqlCmnd.Parameters.Add("@ReportCriteria_id", SqlDbType.BigInt).Value = report_criteria_id;
                    sqlCmnd.Parameters.Add("@User_id", SqlDbType.BigInt).Value = User_id;
                    sqlCmnd.Parameters.Add("@Flag", SqlDbType.Int).Value = flag;
                    sqlCmnd.Parameters.Add("@Report_id", SqlDbType.Int).Value = report_type_id;
                    sqlCmnd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCmnd);

                    dataAdapter.Fill(ds);
                    con.Close();
                  
                }
                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile( "DAL_GetTripSummary.cs", "GetPreliminaryInfo()", ex.Message  + ex.StackTrace);
                    
                }
                finally
                {
                    sqlCmnd.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }

            return ds;

        }

        public DataSet BAL_GetTripSummary_CurrentDaysLastPosition(long vpkDeviceID, string _strStartDateRange, string _strEndDateRange)
        {

            var objTripSummary = new DAL_GetTripSummary();

            var ds = objTripSummary.GetTripSummary_CurrentDaysLastPosition(vpkDeviceID, Convert.ToDateTime(_strStartDateRange), Convert.ToDateTime(_strEndDateRange));

            var dt_with_last_recod = new DataTable();

            foreach (DataTable dt in ds.Tables)
            {
                var _rows = dt.AsEnumerable().Where(p => Convert.ToBoolean(p["bIsIgnitionOn"]));

                dt_with_last_recod = dt.Clone();

                var _last_trip_of_day_row = dt_with_last_recod.NewRow();

                if (_rows.Count() > 0)
                {
                    _last_trip_of_day_row = _rows.FirstOrDefault();

                    dt_with_last_recod.Rows.Add(_last_trip_of_day_row.ItemArray);
                }
            }

            var _returndataset = new DataSet();

            _returndataset.Tables.Add(dt_with_last_recod);

            return _returndataset;
        }

        public DataSet GetDriversForCompany(int company_id)
        {
            DataSet ds = new DataSet();
           
            using (SqlConnection con = new SqlConnection(Connectionstring))
            {

                con.Open();
                SqlCommand sqlCmnd = new SqlCommand("sp_Get_company_drivers", con);
                try
                {

                    sqlCmnd.Parameters.Add("@companyid", SqlDbType.Int).Value = company_id;
                    sqlCmnd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCmnd);

                    dataAdapter.Fill(ds);
                    con.Close();
                  
                }
                catch (Exception ex)
                {
                  
                    LogError.RegisterErrorInLogFile("Trips.cs", "GetDriversForCompany()", ex.Message  + ex.StackTrace);

                    
                }
                finally
                {
                    sqlCmnd.Dispose();
                    con.Close();
                    con.Dispose();
                }
            }
            return ds;
        }

    }
}
