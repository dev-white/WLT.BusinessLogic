using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using WLT.BusinessLogic.Admin_Classes;
using WLT.EmailHelper;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;

namespace WLT.BusinessLogic
{
    class ScheduleReport
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

       public EL_ReportExport _EL_ReportExport { get; set; }


        public void CheckScheduledReports()
        {
            var _objBALMyAccount = new BAL_MyAccount();


            //function call to get scheduled reports data from databse
            DataSet _ds = _objBALMyAccount.GetScheduledReportData();
            try
            {
                foreach (DataRow _dr in _ds.Tables[0].Rows)
                {
                    string ScheduledReportId = Convert.ToString(_dr["ipkScheduledHistoryId"]);
                    string ReportId = Convert.ToString(_dr["ipkReportId"]);
                    string ReportTypeId = Convert.ToString(_dr["ifkReportTypeId"]);
                    string TimeZoneId = Convert.ToString(_dr["vTimeZoneID"]);
                    string ToEmail = Convert.ToString(_dr["vEmail"]);
                    string ReportName = Convert.ToString(_dr["vReportName"]);
                    string UserId = Convert.ToString(_dr["iUserId"]);
                    DateTime SentDateTime = Convert.ToDateTime(_dr["vSentDateTime"]);
                    SmtpServer = Convert.ToString(_dr["SMTPServer"]);
                    SmtpPort = Convert.ToInt32(_dr["SMTPPort"]);
                    AuthEmail = Convert.ToString(_dr["AuthEmail"]);
                    AuthPass = Convert.ToString(_dr["AuthPass"]);
                    EnableSSL = Convert.ToInt32(_dr["EnableSSL"]);
                    EmailEnabled = Convert.ToInt32(_dr["EmailEnabled"]);
                    ResellerID = Convert.ToInt32(_dr["ResellerID"]);
                    ClientID = Convert.ToInt32(_dr["ClientId"]);
                    IsTrial = Convert.ToBoolean(_dr["IsTrial"]);
                    var vCultureInfoCode = Convert.ToString(_dr["IsTrial"]);
                    SendMail(ScheduledReportId, ReportId, ReportTypeId, TimeZoneId, ToEmail, ReportName, UserId, SentDateTime, vCultureInfoCode);
                }
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
                ismailSent = false;




                EL_MyAccount _objparams = new EL_MyAccount();
                _objparams.wasEmailSentOk = ismailSent;
                _objparams.ScheduledReportId = 0;
                _objparams.op = 1;
                _objparams.ifk_ResellerId = ResellerID;
                _objparams.ifk_ClientId = Convert.ToInt32(ClientID);
                _objparams.errorMessage = ErrorMessage;
                _objparams.ShowErrorToReseller = ShowErrorToReseller;
                _objparams.Section = "Scheduled Reports";
                BAL_MyAccount _objBALMyAccounts = new BAL_MyAccount();
                DataSet _Ds = _objBALMyAccounts.CheckSheduleReportHistory(_objparams);

            }
        }

        //public void SendMail_(string ScheduledReportId, string ReportId, string ReportTypeId, string TimeZoneId, string ToEmail, string ReportName, string UserId, DateTime SentDateTime, string vCultureInfoCode)
        //{



        //    //update the database before even sending to signal attempt 
        //    BAL_MyAccount _objBALMyAccount = new BAL_MyAccount();
        //    EL_MyAccount _objELMyAccount = new EL_MyAccount();
        //    _objELMyAccount.ScheduledReportId = Convert.ToInt32(ScheduledReportId);
        //    _objELMyAccount.Status = "1";

        //    //function call to get scheduled reports data from databse
        //    DataSet _Ds = _objBALMyAccount.UpdateScheduledReportData(_objELMyAccount);





        //    // We have to write code here for logic to send mail 

        //    string msgBody = "";
        //    msgBody = "Please find attached your scheduled report <br><br> This report was scheduled on " + SentDateTime.ToString("dd MMM yyyy hh:mm tt");
        //    String to = ToEmail;


        //    var _config = AppConfiguration.Configuration();

        //    // Define Url which will 
        //    //  Uri url = new Uri("http://www.google.com/admanager/static/pdf/getting_started_guide.pdf");            
        //    Uri url = new Uri(_config.AppUrl + "User/ExportReports.aspx?ReportTypeId=" + ReportTypeId + "&ReportId=" + ReportId + " &TimeZoneID=" + TimeZoneId + "&UserId=" + UserId + "&CultureID=" + vCultureInfoCode);
        //    // Mail Subject
        //    String Subject = "Scheduler Report Mailer";
        //    // Name fo Pdf File 
        //    String File_Name = ReportName + ".pdf";
        //    // Type Of Application
        //    String App_Type = "application/pdf";

        //    if (EmailEnabled == 1)
        //    {
        //        try
        //        {

        //            ErrorMessage = Email.SendMail(SmtpServer, AuthEmail, AuthPass, Convert.ToString(to), SmtpPort,
        //            Subject, msgBody, Convert.ToBoolean(EnableSSL), true, url, File_Name, App_Type);
        //            ismailSent = ErrorMessage == string.Empty ? true : false;
        //            ShowErrorToReseller = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorMessage = ex.Message;

        //        }

        //    }
        //    else
        //    {
        //        if (IsTrial == true)
        //        {

