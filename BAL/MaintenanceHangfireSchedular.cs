using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLT.DataAccessLayer.DAL;
using WLT.BusinessLogic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq.Expressions;
using WLT.ErrorLog;
using System.Web;
using System.Globalization;
using Newtonsoft;
using WLT.BusinessLogic.BAL;
using Newtonsoft.Json;
using Hangfire;
using System.IO;
using WLT.EntityLayer.Utilities;
using WLT.BusinessLogic.Bal_Notification;
//using Services;

namespace WLT.BusinessLogic
{

   
    public  class MaintenanceHangfireSchedular 
    {


        public MaintenanceHangfireSchedular() {


        }
        private const int refresh_rate = 10;
        public void ScheduleReports()
        {
           
            var scheduleReport  = new ScheduleReport();

            RecurringJob.RemoveIfExists("schedule-Reports");

            RecurringJob.AddOrUpdate("schedule-Reports", () => scheduleReport.CheckScheduledReports(), Cron.MinuteInterval(1));

        }

        public void Insert()
        {
            var conn = new SqlConnection(AppConfiguration.Getwlt_WebAppConnectionString());

            var cmd = new SqlCommand("insert into a_hangfire  values(1,'tonnie')", conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]      
        public object ScheduleMaintenanceBydate(int id,TimeSpan _TimeDifference)
        {         
            var _mailModel = new MailModel();    
           
            var jobID = BackgroundJob.Schedule(() => _mailModel.SendMaintenanceMail(id,true,2), _TimeDifference);
            
            return string.Empty;
        }


        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public object ScheduleMaintenanceByodometer(int iReminderType,string TimeZoneId )
        {
            var _mailModel = new MailModel();

            var queue_name = new string(Convert.ToString(AppConfiguration.AllConfigurations.GetSection("wlt_config")["HangfireServerName"]).Where(char.IsLetterOrDigit).ToArray()).ToLower();

            RecurringJob.RemoveIfExists("odometers_maintenance");

            RecurringJob.AddOrUpdate("odometers_maintenance",() => _mailModel.SendMaintenanceMail(0,false, iReminderType), Cron.MinuteInterval(5), TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId), queue_name);
                       
            
            return string.Empty;
        }

        [AutomaticRetry(Attempts = 5, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public object ScheduleMaintenanceByEngineHours(int iReminderType,string TimeZoneId )
        {
            var _mailModel = new MailModel();

            var queue_name = new string(Convert.ToString(AppConfiguration.AllConfigurations.GetSection("wlt_config")["HangfireServerName"]).Where(char.IsLetterOrDigit).ToArray()).ToLower();

            RecurringJob.RemoveIfExists("engineHours_Reminders");

            RecurringJob.AddOrUpdate("engineHours_Reminders", () => _mailModel.SendMaintenanceMail(0, false, iReminderType), Cron.MinuteInterval(5), TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId), queue_name);


            return string.Empty;
        }
    }
    


}