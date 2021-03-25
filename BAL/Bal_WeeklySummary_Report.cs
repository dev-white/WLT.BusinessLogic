using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.BusinessLogic.Bal_GPSOL;
namespace WLT.BusinessLogic.BAL
{
    public class Bal_WeeklySummary_Report
    {
        public DataSet DS { get;  set; }
        public DataRow dr_header { get;  set; }
        public int UserId { get;  set; }
        public int ReportId { get;  set; }
        public int ReportTypeId { get; set; }

        public Mod_Weekly_Summary WeeklyModelObj { get; set; }
        public string TimeZoneID { get; set; }
        public Bal_WeeklySummary_Report()
        {


        }
        
        
        public Mod_Weekly_Summary HeaderData()
        {
            var MOD = new Mod_Weekly_Summary();

            var cls = new clsReports_Project();

            var TotalMinutes = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, this.TimeZoneID).Subtract(DateTime.UtcNow).TotalMinutes;

            DS = cls.Go_GetWeeklySummaryReport_with_script(UserId, ReportId, TotalMinutes);

             dr_header = DS.Tables[0].Rows[0];

            MOD.TimeZoneID = this.TimeZoneID;

            MOD.MeasurementUnit = Convert.ToInt32(dr_header["ifkMeasurementUnit"]);

            MOD.CompanyID = Convert.ToInt32(dr_header["ifkCompanyId"]);

            MOD.Asset = String.Format("{0}", Convert.ToString(dr_header["Asset"]));

            MOD.StartTime = Convert.ToDateTime(dr_header["dStartDate"]).ToString("yyyy- MM-dd HH:mm:ss");

            MOD.EndTime = Convert.ToDateTime(dr_header["dEndDate"]).ToString("yyyy- MM-dd HH:mm:ss");

            MOD.Logo = MOD.ShowReportHeader(DS.Tables[1]);           

