using Hangfire.Server;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using WLT.BusinessLogic.Admin_Classes;
using WLT.DataAccessLayer;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{


    #region General DTOs

    public class DevicesjObjItem
    {

        public string DeviceName { get; set; }
        public int TrackerType { get; set; }
        public Int64 vpkDeviceId { get; set; }
        public int ifkDeviceID { get; set; }
    }
    public class Alert
    {
        public string icon { get; set; }
        public string AlertName { get; set; }
        public int AlertPriority { get; set; }
        public int AlertCode { get; set; }



    }   
    public class ZoneClass
    {
        public string icon { get; set; }
        public string ZoneName { get; set; }
        public int ZoneType { get; set; }
        public string vGeofenceType { get; set; }
        public int zoneCode { get; set; }
        public string Asset { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public TimeSpan Duration { get; set; }
        public Int64 vpkDeviceID { get; set; }
        public string vEventName { get; set; }
        public int vEventID { get; set; }
        public bool IsComplete { get; set; }

        public double Lat { get; set; }
        public double Lon { get; set; }
    }
    public class EL_Dashboard_aob
    {

        public string ViolationType { get; set; }
        public int count { get; set; }
        public int code { get; set; }

        public int reportId { get; set; }




    }
    public class Widget_Trip
    {

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public string specificDate { get; set; }
        public object DeviceID { get; set; }

        public int No_of_Trips { get; set; }
        public int No_of_Assets { get; set; }

        public List<Widget_Trip> Data_List { get; set; }


        public Widget_Trip()
        {

            this.Data_List = new List<Widget_Trip>();
        }



    }
    public class Widget_Maintenance
    {
        public string Asset { get; set; }
        public string MaintenanceItem { get; set; }

        public int RemainingTime { get; set; }
        public double Set_ParameterValue { get; set; }

        public DateTime Set_ParameterValueDate { get; set; }
        public double Current_Odo { get; set; }
        public double Remaining_Distance { get; set; }
        public string vlogo { get; set; }
    }
    public class Route
    {
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? RoadSpeed { get; set; }
        public double Speed { get; set; }
        public long vpkDeviceID { get; set; }
    }
    public static class Bal_Helpers
    {
        public static Tuple<DateTime, DateTime> WeekIO(int iWeek, string TimeZoneID)
        {


            var userTime = UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(DateTime.UtcNow, TimeZoneID);

            userTime = new DateTime(userTime.Year, userTime.Month, userTime.Day);

            var startDate = new DateTime();

            var endDate = new DateTime();

            var daysInWk = 7 * iWeek;


            startDate = userTime.AddHours(-(((daysInWk * 24) / 2) - 1));

            endDate = userTime.AddHours(((daysInWk * 24) / 2));

            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);


            var item = new Tuple<DateTime, DateTime>(
                UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(startDate, TimeZoneID),
                 UserSettings.ConvertLocalDateTimeToUTCDateTime_DateFormat(endDate, TimeZoneID)
                );


            return item;
        }


    }

    #endregion


    #region Static Reports and scheduled reports
    public class ClsReportsMail
    {
        // code to include the smtp settings from server and not from config
        public string SmtpServer { set; get; }
        public int SmtpPort { set; get; }
        public string AuthEmail { set; get; }
        public string AuthPass { set; get; }
        public int EnableSSL { set; get; }
        public static int EmailEnabled { set; get; }
        public string ErrorMessage { set; get; }
        public int ResellerID { set; get; }
        public int ClientID { set; get; }
        public bool ShowErrorToReseller { get; set; }
        public bool IsTrial { get; set; }
        public bool ismailSent { get; set; }
        public string _vCultureInfoCode { get; set; }
        public string ReportFormat { get; set; }
        public string _AppUrl { get; set; }
        public string ScheduledReportId { get; set; }
        public string ReportId { get; set; }
        public string ReportTypeId { get; set; }
        public string TimeZoneId { get; set; }
        public string ToEmail { get; set; }
        public string ReportName { get; set; }
        public string UserId { get; set; }
        public string Resellername { get; set; }
        public DateTime SentDateTime { get; set; }


        public void SendReportMail(DataSet _ds)
        {
            EL_MyAccount _objparams = new EL_MyAccount();



            if (_ds.Tables.Count == 0 || _ds.Tables[0].Rows.Count == 0)
                return;


            try
            {
                BAL_MyAccount _objBALMyAccount = new BAL_MyAccount();

                var row_count = _ds.Tables[0].Rows.Count;

                var attachmentResults = new EL_ScheduledReport();

                for (int i = 0; i < row_count; i++)
                {
                    var _dr = _ds.Tables[0].Rows[i];

                    if (i == 0)
                    {
                        ScheduledReportId = Convert.ToString(_dr["ipkScheduledHistoryId"]);
                        ReportId = Convert.ToString(_dr["ipkReportId"]);
                        ReportTypeId = Convert.ToString(_dr["ifkReportTypeId"]);
                        TimeZoneId = Convert.ToString(_dr["vTimeZoneID"]);
                        ReportName = Convert.ToString(_dr["vReportName"]);
                        UserId = Convert.ToString(_dr["iUserId"]);
                        Resellername = Convert.ToString(_dr["reseller"]);
                        SentDateTime = Convert.ToDateTime(_dr["vSentDateTime"]);

                        SmtpServer = Convert.ToString(_dr["SMTPServer"]);
                        SmtpPort = Convert.ToInt32(_dr["SMTPPort"]);
                        AuthEmail = Convert.ToString(_dr["AuthEmail"]);
                        AuthPass = Convert.ToString(_dr["AuthPass"]);
                        EnableSSL = Convert.ToInt32(_dr["EnableSSL"]);
                        EmailEnabled = Convert.ToInt32(_dr["EmailEnabled"]);
                        ResellerID = Convert.ToInt32(_dr["ResellerID"]);
                        ClientID = Convert.ToInt32(_dr["ClientId"]);
                        IsTrial = Convert.ToBoolean(_dr["IsTrial"]);
                        _vCultureInfoCode = Convert.ToString(_dr["vCultureInfoCode"]);

                        ReportFormat = Convert.ToString(_dr["ReportFormat"]);

                        _AppUrl = AppConfiguration.Configuration().RemoteReportUrl + "/reports/MasterExportReport?ReportTypeId=" + ReportTypeId + "&ReportId=" + ReportId + " &TimeZoneID=" + TimeZoneId + "&UserId=" + UserId + "&CultureID=" + _vCultureInfoCode + "&ReportFormat=" + ReportFormat;
                    }

                    //keep adding subscribers to this report
                    ToEmail += $"{ Convert.ToString(_dr["vEmail"])};";
                }

                LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Attempting to send report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId} ", "SendReportMail()", "---");
                               
                     
                    if (SmtpServer == string.Empty)
                    {
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Unable to send report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId}  Smtp  SmtpServer value is empty ", "SendReportMail()", "---");

                        return;
                    }
                    if (AuthEmail == string.Empty)
                    {
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Unable to send report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId}  Smtp  AuthEmail value is empty ", "SendReportMail()", "---");

                        return;
                    }
                    if (Convert.ToString(SmtpPort) == string.Empty)
                    {
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Unable to send report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId}  Smtp  port value is empty ", "SendReportMail()", "---");

                        return;
                    }


                var _El_Report = new El_Report
                {
                    ReportId = Convert.ToInt64(ReportId),
                    TimeZoneID = TimeZoneId,
                    ReportTypeID = Convert.ToInt32(ReportTypeId),
                    UserId = Convert.ToInt32(UserId)
                };

                var _Bal_Reports = new Bal_Reports();

                var _customReportResult = _Bal_Reports.ReportsProfileChecker(_El_Report);

                if (_customReportResult.IsCustomGenerated)
                {
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Fragmenting   {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId} to  {_customReportResult.ReportResults.Count}  parts", "SendReportMail()", "---");


                    if (_customReportResult.ReportResults.Count == 0)
                        _customReportResult.ReportResults.Add(_customReportResult.ReportResult);

                    _customReportResult.ReportResults.ForEach(item =>
                    {

                        var _reportCriteria = _customReportResult.ReportCriteria;

                        ReportName = _reportCriteria.ReportName + _reportCriteria.dStartDate.ToString("dd-MM-yy HH:mm") + " to " + _reportCriteria.dEndDate.ToString("dd-MM-yy HH:mm");

                        var format = ReportFormatter("csv");

                        var attachement = new Attachment(new MemoryStream(item.FileContents), ReportName + ".csv", format);

                        ismailSent = SendMail3(ScheduledReportId, ReportId, ReportTypeId, TimeZoneId, ToEmail, ReportName, UserId, SentDateTime, Resellername, _vCultureInfoCode, ReportFormat, attachement);

                        if (ismailSent)
                            LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId} was successfully sent", "SendReportMail()", "---");

                        else
                            LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId} was unsuccessfully sent!!!!!!!!!!!!!!", "SendReportMail()", "---");

                    });
                }
                else
                {


                    
                    attachmentResults = GenerateReportAttachment(AppConfiguration.Configuration().OptimiseForOnPremise,_AppUrl, ReportId, ReportTypeId, TimeZoneId, ToEmail, ReportName, UserId, SentDateTime, Resellername, _vCultureInfoCode, ReportFormat);

                    if (attachmentResults is { } && attachmentResults.is_scheduled_enabled)
                    {
                      
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Initializing {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId}", "SendReportMail()", "---");

                        ismailSent = SendMail3(ScheduledReportId, ReportId, ReportTypeId, TimeZoneId, ToEmail, ReportName, UserId, SentDateTime, Resellername, _vCultureInfoCode, ReportFormat, attachmentResults.attachement as Attachment);

                        if (ismailSent)
                            LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId} was successfully sent", "SendReportMail()", "---");

                        else
                            LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId} was unsuccessfully sent!!!!!!!!!!!!!!", "SendReportMail()", "---");

                    }

                    else
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Report {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId} was failed  Error: {attachmentResults.error_message} ", "SendReportMail()", "---");

                }

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                ismailSent = false;

                LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Complete report failure  ", "SendReportMail()", "---");

            }
            finally
            {

                _objparams.wasEmailSentOk = ismailSent;
                _objparams.ScheduledReportId = 0;
                _objparams.op = 1;
                _objparams.ifk_ResellerId = ResellerID;
                _objparams.ifk_ClientId = Convert.ToInt32(ClientID);
                _objparams.errorMessage = ErrorMessage;
                _objparams.ShowErrorToReseller = ShowErrorToReseller;

                _objparams.Section = "Scheduled Reports";

                BAL_MyAccount _objBALMyAccounts = new BAL_MyAccount();

                _objBALMyAccounts.CheckSheduleReportHistory(_objparams);


                ///log the process to mail logs 
                try
                {
                    Bal_Report.SaveReportsSentMailLogs(new EL_ReportsEmailLog
                    {
                        ifkCompanyId = ClientID,
                        ifkAlertId = Convert.ToInt32(ReportId),
                        dSentDate = DateTime.UtcNow,
                        bSuccessful = ismailSent,
                        EmailMessage = ReportName,
                        EmailType = "scheduled reports",
                        SentTo = ToEmail,

                    });
                }
                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"  Error saving {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId}  to log reseller mailing logs", "GenerateReportAttachment()", "---");

                }



                //intentioanlly throw an exception              
                if (!ismailSent)
                {
                    #if DEBUG
                    Console.WriteLine("Debug version");

                    #else
                        throw new System.Exception("Throw an exception intentionally to leverage hangfire retry feature");
                    #endif

                }

            }
        }
     
        public string ReportFormatter( string ReportFormat)
        {
            string App_Type = MediaTypeNames.Application.Pdf;

            if (ReportFormat.ToUpper() == "XLSX")
                App_Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";


            else if (ReportFormat.ToUpper() == "PPTX")
                App_Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";


            else if (ReportFormat.ToUpper() == "CSV")
                App_Type = "text/csv";

            else if (ReportFormat.ToUpper() == "JSON")
                App_Type = MediaTypeNames.Application.Json;

            else if (ReportFormat.ToUpper() == "DOCX")
                App_Type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            return App_Type;
        }
        public EL_ScheduledReport GenerateReportAttachment(bool OptimiseForOnPremise, string AppUrl, string ReportId, string ReportTypeId, string TimeZoneId, string ToEmail, string ReportName, string UserId, DateTime SentDateTime, string _ResellerName, string _vCultureInfoCode, string ReportFormat)
        {
            EL_ScheduledReport _EL_ScheduledReport = new EL_ScheduledReport();

            String File_Name = $"{ReportName}.{ReportFormat.ToLower()}";

            string App_Type = ReportFormatter(ReportFormat);
            // Type Of Application

            Uri url;

            try
            {      
                url = new Uri(AppUrl, UriKind.Absolute);
            }
            catch (Exception ex)
            {
                var staticConfig =  @"https://demo.whitelabeltracking.com/reports/" + "MasterExportReport?ReportTypeId=" + ReportTypeId + "&ReportId=" + ReportId + " &TimeZoneID=" + TimeZoneId + "&UserId=" + UserId + "&CultureID=" + _vCultureInfoCode + "&ReportFormat=" + ReportFormat;

                if(OptimiseForOnPremise)
                    staticConfig = @"https://telematics.safaricombusiness.co.ke/reports/" + "MasterExportReport?ReportTypeId=" + ReportTypeId + "&ReportId=" + ReportId + " &TimeZoneID=" + TimeZoneId + "&UserId=" + UserId + "&CultureID=" + _vCultureInfoCode + "&ReportFormat=" + ReportFormat;


                url = new Uri(staticConfig, UriKind.Absolute);

                LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Message: {ex.Message} StackTrace {ex.StackTrace} {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId}  params: {JsonConvert.SerializeObject(new { AppUrl, ReportId, ReportTypeId, TimeZoneId, ToEmail, ReportName, UserId, SentDateTime, _ResellerName, _vCultureInfoCode, ReportFormat })}", "GenerateReportAttachment()", "---");


            }



            try
            {
                var client = new WebClient();

                var stream = client.OpenRead(url);

                WebHeaderCollection responseHeaders = client.ResponseHeaders;

                var is_report_empty = Convert.ToBoolean(responseHeaders.Get("Report-Empty"));

                //only send report mails when they have records (maintenance)


                if (ReportTypeId != "54" || (ReportTypeId == "54" && !is_report_empty))
                {
                    _EL_ScheduledReport.attachement = new Attachment(stream, File_Name, App_Type);

                    _EL_ScheduledReport.is_scheduled_enabled = true;
                }
                else
                {
                    _EL_ScheduledReport.error_message = "This report is a maintenace report is empty";
                }

            }
            catch (Exception ex)
            {
                _EL_ScheduledReport.is_scheduled_enabled = false;
                _EL_ScheduledReport.error_message = ex.Message + " " + ex.StackTrace;


                LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", $"Message: {ex.Message} StackTrace {ex.StackTrace} {ReportName}({ReportId}) from   {Resellername}({ResellerID}) for client id ({ClientID})  to {ToEmail} {UserId}  params: {JsonConvert.SerializeObject(new { AppUrl, ReportId, ReportTypeId, TimeZoneId, ToEmail, ReportName, UserId, SentDateTime, _ResellerName, _vCultureInfoCode, ReportFormat })}", "GenerateReportAttachment()", "---");

            }



            return _EL_ScheduledReport;

        }



        public bool SendMail2(string ScheduledReportId, string ReportId, string ReportTypeId, string TimeZoneId, string ToEmail, string ReportName, string UserId, DateTime SentDateTime, string _ResellerName, string _vCultureInfoCode, string ReportFormat, Attachment _Attachment)
        {
            var is_mailSent_successfully = false;

            //update the database before even sending to signal attempt 
            BAL_MyAccount _objBALMyAccount = new BAL_MyAccount();

            EL_MyAccount _objELMyAccount = new EL_MyAccount();

            _objELMyAccount.ScheduledReportId = Convert.ToInt32(ScheduledReportId);

            _objELMyAccount.Status = "1";

            //function call to get scheduled reports data from databse
            DataSet _Ds = _objBALMyAccount.UpdateScheduledReportData(_objELMyAccount);


            // We have to write code here for logic to send mail 


            string msgBody = "";
            msgBody = "Hi ,<br><br>  Please find attached your scheduled report <br><br> This report was scheduled on " + UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(SentDateTime, TimeZoneId).ToString("ddd dd, MMM yyyy hh:mm tt", new CultureInfo(_vCultureInfoCode));
            String to = ToEmail;

            // Mail Subject
            String Subject = _ResellerName;  //"Scheduler Report Mailer";
                                             // Name fo Pdf File 
                                             // String File_Name =   ReportName + ".pdf";

            String File_Name = $"{ReportName}.{ReportFormat.ToLower()}";



            // Type Of Application
            String App_Type = ReportFormatter(ReportFormat);

           
            if (EmailEnabled == 1)
            {
                try
                {
                    ErrorMessage = WLT.EmailHelper.Email.SendMail2(SmtpServer, AuthEmail, AuthPass, Convert.ToString(to), SmtpPort,
                         Subject, msgBody, Convert.ToBoolean(EnableSSL), true, File_Name, App_Type, ReportTypeId,
                         ReportId, ReportName, ResellerID.ToString(), _ResellerName, _Attachment);



                    is_mailSent_successfully = ErrorMessage == string.Empty ? true : false;

                    ShowErrorToReseller = true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", "SendMail()", ex.Message + ex.StackTrace);

                }

            }
            else
            {
                if (IsTrial == true)
                {

                    try
                    {
                        ErrorMessage = WLT.EmailHelper.Email.SendMail2(AppConfiguration.Configuration().SmtpServer, AppConfiguration.Configuration().FromAddress,
                         AppConfiguration.Configuration().Password, Convert.ToString(to), AppConfiguration.Configuration().SmtpPort,
                        Subject, msgBody, AppConfiguration.Configuration().Ssl, true, File_Name, App_Type, ReportTypeId,
                         ReportId, ReportName, ResellerID.ToString(), _ResellerName, _Attachment);


                        is_mailSent_successfully = ErrorMessage == string.Empty ? true : false;

                        ShowErrorToReseller = false;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", "User on trial", "SendMail()", ex.Message + ex.StackTrace);
                    }
                }
                else
                {

                    ErrorMessage = "This Account is no longer on Trial and Email is not Enabled on scheduled Reports";

                    is_mailSent_successfully = false;

                    ShowErrorToReseller = true;

                    var error_message = $"Could not send to  {ToEmail} because the parent  trial account has expired";

                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", error_message, "SendMail()", "");

                }
            }


            BAL_MyAccount _objBALMyAccounts = new BAL_MyAccount();

            _objBALMyAccounts.UpdateScheduledReports(3, Convert.ToInt32(ReportId), is_mailSent_successfully, ErrorMessage, ResellerID, ClientID, ShowErrorToReseller);


            return is_mailSent_successfully;
        }


        //with mail kit 
        public bool SendMail3(string ScheduledReportId, string ReportId, string ReportTypeId, string TimeZoneId, string ToEmail, string ReportName, string UserId, DateTime SentDateTime, string _ResellerName, string _vCultureInfoCode, string ReportFormat, Attachment _Attachment)
        {
            var is_mailSent_successfully = false;

            //update the database before even sending to signal attempt 
            BAL_MyAccount _objBALMyAccount = new BAL_MyAccount();

            EL_MyAccount _objELMyAccount = new EL_MyAccount();

            _objELMyAccount.ScheduledReportId = Convert.ToInt32(ScheduledReportId);

            _objELMyAccount.Status = "1";

            //function call to get scheduled reports data from databse
            DataSet _Ds = _objBALMyAccount.UpdateScheduledReportData(_objELMyAccount);


            // We have to write code here for logic to send mail 


            string msgBody = "";
            msgBody = "Hi ,<br><br>  Please find attached your scheduled report <br><br> This report was scheduled on " + UserSettings.ConvertUTCDateTimeToLocalDateTime_DateFormat(SentDateTime, TimeZoneId).ToString("ddd dd, MMM yyyy hh:mm tt", new CultureInfo(_vCultureInfoCode));
            String to = ToEmail;

            // Mail Subject
            String Subject = _ResellerName;  //"Scheduler Report Mailer";
                                             // Name fo Pdf File 
                                             // String File_Name =   ReportName + ".pdf";

            String File_Name = $"{ReportName}.{ReportFormat.ToLower()}";



            // Type Of Application
            String App_Type = ReportFormatter(ReportFormat);


            if (EmailEnabled == 1)
            {
                try
                {
                    ErrorMessage = WLT.EmailHelper.Email.SendMail3(SmtpServer, AuthEmail, AuthPass, Convert.ToString(to), SmtpPort,
                         Subject, msgBody, Convert.ToBoolean(EnableSSL), true, File_Name, App_Type, ReportTypeId,
                         ReportId, ReportName, ResellerID.ToString(), _ResellerName, _Attachment);



                    is_mailSent_successfully = ErrorMessage == string.Empty ? true : false;

                    ShowErrorToReseller = true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", "SendMail()", ex.Message + ex.StackTrace);

                }

            }
            else
            {
                if (IsTrial == true)
                {

                    try
                    {
                        ErrorMessage = WLT.EmailHelper.Email.SendMail3(AppConfiguration.Configuration().SmtpServer, AppConfiguration.Configuration().FromAddress,
                         AppConfiguration.Configuration().Password, Convert.ToString(to), AppConfiguration.Configuration().SmtpPort,
                        Subject, msgBody, AppConfiguration.Configuration().Ssl, true, File_Name, App_Type, ReportTypeId,
                         ReportId, ReportName, ResellerID.ToString(), _ResellerName, _Attachment);


                        is_mailSent_successfully = ErrorMessage == string.Empty ? true : false;

                        ShowErrorToReseller = false;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", "User on trial", "SendMail()", ex.Message + ex.StackTrace);
                    }
                }
                else
                {

                    ErrorMessage = "This Account is no longer on Trial and Email is not Enabled on scheduled Reports";

                    is_mailSent_successfully = false;

                    ShowErrorToReseller = true;

                    var error_message = $"Could not send to  {ToEmail} because the parent  trial account has expired";

                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "Reports", error_message, "SendMail()", "");

                }
            }


            BAL_MyAccount _objBALMyAccounts = new BAL_MyAccount();

            _objBALMyAccounts.UpdateScheduledReports(3, Convert.ToInt32(ReportId), is_mailSent_successfully, ErrorMessage, ResellerID, ClientID, ShowErrorToReseller);


            return is_mailSent_successfully;
        }



        public  void  ScheduleSpecificReport(int _ifkReportID, PerformContext context)
        {
            var dd = context;
        }
    }

    #endregion


    #region Maintenance
    public class MailModel
    {
        public string smtpServer { get; set; }
        public string from { get; set; }
        public string password { get; set; }
        public string to { get; set; }
        public int port { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public bool enableSsl { get; set; }
        public bool isBodyHtml { get; set; }
        public Uri pdfAttachment { get; set; }
        public String FileName { get; set; }
        public String MimeType { get; set; }     
        public string vMessageBody { get; set; }

        public object custom { get; set; }

        public bool ismailSent { get; set; }

        

        public List<MailModel> vMail_list { get; set; }
        public bool IsSSLEnabled { get; private set; }
        public string MaintenanceName { get;  set; }
        public string MaintenanceAsset { get;  set; }
        public string ClientName { get;  set; }
        public int ResellerId { get;  set; }
        public string ResellerName { get;  set; }
        public int ClientId { get;  set; }

        public MailModel()
        {

            this.vMail_list = new List<MailModel>();
        }
        public List<MailModel> DeriveMaintenanceMailingItems(DataSet _ds)
        {

            foreach (DataRow _dr in _ds.Tables[0].Rows)
            {
                var mlMdl = new MailModel();

                try
                {
                    var isOnTrial = Convert.ToBoolean(_dr["IsTrial"]);

                        mlMdl.to = Convert.ToString(_dr["NotifyEmailAddress"]);
                        mlMdl.MaintenanceName = Convert.ToString(_dr["MaintenanceItemName"]);
                        mlMdl.MaintenanceAsset = Convert.ToString(_dr["Asset"]);
                        mlMdl.ClientId = Convert.ToInt32(_dr["ClientId"]);
                        mlMdl.ClientName = Convert.ToString(_dr["vCompanyName"]);
                        mlMdl.ResellerId = Convert.ToInt32(_dr["ResellerId"]);
                        mlMdl.ResellerName = Convert.ToString(_dr["ResellerName"]);
                        mlMdl.custom = _dr["Id"];
                        mlMdl. vMessageBody = (Convert.ToInt16(_dr["ParameterToCheck"]) == 1) ? "Your reminder '" + mlMdl.MaintenanceName + "' set for asset " + Convert.ToString(_dr["Asset"]) + " scheduled to report when odometer is " + Convert.ToString(_dr["ParameterValue"]) + " is now due" : "Your maintenance reminder '" + mlMdl.MaintenanceName + "' for asset " + mlMdl.MaintenanceAsset + " is now due";
                    


                    if (isOnTrial)
                    {
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "DeriveMaintenanceMailingItems()", $"Getting {mlMdl.ResellerName}  ({mlMdl.ResellerId})  Trial SMTP  config");

                        var _config = AppConfiguration.Configuration();

                        mlMdl.smtpServer = _config.SmtpServer;

                        mlMdl.port = _config.SmtpPort;

                        mlMdl.from = _config.FromAddress;

                        mlMdl.password = _config.Password;

                        mlMdl.enableSsl = _config.Ssl;

                    }
                    else
                    {
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "ParseToModel()", $"Getting {mlMdl.ResellerName}  ({mlMdl.ResellerId})  SMTP  config");


                        mlMdl.smtpServer = Convert.ToString(_dr["SMTPServer"]);

                        mlMdl.port = Convert.ToInt32(_dr["SMTPPort"]);

                        mlMdl.from = Convert.ToString(_dr["SMTPEmail"]);

                        mlMdl.password = Convert.ToString(_dr["SMTPpass"]);

                        mlMdl.enableSsl = Convert.ToBoolean(_dr["SMTPEnableSSL"]);


                    }

                    this.vMail_list.Add(mlMdl);
                }
                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "DeriveMaintenanceMailingItems()", $"Failed during parsing    Reseller: {mlMdl.ResellerName}({mlMdl.ResellerId}) Client:=> {mlMdl.ClientName } ({mlMdl.ClientId}) : Message:" + ex.Message + ex.StackTrace);

                }
            }
                      

            return this.vMail_list;
        }

        [Obsolete("this an old .NET Smtp client")]
        public void SendMaintenanceMail_smptp(int id, bool _single, int iReminderType)
        {          

            var _maintenance = new DAL_Maintenance();

            LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", "Attempting to get maintenace items to send");

            var _ds = _maintenance.GetMaintenance(id, _single, iReminderType);

            var mail_list_ = this.DeriveMaintenanceMailingItems(_ds);

            LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"{mail_list_.Count()} Maintenance items found");


            foreach (var mailingItem in mail_list_)
            {

                MailMessage mymail = new MailMessage();

                SmtpClient mySmtp = new SmtpClient(mailingItem.smtpServer);

                mySmtp.UseDefaultCredentials = false;

                mySmtp.Host = mailingItem.smtpServer;

                mySmtp.Credentials = new NetworkCredential(mailingItem.from, mailingItem.password);

                mySmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                mySmtp.Port = mailingItem.port;

                mymail.From = new MailAddress(mailingItem.from, "MAINTANANCE NOTIFICATION");


                mymail.To.Add(mailingItem.to);

                mySmtp.EnableSsl = mailingItem.enableSsl;

                mymail.Subject = "Reminder";

                mymail.Body = mailingItem.vMessageBody;

                mymail.Priority = MailPriority.Normal;

                mymail.DeliveryNotificationOptions = DeliveryNotificationOptions.Delay | DeliveryNotificationOptions.OnFailure;

                LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"Sending  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) ...");

                try
                {
                    mySmtp.Send(mymail);

                    _maintenance.UpdateMaintenance(Convert.ToInt32(mailingItem.custom));

                    mailingItem.ismailSent = true;

                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId})  was sent successfully!");

                }

                catch (ArgumentException ex)
                {
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");
                
                }
                catch (TimeoutException ex)
                {

                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");
                                        
                }
                catch (SmtpException ex)
                {
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");
                   
                }

                finally
                {

                    try
                    {
                        Bal_Report.SaveReportsSentMailLogs(new EL_ReportsEmailLog
                        {
                            ifkCompanyId = mailingItem.ClientId,
                            ifkAlertId = Convert.ToInt32(mailingItem.custom),
                            dSentDate = DateTime.UtcNow,
                            bSuccessful = mailingItem.ismailSent,
                            EmailMessage = $"{mailingItem.MaintenanceName} ({mailingItem.MaintenanceAsset})",
                            EmailType = "maintenance mail",
                            SentTo = mailingItem.to,

                        });
                    }
                    catch(Exception ex)
                    {
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail() > SaveReportsSentMailLogs()", $"Maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");

                    }
                }

            }

        }


        public void SendMaintenanceMail(int id, bool _single, int iReminderType)
        {
            var _maintenance = new DAL_Maintenance();

            LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", "Attempting to get maintenace items to send");

            var _ds = _maintenance.GetMaintenance(id, _single, iReminderType);

            var mail_list_ = this.DeriveMaintenanceMailingItems(_ds);

            LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"{mail_list_.Count()} Maintenance items found");


            foreach (var mailingItem in mail_list_)
            {             

                var _Mailmessage = new MimeMessage();

                var builder = new BodyBuilder { HtmlBody = mailingItem.vMessageBody };

                _Mailmessage.Body = builder.ToMessageBody();

                LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"Sending  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) ...");

                try
                {

                    _Mailmessage.Subject = "Reminder"; ;

                    _Mailmessage.To.Add(new MailboxAddress(mailingItem.to));

                    _Mailmessage.From.Add(new MailboxAddress("MAINTANANCE NOTIFICATION", mailingItem.from));


                    using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        //The last parameter here is to use SSL (Which you should!)
                        emailClient.Connect(mailingItem.smtpServer, mailingItem.port, mailingItem.enableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);

                        //Remove any OAuth functionality as we won't be using it. 
                        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                        emailClient.Authenticate(mailingItem.from, mailingItem.password);

                 

                        emailClient.Send(_Mailmessage);

                        emailClient.Disconnect(true);

                        _maintenance.UpdateMaintenance(Convert.ToInt32(mailingItem.custom));

                        mailingItem.ismailSent = true;

                    }

                                      

                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId})  was sent successfully!");

                }

                catch (ArgumentException ex)
                {
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");

                }
                catch (TimeoutException ex)
                {

                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");

                }
                catch (SmtpException ex)
                {
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");

                }

                catch (Exception ex)
                {
                    LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail()", $"maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");

                }

                finally
                {

                    try
                    {
                        Bal_Report.SaveReportsSentMailLogs(new EL_ReportsEmailLog
                        {
                            ifkCompanyId = mailingItem.ClientId,
                            ifkAlertId = Convert.ToInt32(mailingItem.custom),
                            dSentDate = DateTime.UtcNow,
                            bSuccessful = mailingItem.ismailSent,
                            EmailMessage = $"{mailingItem.MaintenanceName} ({mailingItem.MaintenanceAsset})",
                            EmailType = "maintenance mail",
                            SentTo = mailingItem.to,

                        });
                    }
                    catch (Exception ex)
                    {
                        LogError.RegisterErrorInLogFile("ObjectModel.cs", "SendMaintenanceMail() > SaveReportsSentMailLogs()", $"Maintenance  mail   {mailingItem.MaintenanceName}  for asset  {mailingItem.MaintenanceAsset }   AdditinalInfo: Reseller:> {mailingItem.ResellerName} ({mailingItem.ResellerId})  Client:> {mailingItem.ClientName} ({mailingItem.ClientId}) Error:>{ex.Message}  { ex.StackTrace} ");

                    }
                }

            }

        }

    }

    #endregion

}



