using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using WLT.DataAccessLayer;
using WLT.DataAccessLayer.DAL;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Threading;
using System.Timers;
using WLT.EntityLayer;
using WLT.BusinessLogic.BAL;
using SMSGatewayApp.BusinessLogic;
using WLT.ErrorLog;
//using System.Messaging;


namespace WLT.BusinessLogic
{
    public class OtaSetup
    {
        //public static object GetClientTwillioSettings { get; set; }
        //public static object GetWltCompanyTwilioSettings { get; set; }

        public int _ifk_CommandMasterId = 0;
        public int _SequenceID = 0;
        public int _ClientId = 0;
        public string _Imei = "";
        public string _CommandText = "";
        public string _PhoneNumber = "";
        public static List<string> GetUnFormattedSmsCommand(int DeviceId)
        {
            DAL_OtaSetup DalOtaSetup = new DAL_OtaSetup();
            List<string> SmsCommands = new List<string>();
            try
            {


                //##############
                //todo may need  process more than 1 sms
                SmsCommands = DalOtaSetup.GetSmsCommandFrom_tblDevice_OTA_CommandMaster(DeviceId);
                return SmsCommands;
            }
            catch (Exception ex)
            {
                //write ex to log
               LogError.RegisterErrorInLogFile("OtaSetup.cs", "GetUnFormattedSmsCommand()", ex.Message  + ex.StackTrace);
                return SmsCommands;
            }

        }

        public static string FormatSmsCommand(string APN, string USERNAME, string PASSWORD, string CNAME, string IMEINUMBER, string UnformattedSms)
        {

            try
            {
                //Replace template APN settings with specific ones from their network    

                UnformattedSms = UnformattedSms.Replace("[APN]", APN);
                UnformattedSms = UnformattedSms.Replace("[USERNAME]", USERNAME);
                UnformattedSms = UnformattedSms.Replace("[PASSWORD]", PASSWORD);
                UnformattedSms = UnformattedSms.Replace("[CNAME]", CNAME);
                UnformattedSms = UnformattedSms.Replace("[IMEI]", IMEINUMBER);
            }
            catch (Exception ex)
            {
                //write ex to log
               LogError.RegisterErrorInLogFile("OtaSetup.cs", "GetFormattedSmsCommand()", ex.Message  + ex.StackTrace);
                return "";
            }

            return UnformattedSms;
        }

        public string SendSmsCommandsToDevice(EL_OTACommands el_OTACommands, int DeviceId, int ClientId, string PhoneNumber, string ImeiNumber, string APN, string USERNAME, string PASSWORD, string CNAME, string AdminTwilioAccountSID, string AdminTwilioAuthToken, string AdminTwilioSMSNumberFrom, bool isResend)
        {
            try
            {
                
                DAL_OtaSetup DalOtaSetup = new DAL_OtaSetup();               
                string formattedSms = "";
                string messageSent = "";               
                string port = "";
                int resellerId = ResellerHelper.GetResellerIdFromClientId(ClientId);                

                if (el_OTACommands.CommandText == "")
                {
                    port = DalOtaSetup.GetDevicePortNumber(DeviceId);

                    return "port " + port;
                }
               
                _ifk_CommandMasterId = el_OTACommands.ID;
                _SequenceID = el_OTACommands.SequenceID;
                _CommandText = el_OTACommands.CommandText;


                formattedSms = FormatSmsCommand(APN, USERNAME, PASSWORD, CNAME, ImeiNumber, el_OTACommands.CommandText);

                if (!isResend)
                {
                    messageSent = SendSmsMessage(_ifk_CommandMasterId, _SequenceID, ClientId, ImeiNumber, _CommandText, resellerId, formattedSms, AdminTwilioAccountSID, AdminTwilioAuthToken, AdminTwilioSMSNumberFrom, PhoneNumber);

                }
                else
                {
                    messageSent = ReSendMessageWithTwilio(_ifk_CommandMasterId, _SequenceID, ClientId, ImeiNumber, _CommandText, resellerId, formattedSms, AdminTwilioAccountSID, AdminTwilioAuthToken, AdminTwilioSMSNumberFrom, PhoneNumber);
                }

                return messageSent;

            }
            catch (Exception ex)
            {
                //write ex to log
               LogError.RegisterErrorInLogFile("OtaSetup.cs", "SendSmsCommandsToDevice()", ex.Message  + ex.StackTrace);
                //insert failed row into wlt_tblDevice_OTA_CommandLog ([IsMessageSent] = 0) 
                return ex.Message;
            }

        }

