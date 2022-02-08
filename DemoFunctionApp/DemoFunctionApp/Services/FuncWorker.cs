using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFunctionApp.Services
{
    public class FuncWorker
    {

        private BlobStorageService blobStorageService;
        private IConfiguration _configuration;
        public FuncWorker(IConfigurationRoot configuration)
        {
            var private_accesskey = configuration.GetConnectionString("AccessKey");
            
            blobStorageService = new BlobStorageService(private_accesskey, configuration["BlobContainter"]);
            _configuration = configuration;
        }

        public async Task<bool> DailyWorkerAsync(string content)
        {
           
            var data = JsonConvert.DeserializeObject<IDictionary<string, object>>(content);
            var devid = data["deviceId"];
            var current_dt = DateTime.UtcNow;
            var filepath = "/aggregated-data/" + devid + current_dt.Year.ToString() + "/daily/" + current_dt.Month.ToString() + "/" + current_dt.Day.ToString() + ".json";
            if (blobStorageService.CheckBlobExistAsync(filepath))
            {

            }
            else
            {
                blobStorageService.UploadFileToBlobAsync(filepath, content);
            }
            return true;

        }

        public async Task<bool> WeeklyWorkerAsync(string content)
        {

            var data = JsonConvert.DeserializeObject<IDictionary<string, object>>(content);
            var devid = data["deviceId"];
            var current_dt = DateTime.UtcNow;
            var filepath = "/aggregated-data/" + devid + current_dt.Year.ToString() + "/Weekly/"+Helper.Helper.GetIso8601WeekOfYear( current_dt)+".json";
            if (blobStorageService.CheckBlobExistAsync(filepath))
            {

            }
            else
            {
                blobStorageService.UploadFileToBlobAsync(filepath, content);
            }
            return true;

        }

        public async Task<bool> MonthlyWorkerAsync(string content)
        {

            var data = JsonConvert.DeserializeObject<IDictionary<string, object>>(content);
            var devid = data["deviceId"];
            var current_dt = DateTime.UtcNow;
            var filepath = "/aggregated-data/" + devid + current_dt.Year.ToString() + "/monthly/" + current_dt.Month.ToString() +".json";
            if (blobStorageService.CheckBlobExistAsync(filepath))
            {

            }
            else
            {
                blobStorageService.UploadFileToBlobAsync(filepath, content);
            }
            return true;

        }
    }
}