            return MOD;

        }
        private string CreateCSV(List<DataRow> o, string seperator = ",")
        {
            var result = new StringBuilder();

            foreach (var item in o)
            {
                result.Append( item["vpkDeviceID"]);
                result.Append(seperator);
            }
            result.Append("0");

            return result.ToString();
        }
        public DataTable  CrossTabSource()
        {
            var MOD = new Mod_Weekly_Summary();

            var _contents = DS.Tables[2].Copy();

            var weeksInQuery = _contents.DefaultView.ToTable(true, "WEEK");

            var vpkDevices = CreateCSV( _contents.DefaultView.ToTable(true, "vpkDeviceID").AsEnumerable().ToList());

            var dt = MOD.CrossTab_SourceDefinition().Clone();

            //instantiate Trips Object
            var trips = new Trips();

           var _dirtyTrips   = trips.GetDirtyTripSummaryForMultipleAssets(vpkDevices.ToString(), Convert.ToDateTime(WeeklyModelObj.StartTime), Convert.ToDateTime(WeeklyModelObj.EndTime), TimeZoneID);

            var ifkCompany = WeeklyModelObj.CompanyID;


            foreach (DataRow Week in weeksInQuery.Rows)
            {
                var WeeklySpecificdata = _contents.AsEnumerable().Where(n => n.Field<Int32>("WEEK") == Convert.ToInt32(Week["WEEK"])).Distinct().ToList();

              

                foreach (var row in WeeklySpecificdata)
                {
                    //trips section  
                    var CleansummeryList = new List<clsPopulateTripSummary>();

                    var start = Convert.ToDateTime(row["startDate"]); start = new DateTime(start.Year, start.Month, start.Day, 0, 0, 0);

                    var EndLocaldate = Convert.ToDateTime(row["startDate"]).AddDays(6);

                    EndLocaldate = new DateTime(EndLocaldate.Year, EndLocaldate.Month, EndLocaldate.Day, 23, 59, 59);

                    TimeSpan sTotalSpan = new TimeSpan(0, 0, 0);

                    Dictionary<int, Tuple<string, TimeSpan, DateTime>> TablePopulator = new Dictionary<int, Tuple<string, TimeSpan, DateTime>>();

                    int index = 0;

                    var dateLookUp = DateLookUpDisctionary(Convert.ToDateTime(row["startDate"]));

                    foreach (var pair in dateLookUp)
                    {
                        DataRow drSource = dt.NewRow();
                        var standardTime = DateStart_End(pair.Value);


                        var _filteredData = _dirtyTrips.Tables[0].Copy();
                        _filteredData.DefaultView.RowFilter = "vpkDeviceID  =" + Convert.ToInt64(row["vpkDeviceID"]) + "AND " + string.Format(CultureInfo.InvariantCulture, "dGPSDateTime >= #{0}# AND dGPSDateTime <= #{1}#", UserSettings.ConvertLocalDateTimeToUTCDateTime(standardTime.Item1, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime(standardTime.Item2, TimeZoneID));
                        _filteredData = _filteredData.DefaultView.ToTable();

                        DataSet _dirtyDataSet = new DataSet();
                        _dirtyDataSet.Tables.Add(_filteredData);

                        var daySpecificData = trips.CleanDirtyTripSummary(_dirtyDataSet, ifkCompany, Convert.ToInt64(row["vpkDeviceID"]), standardTime.Item1, standardTime.Item2, TimeZoneID, false);
                        daySpecificData = trips.RemoveZeroDistanceFromCleanedTripSummary(0.01, daySpecificData, TimeZoneID);
                        var i = daySpecificData.Count;
                        if (i > 0)
                        {
                            if (daySpecificData[i - 1].StartDate < standardTime.Item1 && TablePopulator.Count == 0)
                            {
                                daySpecificData[i - 1].StartDate = standardTime.Item1;
                                daySpecificData[i - 1].Duration = daySpecificData[i - 1].EndDate - daySpecificData[i - 1].StartDate;
                            }
                            else if (daySpecificData[i - 1].StartDate < standardTime.Item1 && TablePopulator.Count > 0)
                            {

                                var diff = standardTime.Item1 - daySpecificData[i - 1].StartDate;

                                var temp = TablePopulator[index - 1];

                                TablePopulator[index - 1] = new Tuple<string, TimeSpan, DateTime>(temp.Item1, (temp.Item2 + diff), temp.Item3);

                                daySpecificData[i - 1].StartDate = standardTime.Item1;
                                daySpecificData[i - 1].Duration = daySpecificData[i - 1].EndDate - daySpecificData[i - 1].StartDate;
                            }

                        }
                        var day = daySpecificData.AsEnumerable().Select(x => x.Duration).Aggregate(TimeSpan.Zero, (subtotal, t) => subtotal.Add(t));

                        sTotalSpan = sTotalSpan.Add(day);


                        TablePopulator.Add(index, new Tuple<string, TimeSpan, DateTime>(pair.Key, day, pair.Value));

                        #region Distance  

                        drSource["Asset"] = WeeklyModelObj.SubString2(row["vDeviceName"].ToString());

                        drSource["vpkDeviceID"] = Convert.ToInt64(row["vpkDeviceID"]);

                        drSource["Distance"] = row["TotalDistance"];

                        drSource["StartsentDate"] = Convert.ToDateTime(row["startDate"]).ToString("dd/MMM/yyyy");

                        drSource["EndSentDate"] = Convert.ToDateTime(row["startDate"]).AddDays(6).ToString("dd/MMM/yyyy");

                        drSource["vOdometerStart"] = Math.Round(Convert.ToDouble(row["minOd"]), 1).ToString();

                        drSource["vOdometerEnd"] = Math.Round(Convert.ToDouble(row["maxOd"]), 1).ToString();

                        drSource["week"] = WeeklyModelObj.Ordinal(Convert.ToInt32(Week["WEEK"]));

                        drSource["unitcode"] = WeeklyModelObj.MeasurementUnit;

                        drSource["Unit"] = UserSettings.GetOdometerUnitName(WeeklyModelObj.MeasurementUnit);

                        drSource["TimeSpan"] = day;

                        var _dailyDistance = row[pair.Value.ToString("ddd")];                       

                        drSource["DailyDistance"] = _dailyDistance == DBNull.Value ? 0 : _dailyDistance;
                        
                        drSource["Day"] = pair.Value;

                       

                        if (sTotalSpan.Days > 0)
                        {
                            drSource["TotalTime"] = (sTotalSpan.Days * 24) + ":" + sTotalSpan.Minutes + ":" + sTotalSpan.Seconds;
                        }

                        else if (sTotalSpan.Days < 1)
                        {
                            drSource["TotalTime"] = sTotalSpan.Hours + ":" + sTotalSpan.Minutes + ":" + sTotalSpan.Seconds;
                        }


                        #endregion

                        index++;

                        dt.Rows.Add(drSource);
                    }
                    
                }
            }





            return dt;
            }
        private Dictionary<string, DateTime> DateLookUpDisctionary(DateTime DatetoResolve)
        {
            Dictionary<string, DateTime> DaysInWeek = new Dictionary<string, DateTime>();
            var lastday = DatetoResolve.AddDays(6);

            while (DatetoResolve <= lastday)
            {
                DaysInWeek.Add(DatetoResolve.ToString("ddd"), DatetoResolve);
                DatetoResolve = DatetoResolve.AddDays(1);
            }

            DaysInWeek = DaysInWeek.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            return DaysInWeek;

        }
        private Tuple<DateTime, DateTime> DateStart_End(DateTime date)
        {

            var items = new Tuple<DateTime, DateTime>(

                new DateTime(date.Year, date.Month, date.Day, 0, 0, 0),
                new DateTime(date.Year, date.Month, date.Day, 23, 59, 59)

                );
            
            return items;

        }
    }
    public class Bal_WSR  // weekly Summary report 
    {
        public DataSet DS { get; set; }        
        public DateTime dStartDate { get; set; }
        public DateTime dEndDate { get; set; }
        public DataRow dr_header { get; set; }
        public int UserId { get; set; }
        public int ReportId { get; set; }
        public int ReportTypeId { get; set; }

        
        public int iCompanyid { get; set; }