        public static string SendSmsMessage(int ifk_CommandMasterId, int SequenceID, int ClientId, string Imei, string CommandText, int ResellerId, string txtMessage, string AdminTwilioAccountSID, string AdminTwilioAuthToken, string AdminTwilioSMSNumberFrom, string smsToNumber)
        {

            int resellerId = ResellerHelper.GetResellerIdFromClientId(ClientId);

            string results = "";

            bool isSmsEnabled = ResellerHelper.IsSMSEnabled(ResellerId);

            if (!isSmsEnabled)
            {
                resellerId = 3; //Use the WLT Twillio settings from DB  
            }

            //Get SMS Provider
            Bal_SMS balSms = new Bal_SMS();
            var provider = balSms.GetSmsProvider(resellerId);

            if (provider == "Twillio")
            {
                results = balSms.SendSmsWithTwilio(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            else if (provider == "Android Mobile App")
            {
                //Save the data to the database
                results = balSms.SaveSmsToSend(resellerId, ClientId, txtMessage, smsToNumber);

            }
            if (provider == "Esendex")
            {
                results = balSms.SendEssendexSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "AirTouch")
            {
                results = balSms.SendAirTouchSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "SMS Africa")
            {
                results = balSms.SendSMSAfricaSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "SMS Africa SA")
            {
                results = balSms.SendSMSAfricaSASms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "Info Bip")
            {
                results = balSms.SendInfoBipSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "Safaricom")
            {
                results = balSms.SendSafaricomSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "Clickatell")
            {
                results = balSms.SendClickatellSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "InterTelcom")
            {
                results = balSms.SendInterTelcomSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "Bulk SMS")
            {
                results = balSms.SendBulkSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }
            if (provider == "Habary Bulk SMS")
            {
                results = balSms.SendHabaryBulkSms(resellerId, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);
            }

            if (results == "2")//Message queued
            {
                results = "1";
            }

            return results;
        }

        public static string ReSendMessageWithTwilio(int ifk_CommandMasterId, int SequenceID, int ClientId, string Imei, string CommandText, int ResellerId, string txtMessage, string AdminTwilioAccountSID, string AdminTwilioAuthToken, string AdminTwilioSMSNumberFrom, string smsToNumber)
        {
            Bal_SMS balSms = new Bal_SMS();
            var results = "";

            results = balSms.ReSendMessageWithTwilio(3, ClientId, txtMessage, smsToNumber, "OTA Activation", 0);

            
            return results;
        }


        public string SendCustomCommand(int ClientId, string txtMessage, string SIMNumber, string AdminTwilioAccountSID, string AdminTwilioAuthToken, string AdminTwilioSMSNumberFrom)
        {
            int resellerId = ResellerHelper.GetResellerIdFromClientId(ClientId);

            string results = "";

            //Get SMS Provider
            Bal_SMS balSms = new Bal_SMS();
            var provider = balSms.GetSmsProvider(resellerId);

            if (provider == "Twillio")
            {
                results = balSms.SendSmsWithTwilio(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if (provider == "Android Mobile App")
            {
                //Save the data to the database
                results = balSms.SaveSmsToSend(resellerId, ClientId, txtMessage, SIMNumber);

            }
            else if(provider == "Esendex")
            {
                results = balSms.SendEssendexSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if(provider == "AirTouch")
            {
                results = balSms.SendAirTouchSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if(provider == "SMS Africa")
            {
                results = balSms.SendSMSAfricaSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if (provider == "SMS Africa SA")
            {
                results = balSms.SendSMSAfricaSASms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if (provider == "Info Bip")
            {
                results = balSms.SendInfoBipSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if (provider == "Safaricom")
            {
                results = balSms.SendSafaricomSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if (provider == "Clickatell")
            {
                results = balSms.SendClickatellSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if (provider == "InterTelcom")
            {
                results = balSms.SendInterTelcomSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if (provider == "Bulk SMS")
            {
                results = balSms.SendBulkSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else if (provider == "Habary Bulk SMS")
            {
                results = balSms.SendHabaryBulkSms(resellerId, ClientId, txtMessage, SIMNumber, "Device Command", 0);
            }
            else
            {
                //No provider set
                results = "No SMS Provider set!";
            }
            return results;
        }
    }
}
