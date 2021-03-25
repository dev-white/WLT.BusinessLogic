using Hangfire;
using Hangfire.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using WLT.DataAccessLayer.DAL_Installation;
using WLT.EntityLayer;
using WLT.EntityLayer.Utilities;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.Bal_Notification
{
    public class Bal_Microservice
    {


        // hangfire installation  job to  activate webhook, keep trying for ten times before quieting retries;     
        [AutomaticRetry(Attempts = 10)]
        public void StartWebHookInstallationJob(EntityLayer.InstallationStatus _statusDetails)
        {
            BackgroundJob.Schedule(() => SendWebhookInstallationStatus(_statusDetails), TimeSpan.FromSeconds(1));
        }


        // hangfire installation  job to  activate webhook, keep trying for ten times before quieting retries;     
        [AutomaticRetry(Attempts = 10)]
        public void StartReportWebHookInstallationStatusCheckup(EntityLayer.InstallationStatus _statusDetails)
        {
            BackgroundJob.Schedule(() => SendWebhookInstallationStatus(_statusDetails), TimeSpan.FromSeconds(1));
        }





        // hangfire status check  job to  activate webhook, keep trying for ten times before quieting retries;
        [AutomaticRetry(Attempts = 10)]
        public void StartDeviceStatusCheckJob(EntityLayer.EL_Installation _EL_Installation)
        {
            RecurringJob.AddOrUpdate($"DeviceStatusCheck26hours({_EL_Installation.ifkResellerId.ToString()})", () => SendWebhookDeviceStatus(_EL_Installation, null), Cron.HourInterval(26));
        }

        [AutomaticRetry(Attempts = 10)]
        public void StartDeviceStatusCheckJobForReports(EntityLayer.EL_Installation _EL_Installation)
        {
            RecurringJob.AddOrUpdate($"DeviceStatusCheck({_EL_Installation.vResellerName}:{_EL_Installation.ifkResellerId.ToString()})", () => SendWebhookDeviceStatus(_EL_Installation, null), Cron.HourInterval(26));
        }


        // https method  webhook to call  reseller endpoint  
        public void SendWebhookInstallationStatus(EntityLayer.InstallationStatus _Status)
        {

            var _statusCode = 1000;

            var _messageError = "";


            if (!string.IsNullOrWhiteSpace(_Status.vInstallationUrl))
            {

                string _Url, _installationType = "";

                if (_Status.Installation)
                {
                    _installationType = "installation";

                    _Url = _Status.vInstallationUrl;
                }
                else
                {
                    _installationType = "deinstallation";

                    _Url = _Status.vDe_InstallationUrl;
                }


                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri(_Url);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var routeString = $"?imei={_Status.Imei}&{_installationType}={true}";


#if DEBUG
                // debug stuff goes here
#else
                    HttpResponseMessage response = client.GetAsync(routeString).Result;  // Blocking call!  

                    _messageError = response.ReasonPhrase;

                    if (!response.IsSuccessStatusCode)
                    LogError.RegisterErrorInLogFile( "Bal_Microservice.cs", "SendWebhookInstallationStatus()", _messageError);
                                                                      

#endif


            }
        }

        [CleanupFilter]
        public void SendWebhookDeviceStatus(EL_Installation _EL_Installation, PerformContext context)
        {

            LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "SendWebhookDeviceStatus()", "Starting  webhook job ...");

            ObjectCache cache = MemoryCache.Default;

            var api = JobStorage.Current.GetMonitoringApi();

            var job = api.JobDetails(context.BackgroundJob.Id);

            InstallationStatus _StatusDetails;

            if (!cache.Contains(context.BackgroundJob.Id))
            {
                _StatusDetails = GetInstallationWebHookDetails(_EL_Installation);

                // Store data in the cache    
                CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();

                cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddHours(1.0);

                cache.Add(context.BackgroundJob.Id, _StatusDetails, cacheItemPolicy);

            }

            var cached = cache.Get(context.BackgroundJob.Id) as InstallationStatus;


            var _DevicesStatusLogs = new
            {
                  cached.DeviceStatusLogs
            };


            var json = JsonConvert.SerializeObject(_DevicesStatusLogs);

            var location_content = new StringContent(json, Encoding.UTF8, "application/json");


            LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "SendWebhookDeviceStatus()", $"Attempting to send webhook data to ({cached.vDeviceStatusCheckUrl}) ");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HttpResponseMessage response = client.PostAsync(allTo, new FormUrlEncodedContent(content)).Result;

                HttpResponseMessage response = client.PostAsync(cached.vDeviceStatusCheckUrl, location_content).Result;

                if (!response.IsSuccessStatusCode)
                   LogError.RegisterErrorInLogFile( "Bal_Microservice.cs", "SendWebhookDeviceStatus()", response.ReasonPhrase);

            }

            LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "SendWebhookDeviceStatus()", $"Exiting  webhook job ({cached.vDeviceStatusCheckUrl})... ");


        }

        public InstallationStatus GetInstallationWebHookDetails(EL_Installation __EL_Installation)
        {
            var _installationDetails = new InstallationStatus();

            try
            {
                var __Deserialized_EL_Installation = JsonConvert.DeserializeObject<EL_Installation>(JsonConvert.SerializeObject(__EL_Installation));

                __Deserialized_EL_Installation.Operation = 13;

                var ds = DAL_Installation.ExecuteInstallationAction(__Deserialized_EL_Installation);

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
                                LastReportedDate = Convert.ToDateTime(row["dGPSDateTime"]),
                                Status = Convert.ToBoolean(row["Status"]).ToString(),
                                DeviceCustomType = Convert.ToString(row["DeviceCustomType"]),
                                HardwareID = Convert.ToString(row["HardwareID"])
                            });

                    }

                    tableCount++;
                }
            }
            catch(Exception ex)
            {
                LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "GetInstallationWebHookDetails()", $" {ex.Message} {ex.StackTrace} ");

            }
            return _installationDetails;
        }

    }

}
