using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WLT.BusinessLogic.BAL;
using Newtonsoft.Json;
using static WLT.BusinessLogic.BAL.Bal_Microservice;
using Hangfire;
using WLT.DataAccessLayer.DAL;
using WLT.EntityLayer;
using System.Data;
using System.Configuration;
using Hangfire.Server;
using Hangfire.Common;
using Hangfire.States;
using System.Runtime.Caching;
using WLT.ErrorLog;

namespace WLT.BusinessLogic.BAL
{

    public class Bal_Microservice
    {


        // hangfire installation  job to  activate webhook, keep trying for ten times before quieting retries;     
        [AutomaticRetry(Attempts = 10)]
        public void StartWebHookInstallationJob(InstallationStatus _statusDetails)
        {
            BackgroundJob.Schedule(() => SendWebhookInstallationStatus(_statusDetails), TimeSpan.FromSeconds(3));
        }


        // hangfire installation  job to  activate webhook, keep trying for ten times before quieting retries;     
        [AutomaticRetry(Attempts = 10)]
        public void StartReportWebHookInstallationStatusCheckup(InstallationStatus _statusDetails)
        {
            BackgroundJob.Schedule(() => SendWebhookInstallationStatus(_statusDetails), TimeSpan.FromSeconds(1));
        }





        // hangfire status check  job to  activate webhook, keep trying for ten times before quieting retries;
        [AutomaticRetry(Attempts = 10)]
        public void StartDeviceStatusCheckJob(EL_Installation _EL_Installation)
        {
            RecurringJob.AddOrUpdate($"DeviceStatusCheck26hours({_EL_Installation.ifkResellerId.ToString()})", () => SendWebhookDeviceStatus(_EL_Installation, null), Cron.Daily);
        }

        [AutomaticRetry(Attempts = 10)]
        public void StartDeviceStatusCheckJobForReports(EL_Installation _EL_Installation)
        {
            RecurringJob.AddOrUpdate($"DeviceStatusCheck({_EL_Installation.vResellerName}:{_EL_Installation.ifkResellerId.ToString()})", () => SendWebhookDeviceStatus(_EL_Installation, null), Cron.Daily);
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
        public void SendWebhookDeviceStatus(EntityLayer.EL_Installation _EL_Installation, PerformContext context)
        {

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
                DevicesStatusLogs = cached.DeviceStatusLogs
            };

            if (cached.vDeviceStatusCheckUrl != "" && cached.vDeviceStatusCheckUrl != null)
            {

                var json = JsonConvert.SerializeObject(_DevicesStatusLogs);

                var location_content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {             
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HttpResponseMessage response = client.PostAsync(allTo, new FormUrlEncodedContent(content)).Result;
                if (string.IsNullOrWhiteSpace(cached.vDeviceStatusCheckUrl))                
                    LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "SendWebhookDeviceStatus()", "The vDeviceStatusCheckUrl was empty ");                
                else
                {
                    HttpResponseMessage response = client.PostAsync(cached.vDeviceStatusCheckUrl, location_content).Result;

                    if (!response.IsSuccessStatusCode)
                        LogError.RegisterErrorInLogFile("Bal_Microservice.cs", "SendWebhookDeviceStatus()", response.ReasonPhrase);
                }
            }
            

            }
        }

        public InstallationStatus GetInstallationWebHookDetails(EL_Installation __EL_Installation)
        {
            var _installationDetails = new InstallationStatus();

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

            return _installationDetails;
        }

    }


    #region  Custom  hangfire filter to  clean up after performaing its tasks 
    public class CleanupFilter : JobFilterAttribute, IElectStateFilter, IServerFilter
    {
        public void OnStateElection(ElectStateContext context)
        {
            // all failed job after retry attempts comes here
            var failedState = context.CandidateState as FailedState;

            var succeededState = context.CandidateState as SucceededState;

            int retryCount = context.GetJobParameter<int>("RetryCount");

            if (retryCount == 10 || succeededState != null)
            {
                ObjectCache cache = MemoryCache.Default;

                cache.Remove(context.BackgroundJob.Id);

            }
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            // do something 

        }

        public void OnPerformed(PerformedContext filterContext)
        {
            // you have an option to move all code here on OnPerforming if you want.
            var api = JobStorage.Current.GetMonitoringApi();

            var job = api.JobDetails(filterContext.BackgroundJob.Id);


            foreach (var history in job.History)
            {
                // check reason property and you will find a string with
                // Retry attempt 3 of 3: The method or operation is not implemented.            
            }

            var jobParam = job.Job.Args as EL_Installation;

        }
    }

    #endregion
}
