using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Timers;

namespace Api
{
    public class DbMonitoring
    {
        [FunctionName("DbMonitoring")]
        public void Run([TimerTrigger("*/10 * * * * *")]TimerInfo timerInfo, ILogger logger)
        {
            if (timerInfo.IsPastDue)
            {
                logger.LogInformation("Timer is running late!");
            }

            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            logger.LogInformation($"Next timer schedule = {timerInfo?.ScheduleStatus?.Next}");
        }
    }
}
