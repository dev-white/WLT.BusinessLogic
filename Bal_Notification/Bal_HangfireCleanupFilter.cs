using Hangfire;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text;
using WLT.EntityLayer;

namespace WLT.BusinessLogic.Bal_Notification
{
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

}
