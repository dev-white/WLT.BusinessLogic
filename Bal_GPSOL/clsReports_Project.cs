using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.DataAccessLayer.DAL_Report;
using WLT.EntityLayer;
using System.Linq;
using WLT.DataAccessLayer.GPSOL;
using WLT.EntityLayer.Utilities;
using Newtonsoft.Json;
using System.Globalization;
using WLT.BusinessLogic.Bal_Notification;
using WLT.ErrorLog;
using WLT.DataAccessLayer.DAL;

namespace WLT.BusinessLogic.Bal_GPSOL
{

    #region html reports
    public class clsReports_Project
    {

        //   trips.GetDirtyTripSummaryForMultipleAssets(_imeistring.ToString(), dStartDate, dEndDate, TimeZoneID);



        private static wlt_Config _wlt_AppConfig;

        public static string Connectionstring;

      

        public clsReports_Project()
        {
            

            _wlt_AppConfig = AppConfiguration.GetAppSettings<wlt_Config>("wlt_config");

            Connectionstring = AppConfiguration.GetAppSettings<wlt_Config>("ConnectionStrings").wlt_WebAppConnectionString;

        }
             

        public static string GetTripSummary_GetMaxSpeedOfSpecificTrip(DateTime _startDate, DateTime _endDate, long VpkDeviceID, string TimeZoneID, int intMeasurementUnit, bool add_units = false)
        {

            var utcstart = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_startDate, TimeZoneID);

            var utcend = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_endDate, TimeZoneID);

            var mxSpeed = DAL_GetTripSummary.GetTripSummary_GetMaxSpeedOfSpecificTrip(utcstart, utcend, VpkDeviceID);

