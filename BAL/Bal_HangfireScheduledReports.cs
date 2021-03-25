using Hangfire;
using Hangfire.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using WLT.BusinessLogic.Bal_GPSOL;
using WLT.BusinessLogic.Bal_Notification;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{
    public static class Bal_HangfireScheduledReports
    {

        public static string JobID = "";


        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]       
        public static void SchedulebyHangfire(int ifkReportID, string FrequencyParameters, string Frequency, DateTime StartingDate, int ifkUserID, string TimezoneID, int iDisplayTypeId, DateTime StartTime = new DateTime())
        {
         var reportsServerName =    Convert.ToString(AppConfiguration.AllConfigurations.GetSection("wlt_config")["HangfireReportsServerName"]);


            reportsServerName = new string(reportsServerName.Where(char.IsLetterOrDigit).ToArray()).ToLower();
                        

            var cron = "";

            var cronTime = StartTime.ToString("mm HH");

            if (iDisplayTypeId == 4)
            {
                switch (Frequency)
                {
                    case "Hour":

                        cron = "0 */" + FrequencyParameters + " * * *";


                        RecurringJob.AddOrUpdate($"Device Status Check({ifkReportID.ToString()})", () => SheduleReports(ifkReportID, null), cron, TimeZoneInfo.FindSystemTimeZoneById(TimezoneID), reportsServerName);

                        break;

                    case "Day":

                        if (String.IsNullOrEmpty(FrequencyParameters))
                            FrequencyParameters = "50 23 */1 * 0,1,2,3,4,5,6";

                        cron = cronTime + " */1 * " + FrequencyParameters;

                        RecurringJob.AddOrUpdate($"Device Status Check({ifkReportID.ToString()})", () => SheduleReports(ifkReportID, null), cron, TimeZoneInfo.FindSystemTimeZoneById(TimezoneID), reportsServerName);

                        break;

                    case "Week":
                        cron = cronTime + " * * 0";

                        RecurringJob.AddOrUpdate($"Device Status Check({ifkReportID.ToString()})", () => SheduleReports(ifkReportID, null), cron, TimeZoneInfo.FindSystemTimeZoneById(TimezoneID), reportsServerName);

                        break;

                    case "Fortnight":

                        BackgroundJob.Schedule(() => ScheduleAfterWait(ifkReportID), TimeSpan.FromDays(13));

                        break;

                    case "Month":

                        cron = cronTime + " 1 * *";

                        RecurringJob.AddOrUpdate($"Device Status Check({ifkReportID.ToString()})", () => SheduleReports(ifkReportID, null), cron, TimeZoneInfo.FindSystemTimeZoneById(TimezoneID), reportsServerName);

                        break;

                    default:

                        break;
                }
            }
            else
            {
                switch (Frequency)
                {
                    case "Hour":

                        cron = "0 */" + FrequencyParameters + " * * *";


                        RecurringJob.AddOrUpdate(ifkReportID.ToString(), () => SheduleReports(ifkReportID), cron, TimeZoneInfo.FindSystemTimeZoneById(TimezoneID), reportsServerName);

                        break;

                    case "Day":

                        if (String.IsNullOrEmpty(FrequencyParameters))
                            FrequencyParameters = "50 23 */1 * 0,1,2,3,4,5,6";

                        cron = cronTime + " */1 * " + FrequencyParameters;

                        RecurringJob.AddOrUpdate(ifkReportID.ToString(), () => SheduleReports(ifkReportID), cron, TimeZoneInfo.FindSystemTimeZoneById(TimezoneID), reportsServerName);

                        break;

                    case "Week":
                        cron = cronTime + " * * 0";

                        RecurringJob.AddOrUpdate(ifkReportID.ToString(), () => SheduleReports(ifkReportID), cron, TimeZoneInfo.FindSystemTimeZoneById(TimezoneID), reportsServerName);

                        break;

                    case "Fortnight":

                        BackgroundJob.Schedule(() => ScheduleAfterWait(ifkReportID), TimeSpan.FromDays(13));

                        break;

                    case "Month":

                        cron = cronTime + " 1 * *";

                        RecurringJob.AddOrUpdate(ifkReportID.ToString(), () => SheduleReports(ifkReportID), cron, TimeZoneInfo.FindSystemTimeZoneById(TimezoneID), reportsServerName);

                        break;

                    default:

                        break;
                }
            }

        }
        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]

        public static void SheduleReports(int _ifkReportID, PerformContext context)
        {
            //operation 2  get scheduled reports 
            var _DAL_Hangfire_Schedule_Report = new DAL_Hangfire_Scheduled_Report();

            var _scheduled_Reports = _DAL_Hangfire_Schedule_Report.ScheduledReports(5, _ifkReportID);

            int reportDisplayType = 0;

            var __EL_Installation = new EL_Installation();

            foreach (DataTable dt in _scheduled_Reports.Tables)
                foreach (DataRow dr in dt.Rows)
                {
                    reportDisplayType = Convert.ToInt32(dr["reportDisplayType"]);

                    __EL_Installation = new EL_Installation
                    {
                        ifkResellerId = Convert.ToInt32(dr["ResellerId"]),
                        vResellerName = Convert.ToString(dr["reseller"]),
                        webHookGroupingId = Convert.ToInt32(dr["ResellerId"]),
                        Operation = 217,
                        CustomId = _ifkReportID
                    };

                }



            if (reportDisplayType == 4)
            {
                LogError.RegisterErrorInLogFile("Bal_HangfireScheduledReports.cs", "SheduleReports()", $"Switching to webhook report {JsonConvert.SerializeObject(__EL_Installation)}");

                SendWebhookDeviceStatus(__EL_Installation, context);
            }
            else
            {
                LogError.RegisterErrorInLogFile("Bal_HangfireScheduledReports.cs", "SheduleReports()", $"Switching to normal report {JsonConvert.SerializeObject(__EL_Installation)}");

                _scheduled_Reports = _DAL_Hangfire_Schedule_Report.ScheduledReports(2, _ifkReportID);

                var _ClsSendMail = new ClsReportsMail();

                _ClsSendMail.SendReportMail(_scheduled_Reports);
            }



        }

        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]

        public static void SheduleReports(int _ifkReportID)
        {
            var _DAL_Hangfire_Schedule_Report = new DAL_Hangfire_Scheduled_Report();

            var _scheduled_Reports = _DAL_Hangfire_Schedule_Report.ScheduledReports(2, _ifkReportID);

            var _ClsSendMail = new ClsReportsMail();

            _ClsSendMail.SendReportMail(_scheduled_Reports);

        }

  
        [CleanupFilter]
        public static void SendWebhookDeviceStatus(EntityLayer.EL_Installation _EL_Installation, PerformContext context)
        {
            InstallationStatus _StatusDetails;

            var report = new clsReports_Project();

            var cached =  report.GetInstallationWebHookDetails(_EL_Installation);

            if (cached.vDeviceStatusCheckUrl != "" && cached.vDeviceStatusCheckUrl != null)
            {
                try
                {
                    var _DeviceStatusLogs = new
                    {
                        cached.DeviceStatusLogs
                    };


                    var json = JsonConvert.SerializeObject(_DeviceStatusLogs);

                    var location_content = new StringContent(json, Encoding.UTF8, "application/json");


                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        //HttpResponseMessage response = client.PostAsync(allTo, new FormUrlEncodedContent(content)).Result;

                        LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "SendWebhookDeviceStatus()", $" Attempting to send to {cached.vDeviceStatusCheckUrl} ({_DeviceStatusLogs.DeviceStatusLogs.Count}) items" );

                        HttpResponseMessage response = client.PostAsync(cached.vDeviceStatusCheckUrl, location_content).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "SendWebhookDeviceStatus()", response.ReasonPhrase);
                        }
                    }
                }

                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "SendWebhookDeviceStatus()", $"message: {ex.Message } :stacktrace:{ex.StackTrace}" );

                }
            }

        }


        public static void ScheduleAfterWait(int _ifkReportID)
        {
            SheduleReports(_ifkReportID, null);

            if (ReportExists(_ifkReportID))
            {
                BackgroundJob.Schedule(() => ScheduleAfterWait(_ifkReportID), TimeSpan.FromDays(13));
            }

        }


        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public static void DeleteJob(int ReportID)
        {
            RecurringJob.RemoveIfExists(ReportID.ToString());

            RecurringJob.RemoveIfExists($"Device Status Check({ReportID.ToString()})");


        }

        //save  a concatenated job with report id into the database
        public static bool ReportExists(int ReportID)
        {    //operation 1 check if report exists  

            var _DAL_Hangfire_Schedule_Report = new DAL_Hangfire_Scheduled_Report();

            var ds = _DAL_Hangfire_Schedule_Report.GetReport(1, ReportID);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static object UpdateHangFireWithScheduledReports()
        {
            var result = string.Empty;

            var _DAL_Hangfire_Schedule_Report = new DAL_Hangfire_Scheduled_Report();

            var scheduled_reportsDataset = _DAL_Hangfire_Schedule_Report.GetAbsentReports();


            if (scheduled_reportsDataset.Tables.Count > 0)
            {

                var scheduledReports = scheduled_reportsDataset.Tables[0];



                try
                {
                    foreach (DataRow row in scheduledReports.Rows)
                    {

                        int ifkReportID = Convert.ToInt32(row["ipkReportId"]);
                        string Days = "1,2,3,4,5,6,0";
                        string Frequency = Convert.ToString(row["Frequency"]);
                        DateTime StartingDate = new DateTime();
                        int ifkUserID = Convert.ToInt32(row["iUserId"]);
                        string TimezoneID = Convert.ToString(row["vTimeZoneID"]);


                        // please come chexk this place  later
                        SchedulebyHangfire(ifkReportID, Days, Frequency, StartingDate, ifkUserID, TimezoneID, 2);
                    }
                    result = "Success ";
                }

                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("Bal_HangfireSheduleReports.aspx.cs", "UpdateHangFireWithScheduledReports()", ex.Message + " : " + ex.StackTrace);
                    result = "Internal Execution Error";
                }



            }

            return result;

        }

        public static void SheduleJobToStartAsFrom(EL_Hangfire _EL_Hangfire)
        {
            var usertime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(_EL_Hangfire._sd, _EL_Hangfire.TimezoneID);   

            var scheduledDate = new DateTime(usertime.Year, usertime.Month, usertime.Day, _EL_Hangfire._st.Hour, _EL_Hangfire._st.Minute, _EL_Hangfire._st.Second);         

            var utc = DateTime.UtcNow;

            var userTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(utc, _EL_Hangfire.TimezoneID);

            var offset = userTime.Subtract(utc);

            DateTimeOffset enqueueAt = new DateTimeOffset(scheduledDate, offset);

            var timeDifference = enqueueAt.Subtract(DateTime.UtcNow);

            if (_EL_Hangfire.iReportOperation == 11)  // an update operation 
            {
                timeDifference = TimeSpan.FromSeconds(1);
            }
            else  //saving operation
            {
                if (timeDifference.TotalMinutes > 1)
                    timeDifference = timeDifference.Subtract(TimeSpan.FromMinutes(1));
                else
                    timeDifference = TimeSpan.FromMinutes(0);
            }

            _EL_Hangfire.StartingDate = scheduledDate;

            BackgroundJob.Schedule(() => SchedulebyHangfire(_EL_Hangfire.ifkReportID, _EL_Hangfire.FrequecyParameters, _EL_Hangfire.Frequency, _EL_Hangfire.StartingDate, _EL_Hangfire.ifkUserID, _EL_Hangfire.TimezoneID, _EL_Hangfire.iDisplayTypeId, scheduledDate),
                enqueueAt);




        }


    }
}
