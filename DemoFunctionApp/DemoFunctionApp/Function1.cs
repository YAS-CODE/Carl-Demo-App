using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using DemoFunctionApp.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoFunctionApp
{
    public class Function1
    {


        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("%ScheduleTriggerTime%")] TimerInfo myTimer, ILogger log, Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            var configuration = new ConfigurationBuilder()
        .SetBasePath(context.FunctionAppDirectory)
        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables()
        .Build();
            var fnw = new FuncWorker(configuration);
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            var content = await Helper.Helper.GetAsync(configuration["DeviceUrl"]);
            


            if (await fnw.DailyWorkerAsync(content))
            {
                log.LogInformation($"Daily Worker Completed successfuly function executed at: {DateTime.Now}");
            }
            else
            {
                log.LogInformation($"Daily Worker Failed function executed at: {DateTime.Now}");
            }

            if (await fnw.WeeklyWorkerAsync(content))
            {
                log.LogInformation($"Weekly Worker Completed successfuly function executed at: {DateTime.Now}");
            }
            else
            {
                log.LogInformation($"Weekly Worker Failed function executed at: {DateTime.Now}");
            }

            if (await fnw.MonthlyWorkerAsync(content))
            {
                log.LogInformation($"Monthly Worker Completed successfuly function executed at: {DateTime.Now}");
            }
            else
            {
                log.LogInformation($"Monthly Worker Failed function executed at: {DateTime.Now}");
            }
        }

        
    }
}