            return UserSettings.ConvertKMsToXx(intMeasurementUnit, mxSpeed, add_units, 2);

        }

        public static double GetTripSummary_GetMaxSpeedOfSpecificTripUsingDates(DateTime _startDate, DateTime _endDate, long VpkDeviceID, string TimeZoneID)
        {
            double _sample = 0.0;

            var utcstart = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_startDate, TimeZoneID);

            var utcend = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_endDate, TimeZoneID);

            var mxSpeed = DAL_GetTripSummary.GetTripSummary_GetMaxSpeedOfSpecificTrip(utcstart, utcend, VpkDeviceID);

            return Double.TryParse(mxSpeed, out _sample) ? Convert.ToDouble(mxSpeed) : 0.0;
        }


        public DataSet GetReportViolations(string TimeZoneID, int userid, int ifkReportTypeID, int ifkReportCriteriaID, int GetViolationsReport)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = DAL_Reports.GetViolationsReport(TimeZoneID, userid, ifkReportTypeID, ifkReportCriteriaID, GetViolationsReport);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "Get_Violation_()", ex.Message  + ex.StackTrace);
                
            }       

            return ds;

        }


        public DataSet GetReportEvents(string TimeZoneID, int userid, int ifkReportTypeID, int ifkReportCriteriaID, int GetViolationsReport)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = DAL_Reports.GetEventsReport(TimeZoneID, userid, ifkReportTypeID, ifkReportCriteriaID, GetViolationsReport);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile("clsReports_Project.cs", "Get_Violation_()", ex.Message + ex.StackTrace);

            }

            return ds;

        }


        public DataSet Get_AnalogReport(string TimeZoneID, int userid, int ReportType_id, int reportId, int operation, int DeviceID,int sensorType )
        {
            DataSet ds = new DataSet();
            try

               
            {
                var _DAL_Reports = new DAL_Reports();
                ds = _DAL_Reports.getAnalogData_FromDb(userid, reportId, operation, DeviceID, sensorType);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "Get_Violation_()", ex.Message  + ex.StackTrace);
                
            }
            finally
            {

            }


            return ds;

        }

        public DataSet GoGetExecutiveData(int userid, int reportId, int operation)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = DAL_Reports.goGetExecutiveSammary(userid, reportId, operation);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "GoGetExecutiveData()", ex.Message  + ex.StackTrace);
                
            }
            finally { }




            return ds;

        }

        public DataSet GoGetWeeklySummaryReport(int userid, int reportId, int operation)
        {
            DataSet ds = new DataSet();

            try
            {
                ds = DAL_Reports.GoGetWeeklySummaryReportdb(userid, reportId, operation);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "GoGetWeeklySummaryReport()", ex.Message  + ex.StackTrace);
                
            }
            finally { }




            return ds;

        }
        public DataSet GoGet_OverspeedReportCount(int userid, int reportId, int operation, DateTime date, string MultipleImei, int ifkGroupMID, int SpeedType)
        {
            DataSet ds = new DataSet();



            try
            {
                ds = DAL_Reports.GoGet_OverspeedReportCountdb(userid, reportId, operation, date, MultipleImei, ifkGroupMID, SpeedType);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "Get_Violation_()", ex.Message  + ex.StackTrace);
                
            }
            finally
            {

            }


            return ds;

        }

        public DataSet Go_GetMaintenanceReport(int userid, int reportId)
        {
            DataSet ds = new DataSet();

            try
            {

                ds = DAL_Reports.GoGetMaintenanceReport(userid, reportId);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "Go_GetMaintenanceReport()", ex.Message  + ex.StackTrace);
                
            }
            finally { }




            return ds;

        }


        public DataSet Go_GetWeeklySummaryReport_with_script(int userid, int reportId, double TotalMinutes)
        {
            DataSet ds = new DataSet();

            try
            {

                ds = DAL_Reports.Go_GetWeeklySummaryReport_with_scriptdb(userid, reportId, TotalMinutes);
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "GoGetWeeklySummaryReport()", ex.Message  + ex.StackTrace);
                
            }
            finally { }




            return ds;

        }


        public DataSet GetTranslatedEvents(int _operation, int _userId, int _languageId)
        {
            DataSet ds = new DataSet();

            try
            {

                ds = new DAL_Reports().GetReportLanguageCultureDetails(_operation, _userId, _languageId);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "GetTranslatedEvents()", ex.Message  + ex.StackTrace);
                
            }
            return ds;
        }

        public DataSet GetTranslatedEvents(int _operation, string vLanguageName, string EventIds)
        {
            DataSet ds = new DataSet();

            try
            {

                ds = new DAL_Reports().GetReportLanguageCultureDetails(_operation, vLanguageName, EventIds);

            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "GetTranslatedEvents()", ex.Message  + ex.StackTrace);
                
            }
            return ds;
        }

        public InstallationStatus GetInstallationWebHookDetails(EL_Installation __EL_Installation)
        {
            var _installationDetails = new InstallationStatus();

            var ds = new DAL_Reports().GetReportDeviceStatusDetails(__EL_Installation);

            int tableCount = 0;

            foreach (DataTable dt in ds.Tables)
            {
                if (tableCount == 0)
                {
                    foreach (DataRow row in dt.Rows)
                        _installationDetails.vDeviceStatusCheckUrl = Convert.ToString(row["vDeviceStatusCheckUrl"]);
                }

                if (tableCount == 1)
                {
                    foreach (DataRow row in dt.Rows)
                        _installationDetails.DeviceStatusLogs.Add(new EL_DeviceStatusCheck
                        {
                            CustomId = Convert.ToString(row["CustomId"]),
                            IMEI = Convert.ToString(row["imei"]),
                            LastReportedDate = row["dGPSDateTime"] == DBNull.Value ? DateTime.UtcNow : Convert.ToDateTime(row["dGPSDateTime"]),
                            Status = $"{Convert.ToBoolean(row["Status"])}",
                            DeviceCustomType = Convert.ToString(row["DeviceCustomType"]),
                            OwnerId = Convert.ToString(row["OwnerID"]),
                            HardwareID = Convert.ToString(row["HardwareID"])
                        });

                }

                tableCount++;
            }

            return _installationDetails;
        }

        public string getResellerSmtp(int ReportID)
        {
            DataSet ds = new DataSet();
            ResellerSmtp jObj = new ResellerSmtp();
            try
            {

                ds = DAL_Reports.Go_Getsmtpsettingfromdb(ReportID);



                foreach (DataRow row in ds.Tables[0].Rows)
                {

                    jObj.isOnTrial = Convert.ToBoolean(row["IsTrial"]);
                    jObj.IsSmtpEnabled = Convert.ToBoolean(row["IsResellerEmailEnabled"]);



                }
            }
            catch (Exception ex)
            {
                LogError.RegisterErrorInLogFile( "clsReports_Project.cs", "GoGetWeeklySummaryReport()", ex.Message  + ex.StackTrace);
                
            }
            return JsonConvert.SerializeObject(jObj); ;
        }




    }
    #endregion
    #region   Reports on the admin Section
    public class UIadminReports
    {
        static DataSet ds = new DataSet();

        public static string TrialGetConversiondata(EL_AdminDashboard _EL_AdminDashboard)
        {
            CalendarMonthNew(_EL_AdminDashboard);

            var timeDifferenceInMinutes = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _EL_AdminDashboard.TimeZoneID).Subtract(DateTime.UtcNow).TotalMinutes;

            ds = SuperAdminClass.go_GetAdminConversionData(_EL_AdminDashboard, timeDifferenceInMinutes);


            var TrialConversionData = new DataTable();

            foreach (DataTable dt in ds.Tables)
                TrialConversionData = dt;

            var MonthsList = GetMonths(_EL_AdminDashboard.UTCStartDate, _EL_AdminDashboard.UTCEndtDate, _EL_AdminDashboard.TimeZoneID);

            foreach (var month in MonthsList)
            {

                var row = TrialConversionData.NewRow();

                var NoOfMonths = (from m in ds.Tables[0].AsEnumerable()
                                  where Convert.ToDateTime(m.Field<DateTime>("MonthDateItem")).Year == month.Year &&
                                        Convert.ToDateTime(m.Field<DateTime>("MonthDateItem")).Month == month.Month
                                  select m).Count();

                row["MonthDateItem"] = month;
                row["trials"] = 0;
                row["full"] = 0;

                if (NoOfMonths == 0)
                    TrialConversionData.Rows.Add(row);

            }

            DataView dv = TrialConversionData.DefaultView;
            dv.Sort = "MonthDateItem ASC";

            ;

            return JsonConvert.SerializeObject(dv.ToTable());

        }

        public static List<DateTime> GetMonths(DateTime startDate, DateTime endDate, string TimeZoneId)
        {

            var currentDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(startDate, TimeZoneId);

            var _endDateUser = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(endDate, TimeZoneId);

            var returnedList = new List<DateTime>();

            while (currentDate <= _endDateUser)
            {
                returnedList.Add(currentDate);

                currentDate = currentDate.AddMonths(1);
            }
            return returnedList;

        }
        [Obsolete("This ia an old method")]
        public static Tuple<DateTime, DateTime> CalendarMonth(int _timeframe, string TimezoneID)
        {
            var TodayUserTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, TimezoneID);


            if (_timeframe == 1)
            {
                var startDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(TodayUserTime.Year, TodayUserTime.Month, 1), TimezoneID);
                var endDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime, TimezoneID);
                return new Tuple<DateTime, DateTime>(startDate, endDate);

            }
            else
            {
                var startDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddMonths(-_timeframe), TimezoneID);
                var endDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime, TimezoneID);
                return new Tuple<DateTime, DateTime>(startDate, endDate);
            }

        }


        public static void CalendarMonthNew(EL_AdminDashboard _EL_AdminDashboard)
        {
            var TodayUserTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, _EL_AdminDashboard.TimeZoneID);

            DateTime _returnStartDateValue = new DateTime(), _returnEndDateValue = new DateTime();

            if (_EL_AdminDashboard.ReportType == "active-trial-resellers")
            {
                if (_EL_AdminDashboard.Value == "Today")
                {
                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date, _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(1).AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }
                else if (_EL_AdminDashboard.Value == "Yesterday")
                {
                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(-1), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);


                }
                else if (_EL_AdminDashboard.Value == "7")
                {
                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(-6), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);


                }
                else if (_EL_AdminDashboard.Value == "14")
                {
                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(-13), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);


                }
                else if (_EL_AdminDashboard.Value == "30")
                {
                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(-29), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);


                }
                else if (_EL_AdminDashboard.Value == "0")
                {
                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddYears(-20), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }
            }

            else if (_EL_AdminDashboard.ReportType == "resellers-reporting-devices")
            {

                if (_EL_AdminDashboard.Value == "0")  //this month 
                {
                    var thisMonth = new DateTime(TodayUserTime.Year, TodayUserTime.Month, 1);

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(thisMonth, _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(1).AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);
                }
                else if (_EL_AdminDashboard.Value == "1")  //previous month 
                {
                    var thisMonth = new DateTime(TodayUserTime.Year, TodayUserTime.Month, 1);
                    var previousMonth = thisMonth.AddSeconds(-1);
                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(previousMonth.Year, previousMonth.Month, 1), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(previousMonth, _EL_AdminDashboard.TimeZoneID);

                }
                else if (_EL_AdminDashboard.Value == "2")  //last thiry   days  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(-30), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }
                else if (_EL_AdminDashboard.Value == "3")  //last three months  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddMonths(-3), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }
                else if (_EL_AdminDashboard.Value == "6")  //last six months  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddMonths(-6), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }

                else if (_EL_AdminDashboard.Value == "9")  //last nine months  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddMonths(-9), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }

                else if (_EL_AdminDashboard.Value == "12")  //last 12 months  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddMonths(-12), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }

                else if (_EL_AdminDashboard.Value == "15")  //last 15 months  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddMonths(-15), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }

                else if (_EL_AdminDashboard.Value == "18")  //last 18 months  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddMonths(-18), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }

                else if (_EL_AdminDashboard.Value == "21")  //last 21 months  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddMonths(-21), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }

                else if (_EL_AdminDashboard.Value == "24")  //last 24 months  
                {

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddMonths(-24), _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }

                else  // all the rest of the this month options  
                {
                    var _Today = DateTime.ParseExact(_EL_AdminDashboard.Value, "yyyy-MMM", CultureInfo.InvariantCulture);

                    var endMonth = _Today.AddMonths(1).AddSeconds(-1);

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(_Today, _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(endMonth, _EL_AdminDashboard.TimeZoneID);

                }




            }

            else if (_EL_AdminDashboard.ReportType == "conversion-graph")
            {
                if (_EL_AdminDashboard.Value == "1")  //this month 
                {
                    var thisMonth = new DateTime(TodayUserTime.Year, TodayUserTime.Month, 1);

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(thisMonth, _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(1).AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }
                else
                {
                    var ThisMonth = Convert.ToInt32(_EL_AdminDashboard.Value);

                    _returnStartDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddMonths(-ThisMonth).Date, _EL_AdminDashboard.TimeZoneID);
                    _returnEndDateValue = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date.AddDays(1).AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

                }


            }


            _EL_AdminDashboard.UTCStartDate = _returnStartDateValue;

            _EL_AdminDashboard.UTCEndtDate = _returnEndDateValue;

        }


        public static string GetResellerDashboardDetails(EL_AdminDashboard _EL_AdminDashboard)
        {

            var TodayUserTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow.AddDays(-1), _EL_AdminDashboard.TimeZoneID);
            _EL_AdminDashboard.UTCStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.Date, _EL_AdminDashboard.TimeZoneID);
            _EL_AdminDashboard.UTCEndtDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(TodayUserTime.AddDays(1).Date.AddSeconds(-1), _EL_AdminDashboard.TimeZoneID);

            DataTable dt = SuperAdminClass.GetResellerDashboardDetails(_EL_AdminDashboard).Tables[0];

            List<TrialTabledata> ResellerRecords = new List<TrialTabledata>();
            foreach (DataRow row in dt.Rows)
            {

                ResellerRecords.Add(new TrialTabledata
                {
                    ResellerName = Convert.ToString(row["Reseller_Name"]),
                    UserMail = row["UserMail"] == System.DBNull.Value ? "0" : Convert.ToString(row["UserMail"]),
                    LoginCount = row["LoginCount"] == System.DBNull.Value ? 0 : Convert.ToInt32(row["LoginCount"]),
                    DeviceCount = row["DevicesCount"] == System.DBNull.Value ? 0 : Convert.ToInt32(row["DevicesCount"]),
                    ReportingDeviceCount = row["DevicesReported"] == System.DBNull.Value ? 0 : Convert.ToInt32(row["DevicesReported"]),
                    TrialRemainingTime = row["Trial_Remaining"] == System.DBNull.Value ? 0 : Convert.ToInt32(row["Trial_Remaining"]),
                    ResellerID = Convert.ToString(row["ResellerId"]),
                    UserName = Convert.ToString(row["UserName"]),
                    Url = Convert.ToString(row["UrlAddress"])
                });

            }


            return JsonConvert.SerializeObject(ResellerRecords); ;

        }

        public static Tuple<DateTime, DateTime, double> calenderWeekHelper(int i, string TimeZoneId)
        {    //beginning Date instantiation
            var dStartDate = new DateTime();

            //get current utc datetime 
            var UtcNow = DateTime.UtcNow;

            //get current user local  time 


            DateTime userTimeNow = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(UtcNow, TimeZoneId);

            var timeDifference = userTimeNow.Subtract(UtcNow);

            //Create A starting Date for that month


            switch (i)
            {
                case 0:
                    //current month
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, 01), TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                case 1:
                    //previous month
                    var lastMonth = (new DateTime(userTimeNow.Year, userTimeNow.Month, 01)).AddSeconds(-1);
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(lastMonth.Year, lastMonth.Month, 1), TimeZoneId);
                    UtcNow = lastMonth;

                    break;

                case 2:
                    //Last 30 Days
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddDays(-29).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;

                case 3:
                    //Last 3 Months
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddMonths(-3).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;

                case 6:
                    //Last 3 Months
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddMonths(-6).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;


                case 9:
                    //Last 3 Months
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddMonths(-9).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;


                case 12:
                    //Last 3 Months
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddMonths(-12).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;

                case 15:
                    //Last 3 Months
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddMonths(-15).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;


                case 18:
                    //Last 3 Months
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddMonths(-18).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;


                case 21:
                    //Last 3 Months
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddMonths(-21).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;


                case 24:
                    //Last 3 Months
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(userTimeNow.AddMonths(-24).Date, TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;
                default:
                    dStartDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, 01), TimeZoneId);
                    UtcNow = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(new DateTime(userTimeNow.Year, userTimeNow.Month, userTimeNow.Day, 23, 59, 59), TimeZoneId);
                    break;



            }

            return new Tuple<DateTime, DateTime, double>(dStartDate, UtcNow, timeDifference.TotalMinutes);

        }

        public static string ShowReportingDevicesForResellers(EL_AdminDashboard _EL_AdminDashboard)
        {

            CalendarMonthNew(_EL_AdminDashboard);


            DataTable dt = SuperAdminClass.ReportingDevices(_EL_AdminDashboard).Tables[0];

            List<TrialTabledata> ResellerRecords = new List<TrialTabledata>();

            foreach (DataRow row in dt.Rows)
            {

                ResellerRecords.Add(new TrialTabledata
                {
                    ResellerName = Convert.ToString(row["Reseller_Name"]),
                    IsTrial = Convert.ToBoolean(row["IsTrial"]),
                    ReportingDeviceCount = row["ReportingDevices"] == System.DBNull.Value ? 0 : Convert.ToInt32(row["ReportingDevices"]),
                    ResellerID = Convert.ToString(row["ResellerId"])

                });

            }





            return JsonConvert.SerializeObject(ResellerRecords); ;

        }
    }

    #endregion

    #region Graph   Report on Distance Travelled

    public class bal_DashboardGraph
    {
        public int ClientId { get; set; }
        public static DataSet TempDS = new DataSet();
        //  DistanceTravelledGraphModel model = new DistanceTravelledGraphModel();

        public bal_DashboardGraph() { }
        public static string DashboardGraph(DistanceTravelledGraphModel model)
        {


            DataSet ds = FleetManagementDashboardClass.Go_getDeviceIdList(model.ClientId, model.UserId);

            string MeasurementUnt = null;
            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {

                MeasurementUnt = UserSettings.GetOdometerUnitName((int)ds.Tables[1].Rows[0]["ifkMeasurementUnit"]);

            }

            Tuple<DateTime, DateTime> datesTuple = calenderTimeHelper(model.Month, model.timeZoneId);




            StringBuilder deviceIdList_string = new StringBuilder();

            var DeviceList = ds.Tables[0].AsEnumerable().ToList();

            foreach (var id in DeviceList)
            {

                if (Convert.ToString(id["ImeiNumber"]) != null)
                {
                    deviceIdList_string.Append(id["ImeiNumber"].ToString() + ",");
                }

            }

            deviceIdList_string.Append(0);

            Trips trips = new Trips();

            TempDS = trips.GetDirtyTripSummaryForMultipleAssets(deviceIdList_string.ToString(), datesTuple.Item1, datesTuple.Item2, model.timeZoneId);


            DataTable MonthlyData = MonthlyDistanceMethod(DeviceList, datesTuple.Item1, datesTuple.Item2, model.timeZoneId, model.ClientId);


            List<DistanceTravelledGraphOutputModel> modelOutPut = new List<DistanceTravelledGraphOutputModel>();


            var TotalSum = Math.Round(MonthlyData.AsEnumerable().Sum(n => n.Field<double>("DailyTotalvOdometer")), 1);

            // 

            modelOutPut.Add(new DistanceTravelledGraphOutputModel { Unit = MeasurementUnt, OverrallTotalDistance = TotalSum, MonthlyAvg = UnitCalculation(TotalSum, DeviceList.Count, datesTuple).Item1, VehicleAvg = UnitCalculation(TotalSum, DeviceList.Count, datesTuple).Item2 });


            foreach (DataRow row in MonthlyData.Rows)
            {
                modelOutPut.Add(new DistanceTravelledGraphOutputModel { TotalDistance = (double)row[0], Date = (DateTime)row[1] });

            }

            return JsonConvert.SerializeObject(modelOutPut);


        }
        //processes monthly TotalDistance
        public static DataTable MonthlyDistanceMethod(List<DataRow> deviceList, DateTime startDate_, DateTime endDate_, string TimeZoneID, int CompanyId)
        {
            var startDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(startDate_, TimeZoneID);

            var endDate = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(endDate_, TimeZoneID);

            System.TimeSpan DateDiff = endDate - startDate;

            DateTime tempStartDate = startDate;

            DateTime tempEndDate = endDate;

            double cDay = 1;

            if (DateDiff.TotalDays > 30)
            {
                cDay = Convert.ToInt32(Math.Round(DateDiff.TotalDays / 30)) * 2;
            }



            DataTable dt = new DataTable("MonthlyData");

            dt.Columns.Add("DailyTotalvOdometer", typeof(double));

            dt.Columns.Add("dGPSDateTime", typeof(DateTime));

            dt.Columns.Add("ID", typeof(int));

            bool Flag = false;

            while (startDate < tempEndDate)
            {
                double Sum = 0;

                int id = 0;

                endDate = startDate.AddDays(cDay);

                Sum += GetTotalDistanceFromTripListingObject(deviceList, startDate, endDate, TimeZoneID, CompanyId);

                DataRow dr = dt.NewRow();

                dr["DailyTotalvOdometer"] = Math.Round(Sum, 2);

                dr["dGPSDateTime"] = startDate;

                dr["ID"] = id;

                dt.Rows.Add(dr);

                //Loop ( the while loop)  by increamenting a day

                // startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

                if (startDate.AddDays(cDay) > tempEndDate && Flag == false)
                {
                    System.TimeSpan diff = tempEndDate.Subtract(startDate);
                    if (diff.TotalDays > 1)

                        cDay = cDay / 2;

                    cDay = diff.TotalDays;

                    Flag = true;

                }

                startDate = startDate.AddDays(cDay).AddSeconds(1);

            }


            return dt;
        }

        //Calculate  calender Time
        public static Tuple<DateTime, DateTime> calenderTimeHelper(int i, string TimeZoneId)
        {    //beginning Date instantiation
            DateTime dStartDate = new DateTime();
            //Get users Today date
            var tempDate = DateTime.UtcNow.AddDays(-13);

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
            // var start = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(dStartDate, TimeZoneId);
            // var end = UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(CurrentDate, TimeZoneId);



            return new Tuple<DateTime, DateTime>(dStartDate, CurrentDate);

        }

        //Get data from object
        public static double GetTotalDistanceFromTripListingObject(List<DataRow> DeviceList, DateTime dStartDate_, DateTime dEndDate_, string TimeZoneID, int iCompanyid)
        {
            Trips trips = new Trips();

            var startDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(dStartDate_, TimeZoneID);

            var endDate = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(dEndDate_, TimeZoneID);

            double Distance = 0;

            DataSet ds = new DataSet();

            DataTable dt = new DataTable();


            foreach (var id in DeviceList)
            {
                if (TempDS.Tables[0].Rows.Count > 0)
                {

                    //dt = TempDS.Tables[0].AsEnumerable()
                    //                         .Where(r => r.Field<long>("vpkDeviceID") == Convert.ToInt64(id["ImeiNumber"]) && r.Field<DateTime>("dGPSDateTime") >= startDate && r.Field<DateTime>("dGPSDateTime") <= endDate).CopyToDataTable();

                    string filter = " (dGPSDateTime >= #" + startDate.ToString("yyyy/MM/dd HH:mm:ss") + "# And dGPSDateTime <= #" + endDate.ToString("yyyy/MM/dd HH:mm:ss") + "# ) ";

                    //  dt = TempDS.Tables[0].Copy();
                    TempDS.Tables[0].DefaultView.RowFilter = filter + " and vpkDeviceID  =" + Convert.ToInt64(id["ImeiNumber"]);
                    dt = TempDS.Tables[0].DefaultView.ToTable();



                }
                ds.Tables.Add(dt);

                
           

                Distance = Distance + trips.CleanDirtyTripSummary(ds, iCompanyid, 66565544444, startDate, endDate, TimeZoneID, false).Sum(n => n.Distance);
                ds.Tables.Clear();
            }
            return Distance;
        }
        public static Tuple<double, double> UnitCalculation(double TotalDistance, int TotaAssets, Tuple<DateTime, DateTime> dates)
        {

            System.TimeSpan dateDiff = dates.Item2.Subtract(dates.Item1);

            var AvgDistancePer = TotaAssets;

            var AvgDistance = Math.Round((TotalDistance / dateDiff.TotalDays), 1);


            var AvgDistancePerVehicle = Math.Round((TotalDistance / TotaAssets), 1);

            return new Tuple<double, double>(AvgDistance, AvgDistancePerVehicle);
        }



        //total distance covered by all assets from Db




    }

    #endregion

    #region SMTP reselller Settings
    public class ResellerSmtp
    {
        public bool isOnTrial { set; get; }
        public bool IsSmtpEnabled { set; get; }

    }
    #endregion
}