        public int MeasurementUnit { get; set; }
        public Mod_Weekly_Summary WeeklyModelObj { get; set; }
        public Bal_WSR()
        {
            DeviceItems = new List<DevicesjObjItem>();

            WeeklyModelObj = new Mod_Weekly_Summary();
        }
        public List<DevicesjObjItem> DeviceItems { get; set; }

        DataSet DirtyTripSummaryForMultipleAssets = new DataSet();

        public Mod_Weekly_Summary HeaderData( DataSet DS)
        {
            var MOD = new Mod_Weekly_Summary();

            var cls = new clsReports_Project();
    

            dr_header = DS.Tables[0].Rows[0];

            MOD.TimeZoneID = this.TimeZoneID;

            MOD.MeasurementUnit = Convert.ToInt32(dr_header["ifkMeasurementUnit"]);

            MOD.CompanyID = Convert.ToInt32(dr_header["ifkCompanyId"]);

            MOD.Asset = String.Format("{0}", Convert.ToString(dr_header["Asset"]));

            MOD.StartTime = Convert.ToDateTime(dr_header["dStartDate"]).ToString("yyyy- MM-dd HH:mm:ss");

            MOD.EndTime = Convert.ToDateTime(dr_header["dEndDate"]).ToString("yyyy- MM-dd HH:mm:ss");

            MOD.Logo = MOD.ShowReportHeader(DS.Tables[1]);

            return MOD;

        }
        public Dictionary<int, Tuple<DateTime, DateTime>> AvailableWeeksList {get;set;}
        public bool  isCustomRangeEnabled { get; set; }

        public OptionalDateRange optionalDateRange { get; set; }      

        public int iCustomRangeType { get; set; }

        public string TimeZoneID { get; set; }
        public DataSet GetRawData()
        {

            var _imeistring = "";

            var cls = new clsReports_Project();

            var TotalMinutes = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, this.TimeZoneID).Subtract(DateTime.UtcNow).TotalMinutes;

