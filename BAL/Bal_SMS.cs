//using Microsoft.IdentityModel.Protocols;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WLT.DataAccessLayer;
using com.esendex.sdk.messaging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using WLT.ErrorLog;
using WLT.EntityLayer;
using WLT.BusinessLogic;
using WLT.DataAccessLayer.DAL;

namespace SMSGatewayApp.BusinessLogic
{
    public class Bal_SMS
    {

        public Bal_SMS()
        {

        }

        public Bal_SMS(string configuration)
        {
            Configuration = configuration;
        }

              

        public string Configuration { get; set; }

        //Send Test Message

        //Twilio Test
        public string SendTwilioTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            //Send a test message
            try
            {
                TwilioClient.Init(el_SMS.TwilioSID, el_SMS.TwilioToken);

                var message = MessageResource.Create(
                                to: new PhoneNumber(el_SMS.SMSNumberTo),
                                from: new PhoneNumber(el_SMS.TwilioNumberFrom),
                                body: el_SMS.Message

                            );

                if (message.ErrorCode == null)
                {
                    result = "1";
                }
                if (message.ErrorCode != null)
                {

                    var error = message.ErrorMessage;

                    LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendTwilioTestMessage()", error);

                    return error.ToString();
                }

             

            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendTwilioTestMessage()", error);

                return error.ToString();
            }


            return result;
        }

        //Esendex Test
        public string SendEsendexTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            //Send a test message
            try
            {
                var messagingService = new MessagingService(el_SMS.Username, el_SMS.Password);

                var response = messagingService.SendMessage(new SmsMessage(el_SMS.SMSNumberTo, el_SMS.Message, el_SMS.AccountRef));

                result = "1";
            }
            catch (Exception ex)
            {
                var error = ex.Message.ToString();

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendEsendexTestMessage()", error);

                return error.ToString();
            }