        //            try
        //            {
        //                ErrorMessage = WLT.EmailHelper.Email.SendMail(_config.SmtpServer, _config.FromAddress,
        //                _config.Password, Convert.ToString(to), _config.SmtpPort,
        //                Subject, msgBody, _config.Ssl, true, url, File_Name, App_Type);
        //                ismailSent = ErrorMessage == string.Empty ? true : false;

        //                ShowErrorToReseller = false;
        //            }
        //            catch (Exception ex)
        //            {
        //                ErrorMessage = ex.Message;

        //            }
        //        }
        //        else
        //        {

        //            ErrorMessage = "This Account is no longer on Trial and Email is not Enabled on scheduled Reports";
        //            ismailSent = false;
        //            ShowErrorToReseller = true;

        //        }
        //    }

        //    EL_MyAccount _objparams = new EL_MyAccount();
        //    _objparams.wasEmailSentOk = ismailSent;
        //    _objparams.ScheduledReportId = Convert.ToInt64(ScheduledReportId);
        //    _objparams.op = 1;

        //    //Error logging section 
        //    _objparams.ifk_ResellerId = ResellerID;
        //    _objparams.ifk_ClientId = Convert.ToInt32(ClientID);
        //    _objparams.errorMessage = ErrorMessage;
        //    _objparams.ShowErrorToReseller = ShowErrorToReseller;
        //    _objparams.Section = "Scheduled Reports";
        //    BAL_MyAccount _objBALMyAccounts = new BAL_MyAccount();
        //    DataSet _ds = _objBALMyAccounts.CheckSheduleReportHistory(_objparams);



        //}

        public void SendMail(string ScheduledReportId, string ReportId, string ReportTypeId, string TimeZoneId, string ToEmail, string ReportName, string UserId, DateTime SentDateTime, string vCultureInfoCode)
        {

            var _EL_ReportExport = new EL_ReportExport
            {
                smtpServer = SmtpServer,
                from = AuthEmail,
                password = AuthPass,              
                port = SmtpPort,                          
                enableSsl = Convert.ToBoolean(EnableSSL),
                isBodyHtml = true,
                            
            };


            //update the database before even sending to signal attempt 
            BAL_MyAccount _objBALMyAccount = new BAL_MyAccount();

            EL_MyAccount _objELMyAccount = new EL_MyAccount();

            _objELMyAccount.ScheduledReportId = Convert.ToInt32(ScheduledReportId);

            _objELMyAccount.Status = "1";

            //function call to get scheduled reports data from databse
            DataSet _Ds = _objBALMyAccount.UpdateScheduledReportData(_objELMyAccount);


            // We have to write code here for logic to send mail 

            string msgBody = "";

            _EL_ReportExport.message = "Please find attached your scheduled report <br><br> This report was scheduled on " + SentDateTime.ToString("dd MMM yyyy hh:mm tt");

            _EL_ReportExport.to = ToEmail;

            var _config = AppConfiguration.Configuration();

            _EL_ReportExport.Url = new Uri(_config.AppUrl );

            _EL_ReportExport.subject = "Scheduler Report Mailer";

            _EL_ReportExport.FileName  = ReportName + ".pdf";

            _EL_ReportExport.MimeType = "application/pdf";

            _EL_ReportExport.Report = new El_Report
            {
                ReportTypeID = Convert.ToInt32(ReportTypeId),
                ReportId = Convert.ToInt32(ReportId),
                TimeZoneID = TimeZoneId,
                UserId = Convert.ToInt32(UserId),
                CultureID = vCultureInfoCode

            };

            if (EmailEnabled == 1)
            {
                try
                {       
                    ErrorMessage = Email.SendReportMail(_EL_ReportExport);

                    ismailSent = ErrorMessage == string.Empty ? true : false;

                    ShowErrorToReseller = true;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;

                }

            }
            else
            {
                if (IsTrial == true)
                {                    
                    _EL_ReportExport.smtpServer = _config.SmtpServer;
                    _EL_ReportExport.from = _config.FromAddress;
                    _EL_ReportExport.password = _config.Password;
                    _EL_ReportExport.port = _config.SmtpPort;
                    _EL_ReportExport.enableSsl = _config.Ssl;

                    try
                    {
                        //ErrorMessage = WLT.EmailHelper.Email.SendMail(_config.SmtpServer, _config.FromAddress,
                        //_config.Password, Convert.ToString(to), _config.SmtpPort,
                        //Subject, msgBody, _config.Ssl, true, url, File_Name, App_Type);

                        ErrorMessage = Email.SendReportMail(_EL_ReportExport);

                        ismailSent = ErrorMessage == string.Empty ? true : false;

                        ShowErrorToReseller = false;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;

                    }
                }
                else
                {

                    ErrorMessage = "This Account is no longer on Trial and Email is not Enabled on scheduled Reports";

                    ismailSent = false;

                    ShowErrorToReseller = true;

                }
            }

            EL_MyAccount _objparams = new EL_MyAccount();
            _objparams.wasEmailSentOk = ismailSent;
            _objparams.ScheduledReportId = Convert.ToInt64(ScheduledReportId);
            _objparams.op = 1;

            //Error logging section 
            _objparams.ifk_ResellerId = ResellerID;
            _objparams.ifk_ClientId = Convert.ToInt32(ClientID);
            _objparams.errorMessage = ErrorMessage;
            _objparams.ShowErrorToReseller = ShowErrorToReseller;
            _objparams.Section = "Scheduled Reports";
            BAL_MyAccount _objBALMyAccounts = new BAL_MyAccount();
            DataSet _ds = _objBALMyAccounts.CheckSheduleReportHistory(_objparams);



        }


    }
}