             DS = cls.Go_GetWeeklySummaryReport_with_script(UserId, ReportId, TotalMinutes);
                     

            int tableCount = 0;
             foreach(DataTable dt in DS.Tables)
            {
                if(tableCount==0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        optionalDateRange = JsonConvert.DeserializeObject<OptionalDateRange>(Convert.ToString(dr["vCustomTime"]));
                        isCustomRangeEnabled = Convert.ToBoolean(dr["isCustomDateEnabled"] ==DBNull.Value? false: dr["isCustomDateEnabled"]);
                        iCustomRangeType =   Convert.ToInt32(dr["iEnabledDateType"] == DBNull.Value ? false : dr["iEnabledDateType"]); 
                        MeasurementUnit = Convert.ToInt32(dr["ifkMeasurementUnit"]);
                        dStartDate = Convert.ToDateTime(dr["dStartDate"] == DBNull.Value ? default(DateTime): dr["dStartDate"]);
                        dEndDate = Convert.ToDateTime(dr["dEndDate"]);
                        iCompanyid = Convert.ToInt32(dr["ifkCompanyId"]);
                    }

                }
                if (tableCount == 2)
                {
                    var lsts = dt.Select().ToList();

                     _imeistring = CreateCSV(lsts);

                        DeviceItems = lsts.AsEnumerable().Select(p=> new DevicesjObjItem
                        {
                            vpkDeviceId = p.Field<long>("vpkDeviceID"),
                            DeviceName = p.Field<string>("AssetName"),
                        }).ToList();
                }

                    tableCount++;
            }

            Trips trips = new Trips();

             DirtyTripSummaryForMultipleAssets = trips.GetDirtyTripSummaryForMultipleAssets(_imeistring, dStartDate, dEndDate, TimeZoneID);


            SeparateDaysToWeeks(this.dStartDate,this.dEndDate);