            return result;
        }

        //AirTouch Test
        public string SendAirTouchTestMessage(EL_SMS el_SMS)
        {
            string result = "";
            var response = "";
            //Send a test message
            try
            {
                var phoneToSend = el_SMS.SMSNumberTo.Contains("+") == true ? el_SMS.SMSNumberTo.Split('+')[1] : el_SMS.SMSNumberTo;

                var url = "http://api.sms.bambika.co.ke:8555/?target=AirTouch&msisdn=" + phoneToSend + "&text=" + el_SMS.Message + "&login=" + el_SMS.Username + "&pass=" + el_SMS.Password;

                using (var client = new WebClient())
                {
                    response = client.DownloadString(url);
                }

                result = "1";
            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendAirTouchTestMessage()", error);

                return error.ToString();
            }


            return result;
        }

        //SMSAfrica Test
        public string SendSMSAfricaTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            //Send a test message
            try
            {
                var request = WebRequest.Create("https://smsafrica.tech/api/sms") as HttpWebRequest;

                request.KeepAlive = true;
                //request.Timeout = 52;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("apikey", el_SMS.ApiKey);

                string postData = "";

                if (el_SMS.SenderID != "")
                {
                    postData = "account_number=" + el_SMS.AccountRef + "&to=" + el_SMS.SMSNumberTo + "&from=" + el_SMS.SenderID
                   + "&message=" + el_SMS.Message + "&apikey=" + el_SMS.ApiKey;
                }
                else
                {
                    postData = "account_number=" + el_SMS.AccountRef + "&to=" + el_SMS.SMSNumberTo
                   + "&message=" + el_SMS.Message + "&apikey=" + el_SMS.ApiKey;
                }


                byte[] byteArray = Encoding.ASCII.GetBytes(postData);


                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                var details = JObject.Parse(responseContent);

                if (Convert.ToInt32(details["status"]["http_code"]) == 200)
                {
                    result = "1";
                }
                else
                {
                    result = Convert.ToString(details["status"]["description"]);
                }

            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendSMSAfricaTestMessage()", error);

                return error.ToString();
            }


            return result;
        }

        //SMSAfricaSA Test
        public string SendSMSAfricaSATestMessage(EL_SMS el_SMS)
        {
            string result = "";

            //Send a test message
            try
            {
                var request = WebRequest.Create("http://107.20.199.106/sms/1/text/single") as HttpWebRequest;

                var plainTextBytes = Encoding.UTF8.GetBytes(el_SMS.Username + ":" + el_SMS.Password);
                var Token = Convert.ToBase64String(plainTextBytes);

                request.KeepAlive = true;
                //request.Timeout = 52;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("authorization", "Basic " + Token);

                var obj = new
                {
                    from = el_SMS.SenderID,
                    to = el_SMS.SMSNumberTo,
                    text = el_SMS.Message
                };

                var param = JsonConvert.SerializeObject(obj);

                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                var details = JObject.Parse(responseContent);

                SmsAfricaSA res = (SmsAfricaSA)JsonConvert.DeserializeObject(details.ToString(), typeof(SmsAfricaSA));

                if (res.messages[0].status.id == 2 || res.messages[0].status.id == 3 || res.messages[0].status.id == 5 || res.messages[0].status.id == 7 || res.messages[0].status.id == 26)
                {
                    result = "1";
                }
                else
                {
                    result = res.messages[0].status.name + ". " + res.messages[0].status.description;
                }

            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendSMSAfricaSATestMessage()", error);

                return error.ToString();
            }


            return result;
        }

        //InfoBip Test
        public string SendInfoBipTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            //Send a test message
            try
            {
                var request = WebRequest.Create(el_SMS.BaseURL) as HttpWebRequest;

                var plainTextBytes = Encoding.UTF8.GetBytes(el_SMS.Username + ":" + el_SMS.Password);
                var Token = Convert.ToBase64String(plainTextBytes);

                request.KeepAlive = true;
                //request.Timeout = 52;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("authorization", "Basic " + Token);

                var obj = new
                {
                    from = el_SMS.SenderID,
                    to = el_SMS.SMSNumberTo,
                    text = el_SMS.Message
                };

                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                var details = JObject.Parse(responseContent);

                SmsAfricaSA res = (SmsAfricaSA)JsonConvert.DeserializeObject(details.ToString(), typeof(SmsAfricaSA));

                if (res.messages[0].status.id == 2 || res.messages[0].status.id == 3 || res.messages[0].status.id == 5 || res.messages[0].status.id == 7 || res.messages[0].status.id == 26)
                {
                    result = "1";
                }
                else
                {
                    result = res.messages[0].status.name + ". " + res.messages[0].status.description;
                }

            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendInfoBipTestMessage()", error);

                return error.ToString();
            }


            return result;
        }

        //Safaricom Test
        public string SendSafaricomTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            try
            {
                string senderName = el_SMS.SenderID;
                string spId = el_SMS.spId;
                string serviceId = el_SMS.serviceId;
                string oA = el_SMS.oA;
                string fA = el_SMS.fA;
                string spPassword = el_SMS.Password;
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                spPassword = MD5Hash(spId + el_SMS.Password + timeStamp);

                var client = new RestClient("http://41.90.0.130:8310/SendSmsService/services/SendSms");
                var request = new RestRequest(Method.POST);
                request.Timeout = 300000;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("KeepAlive", "true");
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Host", "41.90.0.130:8310");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("User-Agent", "WLT");
                request.AddParameter("text/xml; charset=\"utf-8\"", "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:v2=\"http://www.huawei.com.cn/schema/common/v2_1\" xmlns:loc=\"http://www.csapi.org/schema/parlayx/sms/send/v2_2/local\"><soapenv:Header><v2:RequestSOAPHeader><v2:spId>" + spId + "</v2:spId><v2:spPassword>" + spPassword + "</v2:spPassword><v2:serviceId>" + serviceId + "</v2:serviceId><v2:timeStamp>" + timeStamp + "</v2:timeStamp><v2:OA>tel:" + oA + "</v2:OA><v2:FA>tel:" + fA + "</v2:FA></v2:RequestSOAPHeader></soapenv:Header><soapenv:Body><loc:sendSms><loc:addresses>tel:" + el_SMS.SMSNumberTo + "</loc:addresses><loc:senderName>tel:" + senderName + "</loc:senderName><loc:message>This is for testing</loc:message><loc:receiptRequest><endpoint>https://dev.whitelabeltracking.com/api/SMSGateway/Datasync/Datasync</endpoint><interfaceName>SmsNotification</interfaceName><correlator>5597819</correlator></loc:receiptRequest></loc:sendSms></soapenv:Body></soapenv:Envelope>", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.Content == "")
                {
                    return "An error occured while sending the SMS.";
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);


                foreach (XmlNode node in doc.DocumentElement)
                {
                    string name = node.FirstChild.LocalName;
                    switch (name)
                    {
                        case "sendSmsResponse":
                            result = "1";
                            break;
                        case "Fault":

                            result = node.FirstChild.ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText;
                            break;
                    }
                }

            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendSafaricomTestMessage()", error);

                result = error.ToString();
            }


            return result;
        }

        //Clickatell Test
        public string SendClickatellTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            try
            {
                string baseURL = el_SMS.BaseURL;
                string AuthToken = el_SMS.AuthToken;


                var request = WebRequest.Create(baseURL) as HttpWebRequest;

                request.KeepAlive = true;
                //request.Timeout = 300;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("authorization", AuthToken);

                List<string> phone = new List<string>();
                phone.Add(el_SMS.SMSNumberTo);

                var obj = new
                {
                    content = "This is a test message",
                    to = phone
                };


                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                if (responseContent == "")
                {
                    return "An error occured while sending the SMS.";
                }


                var details = JObject.Parse(responseContent);

                Clickatell.Clickatell res = (Clickatell.Clickatell)JsonConvert.DeserializeObject(responseContent, typeof(Clickatell.Clickatell));

                if (res.error == null)
                {
                    result = "1";
                }
                else
                {
                    result = res.errorDescription.ToString();
                }

            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendClickatellTestMessage()", error);

                result = error.ToString();
            }


            return result;
        }

        //InterTelcom Test
        public string SendInterTelcomTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            try
            {
                string userName = el_SMS.Username;
                string AuthToken = el_SMS.AuthToken;
                string phone = el_SMS.SMSNumberTo.Contains("+") ? el_SMS.SMSNumberTo.Split('+')[1] : el_SMS.SMSNumberTo;


                var request = (HttpWebRequest)WebRequest.Create("https://mysim.intertelecom.gr/billing/api/sms_send");

                var postData = "u=" + userName + "&h=" + AuthToken + "&dst=" + phone + "&message=This is a test message";
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                if (responseString == "")
                {
                    return "An error occured while sending the SMS.";
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseString);


                foreach (XmlNode node in doc.DocumentElement)
                {
                    string name = node.Name;
                    switch (name)
                    {
                        case "response":
                            if (node.FirstChild.InnerText == "ok")
                            {
                                result = "1";
                            }
                            else
                            {
                                result = node.LastChild.InnerText;
                            }
                            break;
                        case "error":

                            result = node.InnerText;
                            break;
                    }
                }

            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendInterTelcomTestMessage()", error);

                result = error.ToString();
            }


            return result;
        }

        //BulkSMS Test
        public string SendBulkSMSTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            try
            {
                string AuthToken = el_SMS.AuthToken;
                string Password = el_SMS.Password;

                var plainTextBytes = Encoding.UTF8.GetBytes(AuthToken + ":" + Password);

                var apiToken = Convert.ToBase64String(plainTextBytes);

                var request = WebRequest.Create("https://api.bulksms.com/v1/messages") as HttpWebRequest;

                request.KeepAlive = true;
                //request.Timeout = 300;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("authorization", "Basic " + apiToken);

                var obj = new
                {

                    to = el_SMS.SMSNumberTo,
                    body = "This is a test message"
                };

                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }


                if (responseContent == "")
                {
                    return "An error occured while sending the SMS.";
                }


                var json = responseContent.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });
                var details = JObject.Parse(json);

                BulkSMS.BulkSMS res = (BulkSMS.BulkSMS)JsonConvert.DeserializeObject(details.ToString(), typeof(BulkSMS.BulkSMS));

                if (res.type == "SENT")
                {

                    return "1";
                }
                else
                {
                    return res.title;
                }

            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }


                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendBulkSMSTestMessage()", error);

                var json = error.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });

                var details = JObject.Parse(json);

                BulkSMS.BulkSMS res = (BulkSMS.BulkSMS)JsonConvert.DeserializeObject(details.ToString(), typeof(BulkSMS.BulkSMS));

                result = res.title;
            }


            return result;
        }

        //Habary Bulk Test
        public string SendHabaryBulkSMSTestMessage(EL_SMS el_SMS)
        {
            string result = "";

            try
            {
                string BaseURL = el_SMS.BaseURL;
                string AuthToken = el_SMS.AuthToken;
                string SenderID = el_SMS.SenderID;

                var request = WebRequest.Create(BaseURL) as HttpWebRequest;

                request.KeepAlive = true;
                request.Timeout = 3000;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("X-API-KEY", AuthToken);

                var obj = new
                {

                    identifier = "sms_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    receiver = el_SMS.SMSNumberTo,
                    sender = SenderID,
                    priority = 1,
                    message = "This is a test message"
                };

                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }


                if (responseContent == "")
                {
                    return "An error occured while sending the SMS.";
                }


                var details = JObject.Parse(responseContent);

                HabaryBulkSMS.SMS res = (HabaryBulkSMS.SMS)JsonConvert.DeserializeObject(details.ToString(), typeof(HabaryBulkSMS.SMS));

                if (res.response == null)
                {
                    if (res.StatusCode == "A001")
                    {
                        return "1";
                    }

                    return res.detail;
                }
                else
                {
                    return res.response;
                }

            }
            catch (WebException ex)
            {
                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendHabaryBulkSMSTestMessage()", error);

                var details = JObject.Parse(error);

                HabaryBulkSMS.SMS res = (HabaryBulkSMS.SMS)JsonConvert.DeserializeObject(details.ToString(), typeof(HabaryBulkSMS.SMS));

                result = res.response;
            }


            return result;
        }

        /***********************************************************************************************************************************/

        //Send SMS 

        //Twilio
        public string SendSmsWithTwilio(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {

            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> twilioSettings = new List<string>();

                string accountSid = "";
                string authToken = "";
                string numFrom = "";

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);



                if (!quotaExceeded)
                {
                    return "-1";
                }

                twilioSettings = dalOtaSetup.GetTwilioSettingsFromReseller(resellerId);
                accountSid = twilioSettings[0];
                numFrom = twilioSettings[1];
                authToken = twilioSettings[2];

                TwilioClient.Init(accountSid, authToken);

                var messageToSend = MessageResource.Create(
                    to: new PhoneNumber(phone),
                    from: new PhoneNumber(numFrom),
                    body: message
                );

                if (messageToSend.ErrorCode != null)
                {
                    var error = messageToSend.ErrorMessage;

                    LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendTwilioMessage()", error);

                    //Log Failed Message
                    DAL_SMS dalSms = new DAL_SMS();
                  

                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                    return error;
                }
                else if (messageToSend.ErrorCode == null)
                {
                    //log SMS sent for quota (Delivered)
                    DAL_SMS dalSms = new DAL_SMS();
                   

                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                    //Update client quota
                    dalOtaSetup.UpdateClientSMSQuota(clientId);
                }

                return "1";
            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendSmsWithTwilio()", ex.StackTrace.ToString() + " . Reseller ID: " + resellerId);

                return ex.Message.ToString();

            }
        }

        //Twilio Resend
        public string ReSendMessageWithTwilio(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               ///

                List<string> twilioSettings = new List<string>();

                //Check SMS Quota
             //   
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                string accountSid = "";
                string authToken = "";
                string numFrom = "";

                twilioSettings = dalOtaSetup.GetTwilioSettingsFromReseller(resellerId);
                accountSid = twilioSettings[0];
                numFrom = twilioSettings[1];
                authToken = twilioSettings[2];

                TwilioClient.Init(accountSid, authToken);

                var messageToSend = MessageResource.Create(
                    to: new PhoneNumber(phone),
                    from: new PhoneNumber(numFrom),
                    body: message
                );

                if (messageToSend.ErrorCode != null)
                {
                    var error = messageToSend.ErrorMessage;

                    LogError.RegisterErrorInLogFile("Bal_SMS.cs", "ReSendMessageWithTwilio()", error + " . Reseller ID: " + resellerId);

                    //Log Failed messages
                    DAL_SMS dalSms = new DAL_SMS();
                  

                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                    return error;
                }
                else if (messageToSend.ErrorCode == null)
                {
                    //log SMS sent for quota
                    DAL_SMS dalSms = new DAL_SMS();
                  

                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                    //Update client quota
                    dalOtaSetup.UpdateClientSMSQuota(clientId);
                }


                return "1";
            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "ReSendMessageWithTwilio()", ex.StackTrace.ToString() + " . Reseller ID: " + resellerId);

                return ex.Message.ToString();
            }

        }

        //Android Mobile App
        public string SaveSmsToSend(int resellerId, int clientId, string message, string phone)
        {
            string strReturn = "";
            DAL_SMS dalSms = new DAL_SMS();
          

            strReturn = dalSms.SaveSmsToSend(resellerId, clientId, message, phone);

            return strReturn;
        }

        //Essendex
        public string SendEssendexSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> essendexSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                string username = "";
                string password = "";
                string accountRef = "";

                essendexSettings = dalOtaSetup.GetEssendexSettingsFromReseller(resellerId);
                username = essendexSettings[0];
                password = essendexSettings[1];
                accountRef = essendexSettings[2];

                var messagingService = new MessagingService(username, password);

                var response = messagingService.SendMessage(new SmsMessage(phone, message, accountRef));

                //log SMS sent for quota (Delivered)
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                //Update client quota
                dalOtaSetup.UpdateClientSMSQuota(clientId);

                return "1";
            }
            catch (Exception ex)
            {

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendEssendexSms()", ex.StackTrace.ToString() + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                return ex.Message.ToString();

            }
        }

        //AirTouch
        public string SendAirTouchSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            var response = "";

            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> airTouchSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                string username = "";
                string password = "";

                airTouchSettings = dalOtaSetup.GetAirTouchSettingsFromReseller(resellerId);
                username = airTouchSettings[0];
                password = airTouchSettings[1];

                var phoneToSend = phone.Contains("+") == true ? phone.Split('+')[1] : phone;

                var url = "http://api.sms.bambika.co.ke:8555/?target=AirTouch&msisdn=" + phoneToSend + "&text=" + message + "&login=" + username + "&pass=" + password;

                using (var client = new WebClient())
                {
                    response = client.DownloadString(url);
                }

                //log SMS sent for quota (Delivered)
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                //Update client quota
                dalOtaSetup.UpdateClientSMSQuota(clientId);

                return "1";
            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendAirTouchSms()", ex.StackTrace.ToString() + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                return error.ToString();

            }
        }

        //Sms Africa
        public string SendSMSAfricaSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> smsAfricaSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                string accountRef = "";
                string apikey = "";
                string senderID = "";

                smsAfricaSettings = dalOtaSetup.GetSMSAfricaSettingsFromReseller(resellerId);
                accountRef = smsAfricaSettings[0];
                apikey = smsAfricaSettings[1];
                senderID = smsAfricaSettings[2];

                var request = WebRequest.Create("https://smsafrica.tech/api/sms") as HttpWebRequest;

                request.KeepAlive = true;
                //request.Timeout = 52;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("apikey", apikey);

                string postData = "";

                if (senderID != "")
                {
                    postData = "account_number=" + accountRef + "&to=" + phone + "&from=" + senderID
                    + "&message=" + message + "&apikey=" + apikey;
                }
                else
                {
                    postData = "account_number=" + accountRef + "&to=" + phone
                    + "&message=" + message + "&apikey=" + apikey;
                }


                byte[] byteArray = Encoding.ASCII.GetBytes(postData);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                var details = JObject.Parse(responseContent);

                if (Convert.ToInt32(details["status"]["http_code"]) == 200)
                {
                    //log SMS sent for quota (Delivered)
                    DAL_SMS dalSms = new DAL_SMS();
                  

                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                    //Update client quota
                    dalOtaSetup.UpdateClientSMSQuota(clientId);

                    return "1";
                }
                else
                {
                    return Convert.ToString(details["status"]["description"]);
                }
            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendSMSAfricaSms()", error + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                return error.ToString();

            }
        }

        //Sms Africa SA
        public string SendSMSAfricaSASms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> smsAfricaSASettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                string username = "";
                string password = "";
                string senderID = "";

                smsAfricaSASettings = dalOtaSetup.GetSMSAfricaSASettingsFromReseller(resellerId);
                username = smsAfricaSASettings[0];
                password = smsAfricaSASettings[1];
                senderID = smsAfricaSASettings[2];

                var plainTextBytes = Encoding.UTF8.GetBytes(username + ":" + password);
                var Token = Convert.ToBase64String(plainTextBytes);

                var request = WebRequest.Create("http://107.20.199.106/sms/1/text/single") as HttpWebRequest;

                request.KeepAlive = true;
                //request.Timeout = 52;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("authorization", "Basic " + Token);

                var obj = new
                {
                    from = senderID,
                    to = phone,
                    text = message
                };


                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                var details = JObject.Parse(responseContent);

                SmsAfricaSA res = (SmsAfricaSA)JsonConvert.DeserializeObject(details.ToString(), typeof(SmsAfricaSA));


                if (res.messages[0].status.id == 2 || res.messages[0].status.id == 3 || res.messages[0].status.id == 5 || res.messages[0].status.id == 7 || res.messages[0].status.id == 26)
                {
                    //log SMS sent for quota (Delivered)
                    DAL_SMS dalSms = new DAL_SMS();
                  

                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                    //Update client quota
                    dalOtaSetup.UpdateClientSMSQuota(clientId);

                    return "1";
                }
                else
                {
                    return res.messages[0].status.name + ". " + res.messages[0].status.description;
                }

            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendSMSAfricaSASms()", error + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                return error.ToString();

            }
        }

        //Info Bip
        public string SendInfoBipSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> smsInfoBipSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                string baseURL = "";
                string username = "";
                string password = "";
                string senderID = "";

                smsInfoBipSettings = dalOtaSetup.GetInfoBipSettingsFromReseller(resellerId);

                baseURL = smsInfoBipSettings[0];
                username = smsInfoBipSettings[1];
                password = smsInfoBipSettings[2];
                senderID = smsInfoBipSettings[3];

                var plainTextBytes = Encoding.UTF8.GetBytes(username + ":" + password);
                var Token = Convert.ToBase64String(plainTextBytes);

                var request = WebRequest.Create(baseURL) as HttpWebRequest;

                request.KeepAlive = true;
                //request.Timeout = 52;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("authorization", "Basic " + Token);

                var obj = new
                {
                    from = senderID,
                    to = phone,
                    text = message
                };


                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                var details = JObject.Parse(responseContent);

                SmsAfricaSA res = (SmsAfricaSA)JsonConvert.DeserializeObject(details.ToString(), typeof(SmsAfricaSA));


                if (res.messages[0].status.id == 2 || res.messages[0].status.id == 3 || res.messages[0].status.id == 5 || res.messages[0].status.id == 7 || res.messages[0].status.id == 26)
                {
                    //log SMS sent for quota (Delivered)
                    DAL_SMS dalSms = new DAL_SMS();
                  

                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                    //Update client quota
                    dalOtaSetup.UpdateClientSMSQuota(clientId);

                    return "1";
                }
                else
                {
                    return res.messages[0].status.name + ". " + res.messages[0].status.description;
                }

            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendInfoBipSms()", error + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                return error.ToString();

            }
        }
        //Safaricom
        public string SendSafaricomSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            var result = "";

            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> safaricomSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                string senderName = "";
                string spId = "";
                string serviceId = "";
                string oA = "";
                string fA = "";
                string spPassword = "";
                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                safaricomSettings = dalOtaSetup.GetSafaricomSettingsFromReseller(resellerId);

                senderName = safaricomSettings[0];
                spId = safaricomSettings[1];
                serviceId = safaricomSettings[2];
                oA = safaricomSettings[3];
                fA = safaricomSettings[4];
                spPassword = safaricomSettings[5];

                spPassword = MD5Hash(spId + spPassword + timeStamp);

                var client = new RestClient("http://41.90.0.130:8310/SendSmsService/services/SendSms");
                var request = new RestRequest(Method.POST);
                request.Timeout = 300000;
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("KeepAlive", "true");
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Host", "41.90.0.130:8310");
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Accept", "*/*");
                request.AddHeader("User-Agent", "WLT");
                request.AddParameter("text/xml; charset=\"utf-8\"", "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:v2=\"http://www.huawei.com.cn/schema/common/v2_1\" xmlns:loc=\"http://www.csapi.org/schema/parlayx/sms/send/v2_2/local\"><soapenv:Header><v2:RequestSOAPHeader><v2:spId>" + spId + "</v2:spId><v2:spPassword>" + spPassword + "</v2:spPassword><v2:serviceId>" + serviceId + "</v2:serviceId><v2:timeStamp>" + timeStamp + "</v2:timeStamp><v2:OA>tel:" + oA + "</v2:OA><v2:FA>tel:" + fA + "</v2:FA></v2:RequestSOAPHeader></soapenv:Header><soapenv:Body><loc:sendSms><loc:addresses>tel:" + phone + "</loc:addresses><loc:senderName>tel:" + senderName + "</loc:senderName><loc:message>" + message + "</loc:message><loc:receiptRequest><endpoint>https://dev.whitelabeltracking.com/api/SMSGateway/Datasync/Datasync</endpoint><interfaceName>SmsNotification</interfaceName><correlator>5597819</correlator></loc:receiptRequest></loc:sendSms></soapenv:Body></soapenv:Envelope>", ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.Content == "")
                {
                    return "An error occured while sending the SMS.";
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response.Content);

                foreach (XmlNode node in doc.DocumentElement)
                {
                    string name = node.FirstChild.LocalName;

                    DAL_SMS dalSms = new DAL_SMS();
                  

                    switch (name)
                    {
                        case "sendSmsResponse":
                            result = "1";

                            //log SMS sent for quota (Delivered)                           
                            dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                            //Update client quota
                            dalOtaSetup.UpdateClientSMSQuota(clientId);

                            break;
                        case "Fault":

                            result = node.FirstChild.ChildNodes[2].ChildNodes[0].ChildNodes[1].InnerText;

                            //Log Failed Message                          
                            dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                            break;

                    }
                }

            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendSafaricomSms()", error + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                result = error.ToString();

            }

            return result;
        }

        //Clickatell
        public string SendClickatellSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            var result = "";

            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> clickatellSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                clickatellSettings = dalOtaSetup.GetClickatellSettingsFromReseller(resellerId);


                string baseURL = clickatellSettings[0];
                string AuthToken = clickatellSettings[1];

                var request = WebRequest.Create(baseURL) as HttpWebRequest;

                request.KeepAlive = true;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("authorization", AuthToken);

                List<string> address = new List<string>();
                address.Add(phone);

                var obj = new
                {
                    content = message,
                    to = address
                };


                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                if (responseContent == "")
                {
                    return "An error occured while sending the SMS.";
                }


                var details = JObject.Parse(responseContent);

                Clickatell.Clickatell res = (Clickatell.Clickatell)JsonConvert.DeserializeObject(responseContent, typeof(Clickatell.Clickatell));

                if (res.error == null)
                {
                    result = "1";
                }
                else
                {
                    result = res.errorDescription.ToString();
                }



            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendClickatellSms()", error + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                result = error.ToString();

            }

            return result;
        }

        //InterTelcom       
        public string SendInterTelcomSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            var result = "";

            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> interTelcomSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                interTelcomSettings = dalOtaSetup.GetInterTelcomSettingsFromReseller(resellerId);


                string userName = interTelcomSettings[0];
                string AuthToken = interTelcomSettings[1];
                phone = phone.Contains("+") ? phone.Split('+')[1] : phone;

                var request = (HttpWebRequest)WebRequest.Create("https://mysim.intertelecom.gr/billing/api/sms_send");

                var postData = "u=" + userName + "&h=" + AuthToken + "&dst=" + phone + "&message=" + message;
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();


                if (responseString == "")
                {
                    return "An error occured while sending the SMS.";
                }


                XmlDocument doc = new XmlDocument();
                doc.LoadXml(responseString);

                foreach (XmlNode node in doc.DocumentElement)
                {
                    string name = node.Name;

                    DAL_SMS dalSms = new DAL_SMS();
                  

                    switch (name)
                    {
                        case "response":

                            if (node.FirstChild.InnerText == "ok")
                            {
                                result = "1";

                                //log SMS sent for quota (Delivered)                           
                                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                                //Update client quota
                                dalOtaSetup.UpdateClientSMSQuota(clientId);
                            }
                            else
                            {
                                result = node.LastChild.InnerText;

                                //Log Failed Message                          
                                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);
                            }


                            break;
                        case "error":

                            result = node.InnerText;

                            //Log Failed Message                          
                            dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                            break;

                    }
                }



            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendInterTelcomSms()", error + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                result = error.ToString();

            }

            return result;
        }

        //Bulk SMS
        public string SendBulkSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            var result = "";

            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               

                List<string> BulkSMSSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                BulkSMSSettings = dalOtaSetup.GetBulkSMSSettingsFromReseller(resellerId);


                string AuthToken = BulkSMSSettings[0];
                string Password = BulkSMSSettings[1];

                var plainTextBytes = Encoding.UTF8.GetBytes(AuthToken + ":" + Password);

                var apiToken = Convert.ToBase64String(plainTextBytes);

                var request = WebRequest.Create("https://api.bulksms.com/v1/messages") as HttpWebRequest;

                request.KeepAlive = true;
                //request.Timeout = 300;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("authorization", "Basic " + apiToken);

                var obj = new
                {
                    to = phone,
                    body = message
                };

                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }


                if (responseContent == "")
                {
                    return "An error occured while sending the SMS.";
                }


                var json = responseContent.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });

                var details = JObject.Parse(json);

                BulkSMS.BulkSMS res = (BulkSMS.BulkSMS)JsonConvert.DeserializeObject(details.ToString(), typeof(BulkSMS.BulkSMS));

                DAL_SMS dalSms = new DAL_SMS();
              

                if (res.type == "SENT")
                {
                    result = "1";

                    //log SMS sent for quota (Delivered)                           
                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                    //Update client quota
                    dalOtaSetup.UpdateClientSMSQuota(clientId);
                }
                else
                {
                    //Log Failed Message                          
                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                    result = res.title;
                }
            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }

                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendBulkSms()", error + " . Reseller ID: " + resellerId);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                var json = error.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });

                var details = JObject.Parse(json);

                BulkSMS.BulkSMS res = (BulkSMS.BulkSMS)JsonConvert.DeserializeObject(details.ToString(), typeof(BulkSMS.BulkSMS));

                result = res.title;

            }

            return result;
        }

        //Habary Bulk SMS
        public string SendHabaryBulkSms(int resellerId, int clientId, string message, string phone, string alertType, int alertId)
        {
            var result = "";

            try
            {
                DAL_OtaSetup dalOtaSetup = new DAL_OtaSetup();
               


                List<string> HabaryBulkSMSSettings = new List<string>();

                //Check SMS Quota
                
                bool quotaExceeded = ResellerHelper.HasClientExceededSmsQuota(clientId);


                if (!quotaExceeded)
                {
                    return "-1";
                }

                HabaryBulkSMSSettings = dalOtaSetup.GetHabaryBulkSMSSettingsFromReseller(resellerId);

                string BaseURL = HabaryBulkSMSSettings[0];
                string AuthToken = HabaryBulkSMSSettings[1];
                string SenderID = HabaryBulkSMSSettings[2];


                var request = WebRequest.Create(BaseURL) as HttpWebRequest;

                request.KeepAlive = true;
                request.Timeout = 3000;
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.Headers.Add("X-API-KEY", AuthToken);

                var obj = new
                {
                    identifier = "sms_" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    receiver = phone,
                    sender = SenderID,
                    priority = 1,
                    message = message
                };


                var param = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.UTF8.GetBytes(param);

                string responseContent = null;

                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }


                if (responseContent == "")
                {
                    return "An error occured while sending the SMS.";
                }

                DAL_SMS dalSms = new DAL_SMS();
              

                var details = JObject.Parse(responseContent);

                HabaryBulkSMS.SMS res = (HabaryBulkSMS.SMS)JsonConvert.DeserializeObject(details.ToString(), typeof(HabaryBulkSMS.SMS));

                if (res.response == null)
                {
                    if (res.StatusCode == "A001")
                    {
                        result = "1";

                        //log SMS sent for quota (Delivered)                           
                        dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 1);

                        //Update client quota
                        dalOtaSetup.UpdateClientSMSQuota(clientId);
                    }
                    else
                    {
                        //Log Failed Message                          
                        dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                        result = res.detail;
                    }

                }
                else
                {
                    //Log Failed Message                          
                    dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                    result = res.response;
                }

            }
            catch (WebException ex)
            {

                var error = "";

                using (StreamReader r = new StreamReader(

                    ex.Response.GetResponseStream()))
                {
                    error = r.ReadToEnd();
                }


                LogError.RegisterErrorInLogFile("Bal_SMS.cs", "SendHabaryBulkSms()", error);

                //Log Failed Message
                DAL_SMS dalSms = new DAL_SMS();
              

                dalSms.InsertSmsSentLog(clientId, message, phone, alertType, alertId, 0);

                var details = JObject.Parse(error);

                HabaryBulkSMS.SMS res = (HabaryBulkSMS.SMS)JsonConvert.DeserializeObject(details.ToString(), typeof(HabaryBulkSMS.SMS));

                result = res.response;

            }

            return result;
        }

        public static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://41.90.0.130:8310/SendSmsService/services/SendSms");
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        public string GetSmsProvider(int resellerId)
        {
            DataSet ds = new DataSet();

            DAL_SMS dalSms = new DAL_SMS();
          

            ds = dalSms.GetSmsProvider(resellerId);

            var results = "";

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                results = Convert.ToString(ds.Tables[0].Rows[0]["SmsProviderName"]);
            }
            return results;
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

    }


    public class Status
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class Message
    {
        public string to { get; set; }
        public Status status { get; set; }
        public int smsCount { get; set; }
        public string messageId { get; set; }
    }

    public class SmsAfricaSA
    {
        public List<Message> messages { get; set; }
    }

    namespace Clickatell
    {
        public class Message
        {
            public string apiMessageId { get; set; }
            public bool accepted { get; set; }
            public string to { get; set; }
            public object errorCode { get; set; }
            public object error { get; set; }
            public object errorDescription { get; set; }
        }

        public class Clickatell
        {
            public List<Message> messages { get; set; }
            public object errorCode { get; set; }
            public object error { get; set; }
            public object errorDescription { get; set; }
        }


    }

    public class BulkSMS2
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string detail { get; set; }
    }

    namespace BulkSMS
    {
        public class Submission
        {
            public string id { get; set; }
            public DateTime date { get; set; }
        }

        public class Status
        {
            public string id { get; set; }
            public string type { get; set; }
            public string subtype { get; set; }
        }

        public class BulkSMS
        {
            public string id { get; set; }
            public string type { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public string body { get; set; }
            public string encoding { get; set; }
            public int protocolId { get; set; }
            public int messageClass { get; set; }
            public Submission submission { get; set; }
            public Object status { get; set; }
            public string title { get; set; }
            public string detail { get; set; }
            public string relatedSentMessageId { get; set; }
            public string userSuppliedId { get; set; }
            public string numberOfParts { get; set; }
            public string creditCost { get; set; }
        }
    }

    namespace HabaryBulkSMS
    {
        public class SMS
        {
            public string reference { get; set; }
            public string identifier { get; set; }
            public string detail { get; set; }
            public string StatusCode { get; set; }
            public string response { get; set; }

        }
    }
}