            return DS;
        }
        private string CreateCSV(List<DataRow> o, string seperator = ",")
        {
            var result = new StringBuilder();

            foreach (var item in o)
            {
                result.Append(item["vpkDeviceID"]);
                result.Append(seperator);
            }
            result.Append("0");

            return result.ToString();
        }
        public void  SeparateDaysToWeeks( DateTime _start, DateTime _end)
        {
           var resultWeeksList = new Dictionary<int, Tuple<DateTime, DateTime>>();

            var totalDays = _end.Subtract(_start).TotalDays;

            var remainingDays = totalDays;

            var totalWeeks = totalDays / 7; 

            DateTime _startDateLocal = _start;

            DateTime _endDateLocal = new DateTime();

            int week = 0;

            for (week = 1; week <= totalWeeks; week++)
            {
                remainingDays -= 7;

                _endDateLocal = _startDateLocal.AddDays(7).AddSeconds(-1);

                resultWeeksList.Add(week, new Tuple<DateTime, DateTime>(_startDateLocal, _endDateLocal));

                _startDateLocal = _startDateLocal.AddDays(7);

            }

            if(remainingDays < 7)
            {
                resultWeeksList.Add(week, new Tuple<DateTime, DateTime>(_startDateLocal, _end));
            }

            AvailableWeeksList = resultWeeksList;

        }
        public Dictionary<int, Tuple<DateTime, DateTime>> ExtractDaysFromWeek(DateTime _start, DateTime _end)
        {
            var resultDayList = new Dictionary<int, Tuple<DateTime, DateTime>>();

            var totalDays = _end.Subtract(_start).TotalDays;

            var remainingDays = totalDays;           

            DateTime _startDateLocal = _start;

            DateTime _endDateLocal = new DateTime();

            int day = 0;

            for (day = 1; day <= totalDays; day++)
            {
                remainingDays --;

                _endDateLocal = _startDateLocal.AddDays(1).AddSeconds(-1);

                resultDayList.Add(day, new Tuple<DateTime, DateTime>(_startDateLocal, _endDateLocal));

                _startDateLocal = _startDateLocal.AddDays(1).Date;

            }

            if (remainingDays < 1)
            {
                resultDayList.Add(day, new Tuple<DateTime, DateTime>(_startDateLocal, _end));
            }

            return resultDayList;

        }


        public DataTable CalculateSummary( DateTime _start,DateTime _end)
        {
            var WeeklySummaryStructure = new List<WeeklyObj>();


            var dt = new DataTable();
            foreach (var deviceItem in DeviceItems)
            {
                for (int week=1;week <= AvailableWeeksList.Count;week ++)
                  {               
                    var weekDates = AvailableWeeksList[week];
              
                        var weeklySummaryStats = GetAWeeksTimeTotalStatistics(weekDates.Item1, weekDates.Item2, deviceItem.vpkDeviceId);

                        weeklySummaryStats.weekNo = week;

                        weeklySummaryStats.Device = new DevicesjObjItem {   DeviceName =   deviceItem.DeviceName,  vpkDeviceId = deviceItem.vpkDeviceId };

                        WeeklySummaryStructure.Add(weeklySummaryStats);
                }
               

            }


            dt.Merge(CrossTab_SourceDefinition(WeeklySummaryStructure));


            return dt;
        } 

        public void FlattenStructureSource( WeeklyObj weekData,ref DataTable dt)
        {
            var item = weekData;

            foreach (var day in item.Days)
            {
                var drSource = dt.NewRow();

                drSource["Asset"] = WeeklyModelObj.SubString2(item.Device.DeviceName);

                drSource["vpkDeviceID"] = item.Device.vpkDeviceId;

                drSource["Distance"] = Math.Round(item.weekOdoEnd - item.weekOdoStart, 2);

                drSource["StartsentDate"] = item.weekStartTime;

                drSource["EndSentDate"] = item.weekEndTime;

                drSource["vOdometerStart"] = Math.Round(item.weekOdoStart, 1).ToString();

                drSource["vOdometerEnd"] = Math.Round(item.weekOdoEnd, 1).ToString();

                drSource["week"] = WeeklyModelObj.Ordinal(item.weekNo);

                drSource["unitcode"] = MeasurementUnit;


                WeeklyModelObj.MeasurementUnit = MeasurementUnit;

                drSource["Unit"] = UserSettings.GetOdometerUnitName(WeeklyModelObj.MeasurementUnit);

                drSource["TimeSpan"] = TimeSpan.FromSeconds(day.Value.TotalDuration);

                var _dailyDistance = day.Value.totalDailyDistance;

                drSource["DailyDistance"] = Math.Round(_dailyDistance, 2);

                drSource["Day"] = day.Key;

                drSource["TotalTimeRangeDistance"] = item.TotalTimeRangeDistance;


                //if (sTotalSpan.Days > 0)
                //{
                //    drSource["TotalTime"] = (sTotalSpan.Days * 24) + ":" + sTotalSpan.Minutes + ":" + sTotalSpan.Seconds;
                //}

                //else if (sTotalSpan.Days < 1)
                //{
                //    drSource["TotalTime"] = sTotalSpan.Hours + ":" + sTotalSpan.Minutes + ":" + sTotalSpan.Seconds;
                //}



                dt.Rows.Add(drSource);
            }

          
        }
        public DataTable CrossTab_SourceDefinition(List<WeeklyObj>WeeklySummaryStructure)
        {            

            DataTable dt = new DataTable("DataTable");
            dt.Columns.Add("Distance", typeof(double));
            dt.Columns.Add("Asset", typeof(string));
            dt.Columns.Add("TotalTime", typeof(string));
            dt.Columns.Add("vOdometerStart", typeof(string));
            dt.Columns.Add("vOdometerEnd", typeof(string));
            dt.Columns.Add("vpkDeviceID", typeof(long));
            dt.Columns.Add("StartsentDate", typeof(string));
            dt.Columns.Add("EndSentDate", typeof(string));
            dt.Columns.Add("week", typeof(string));
            dt.Columns.Add("Unit", typeof(string));
            dt.Columns.Add("TimeSpan", typeof(TimeSpan));
            dt.Columns.Add("DailyDistance", typeof(double));
            dt.Columns.Add("unitcode", typeof(int));
            dt.Columns.Add("TotalTimeRangeDistance", typeof(double));

            
            //Days
            dt.Columns.Add("Day", typeof(DateTime));
            dt.Columns.Add("RealDay", typeof(DateTime));



          
            foreach( var  item in WeeklySummaryStructure)
            {
                FlattenStructureSource(item,ref dt);              

            }       

            return dt;
        }




        public WeeklyObj GetAWeeksTimeTotalStatistics(DateTime _start, DateTime _end, long _vpkDeviceID)
        {

            if (864403041444911 == _vpkDeviceID)
            {
                var e = "";
            }
            var weeklyObject = new WeeklyObj();

            var utc_start = UserSettings.ConvertLocalDateTimeToUTCDateTime(_start, TimeZoneID);
            var utc_end = UserSettings.ConvertLocalDateTimeToUTCDateTime(_end, TimeZoneID);
            try
            {

                // get all the week's 7 & 8s
                var rows = DirtyTripSummaryForMultipleAssets.Tables[0].Select("dGPSDateTime >=#" + utc_start + "#   AND dGPSDateTime <= #" + utc_end + "# and  vpkDeviceID = " + _vpkDeviceID + "");


                Trips trips = new Trips();

                var CleansummeryList = new List<clsPopulateTripSummary>();

               var ds = new DataSet();

                if (rows.Count() > 0)
                    ds.Tables.Add(rows.CopyToDataTable());
                else
                    ds.Tables.Add(new DataTable());


                CleansummeryList = trips.CleanDirtyTripSummary(ds, iCompanyid, _vpkDeviceID, _start, _end, TimeZoneID, false);

                var FinalList = trips.RemoveZeroDistanceFromCleanedTripSummary(0.01, CleansummeryList, TimeZoneID);

                var _DatefillteredTrips = new List<clsPopulateTripSummary>();


                // get individual    start and end day  pairs in a  given  week
                var daysList = ExtractDaysFromWeek(_start, _end);


                var WeeklyDuration = 0.0;

                var weeklyDistance = 0.0;

                foreach (var itemValue in daysList.Values)
                {      
                 
                  
                    var localStartDate = new DateTime();

                    var localEndDate = new DateTime(); 
                    
                    localStartDate = DateTime.ParseExact( $"{itemValue.Item1.ToString("yyyy-MM-dd")} {optionalDateRange.startTime}", "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);

                    localEndDate = DateTime.ParseExact($"{itemValue.Item2.ToString("yyyy-MM-dd")} {optionalDateRange.endTime}", "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);

                    var _Bal_FilterTrips = new Bal_FilterTrips( _vpkDeviceID, TimeZoneID, isCustomRangeEnabled);

                    _Bal_FilterTrips.FilterTrips(FinalList, localStartDate, localEndDate, iCustomRangeType);

                    WeeklyDuration += _Bal_FilterTrips.DailyDuration;

                    weeklyDistance += _Bal_FilterTrips.DailyDistance;     

                    weeklyObject.Days.Add(itemValue.Item1,new clsDays  { totalDailyDistance = _Bal_FilterTrips.DailyDistance, TotalDuration= _Bal_FilterTrips.DailyDuration });

                }

                weeklyObject.weekStartTime = _start;

                weeklyObject.weekStartTime = _end;

                var td = new DataTable();

               
                var deviceRawData = ds.Tables[0].AsEnumerable().Where(p => p.Field<long>("vpkDeviceID") == _vpkDeviceID);

                if (deviceRawData.Count() > 0) td = deviceRawData.CopyToDataTable();

                var rowsCount = td.Rows.Count;

                if (rowsCount > 0)
                {
                    weeklyObject.weekOdoStart =  Convert.ToDouble(td.AsEnumerable().FirstOrDefault()?["vOdometer"]??0.0)  ;// Convert.ToDouble(td.Rows[0]["vOdometer"]);

                    weeklyObject.weekOdoEnd = Convert.ToDouble(td.AsEnumerable().LastOrDefault()?["vOdometer"] ?? 0.0); // Convert.ToDouble(td.Rows[0]["endOdometer"]);                   

                }


                if (isCustomRangeEnabled)
                {
                    weeklyObject.TotalWeekDistance = weeklyDistance;
                    weeklyObject.TotalTimeRangeDistance = Math.Round(  weeklyObject.Days.Values.Sum(r=>r.totalDailyDistance),2);
                }
                else
                {
                   
                    weeklyObject.TotalWeekDistance = weeklyObject.weekOdoEnd - weeklyObject.weekOdoStart;
                    weeklyObject.TotalTimeRangeDistance = weeklyObject.TotalWeekDistance;
                }

            }

            catch (Exception ex)
            {
                var message = ex.Message;
            }



            return weeklyObject;
        }
             
    }
    public class OptionalDateRange
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
    }
    public class clsDays
    {
        public double TotalDuration { get; set; }
        public double totalDailyDistance { get; set; }
    }
    public class WeeklyObj
    {
       public  WeeklyObj()
        {
            Days = new Dictionary<DateTime, clsDays>();
            Device = new DevicesjObjItem();
        }
        public int weekNo { get; set; }

        public DevicesjObjItem Device { get; set; }
        public double weekOdoStart { get; set; }
        public double weekOdoEnd { get; set; }
        public double TotalWeekDistance { get; set; }

        public double TotalTimeRangeDistance { get; set; }
        public DateTime weekStartTime { get; set; }
        public DateTime weekEndTime { get; set; }
        public Dictionary <DateTime, clsDays> Days { get; set; }
      
    }

    public class Bal_FilterTrips
    {

       public  long VpkDeviceID { get; set; }

        public string TimeZoneID { get; set; }

        public double DailyDistance { get; set; }

        public double DailyDuration { get; set; }
        public bool  isCustomRangeEnabled { get; set; }
        public Bal_FilterTrips(  long _vpkDeviceID,string _TimeZoneID, bool _isCustomRangeEnabled )
        {
            TimeZoneID = _TimeZoneID;


            VpkDeviceID = _vpkDeviceID;

            isCustomRangeEnabled = _isCustomRangeEnabled;
        }
        public void FilterTrips(List<clsPopulateTripSummary> _trips, DateTime _start, DateTime _end, int filterType)
        {

            var endOfDay = _start.Date.AddDays(1).AddSeconds(-1);   //to get the exteme end of day 


            foreach ( var trip in _trips)
            {

              if ( (trip.StartDate.IsBetween(_start.Date , endOfDay)) || (trip.EndDate.IsBetween(_start.Date, endOfDay)))
                {
                    if (!isCustomRangeEnabled || (filterType == 1 && isCustomRangeEnabled))
                    {

                        if (!isCustomRangeEnabled)
                        {
                            _start = _start.Date;
                            _end = endOfDay;

                        }

                        // both x and y are in the range 
                        if (_start <= trip.StartDate && trip.EndDate <= _end)
                        {
                            DailyDuration += trip.Duration.TotalSeconds;
                            DailyDistance += trip.Distance;
                        }

                        // x is outside start time but y  is inside the range 
                        if (trip.StartDate <= _start && trip.EndDate <= _end && _start <= trip.EndDate)
                        {
                            var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_start, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.EndDate, TimeZoneID), VpkDeviceID);

                            DailyDuration += trip.EndDate.Subtract(_start).TotalSeconds;
                            DailyDistance += trip.EndOdometer - Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]);
                        }


                        // x is inside  but y  is outside  the timerange  range 
                        if (trip.StartDate >= _start && trip.EndDate >= _end && trip.StartDate <= _end)
                        {
                            var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.StartDate, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_end, TimeZoneID), VpkDeviceID);

                            DailyDuration += _end.Subtract(trip.StartDate).TotalSeconds;
                            DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - trip.StartOdometer;
                        }



                        //  x & y are outside timerange  range   meaning trip was the while day 
                        if (trip.StartDate <= _start && trip.EndDate >= _end)
                        {
                            var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_start, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_end, TimeZoneID), VpkDeviceID);

                            DailyDuration += _end.Subtract(_start).TotalSeconds;
                            DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]); ;
                        }

                    }

                    if (filterType == 2 && isCustomRangeEnabled)
                    {                        
                        
                        
                        // both x & y are belowe the lower bound 
                        if( trip.StartDate< _start && trip.EndDate< _start)
                        {
                            if (trip.StartDate < _start.Date)
                            {
                                DailyDuration += _start.Subtract(_start.Date).TotalSeconds;
                                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.StartDate.Date, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_start, TimeZoneID), VpkDeviceID);
                                DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]);
                            }
                            else
                            {
                                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.StartDate, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_start, TimeZoneID), VpkDeviceID);

                                DailyDuration += _start.Subtract(trip.StartDate).TotalSeconds;

                                DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - trip.EndOdometer;
                            }


                        }

                        // both x & y are above  the upper bound 
                        if (trip.StartDate >_end && trip.EndDate >_end)
                        {
                            if (trip.EndDate > endOfDay)
                            {
                             
                                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.StartDate, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(endOfDay, TimeZoneID), VpkDeviceID);
                                DailyDuration += endOfDay.Subtract(trip.StartDate).TotalSeconds;
                                DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]);
                            }
                            else
                            {
                                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.StartDate, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.EndDate, TimeZoneID), VpkDeviceID);

                                DailyDuration += trip.EndDate.Subtract(trip.StartDate).TotalSeconds;

                                DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - trip.StartOdometer;
                            }


                        }

                        //x is below the lower bound whereas y is above the upper bound 
                        if (trip.StartDate < _start && trip.EndDate > _end)
                        {


                            DailyDuration += _start.Subtract(trip.StartDate).TotalSeconds;
                            var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.StartDate, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_start, TimeZoneID), VpkDeviceID);
                            DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]);


                            DailyDuration += trip.EndDate.Subtract(_end).TotalSeconds;
                            var __details2 = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_end, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.EndDate, TimeZoneID), VpkDeviceID);
                            DailyDistance += Convert.ToDouble(__details2.Tables[0].Rows[1]["Odometer"]) - Convert.ToDouble(__details2.Tables[0].Rows[0]["Odometer"]);


                        }


                        // x is lower than the lower bound but y is in bwteen the bounds 
                        if (trip.StartDate < _start && trip.EndDate.IsBetween(_start, _end))
                        {
                            if (trip.StartDate < trip.StartDate.Date)
                            {

                                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.StartDate.Date, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_start, TimeZoneID), VpkDeviceID);
                                DailyDuration += _start.Subtract(trip.StartDate.Date).TotalSeconds;
                                DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]);
                            }
                            else
                            {
                                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.StartDate, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_start, TimeZoneID), VpkDeviceID);

                                DailyDuration += _start.Subtract(trip.StartDate).TotalSeconds;

                                DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - trip.StartOdometer;
                            }


                        }

                        // y is higher than the upper bound but x is in bwteen the bounds 
                        if (trip.EndDate > _end && trip.StartDate.IsBetween(_start, _end))
                        {
                            if (trip.EndDate > endOfDay)
                            {

                                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_end, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(endOfDay, TimeZoneID), VpkDeviceID);
                                DailyDuration += endOfDay.Subtract(_end).TotalSeconds;
                                DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]);
                            }
                            else
                            {
                                var __details = DAL_CurfewViolationReport.GetCurfewDetails(1, UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_end, TimeZoneID), UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(trip.EndDate, TimeZoneID), VpkDeviceID);

                                DailyDuration += trip.EndDate.Subtract(_end).TotalSeconds;

                                DailyDistance += Convert.ToDouble(__details.Tables[0].Rows[1]["Odometer"]) - Convert.ToDouble(__details.Tables[0].Rows[0]["Odometer"]);

                            }


                        }

                    }
                }
               
            }

        }


    }

 
}
