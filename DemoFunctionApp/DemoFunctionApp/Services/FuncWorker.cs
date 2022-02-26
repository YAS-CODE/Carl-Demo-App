using DemoFunctionApp.Models;
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
           
            var data = JsonConvert.DeserializeObject<AzureData>(content);
            var devid = data.deviceId;
            var current_dt = DateTime.UtcNow;
            var filepath = "/aggregated-data/" + devid + current_dt.Year.ToString() + "/daily/" + current_dt.Month.ToString() + "/" + current_dt.Day.ToString() + ".json";
            if (blobStorageService.CheckBlobExistAsync(filepath))
            {
                var datajson = await blobStorageService.ReadFileToBlobAsync(filepath);                
                var dataSeries = JsonConvert.DeserializeObject<DataSeries>(datajson);


                
                dataSeries.Samples++;
                dataSeries.IsFirstPeriod = false;
                int count = 0;
                foreach (var zone in data.Zones)
                {
                    
                    dataSeries.ZoneTimeSeries[count++].TimeSeries.Add(new TankTime() { CurrentTemperature = zone.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = zone.minHeatingTemperature, SetTemperature = zone.targetHeatingTemperature });
                    
                }
                dataSeries.TankTimeSeries.Add(new TankTime() { CurrentTemperature = data.tank.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = data.tank.maxHeatingTemperature, SetTemperature = data.tank.targetHeatingTemperature });
                blobStorageService.DeleteBlobData(filepath);
                blobStorageService.UploadFileToBlobAsync(filepath, JsonConvert.SerializeObject(dataSeries));

            }
            else
            {
                DataSeries dataSeries = new DataSeries() { IsFirstPeriod = true, Samples = 1};
                foreach (var zone in data.Zones)
                {
                    List<TankTime> tankTimes = new List<TankTime>();
                    tankTimes.Add(new TankTime() { CurrentTemperature = zone.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = zone.minHeatingTemperature, SetTemperature = zone.targetHeatingTemperature });
                    dataSeries.ZoneTimeSeries.Add(item: new ZoneTimeSeries { ZoneId = "Zone"+zone.Id.ToString(), Name = "Zone" + zone.Id.ToString() , Samples = 1 , TimeSeries = tankTimes });

                }
                dataSeries.TankTimeSeries.Add(new TankTime() { CurrentTemperature = data.tank.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = data.tank.maxHeatingTemperature, SetTemperature = data.tank.targetHeatingTemperature });
                blobStorageService.UploadFileToBlobAsync(filepath, JsonConvert.SerializeObject(dataSeries));
            }
            return true;

        }

        public async Task<bool> WeeklyWorkerAsync(string content)
        {

            var data = JsonConvert.DeserializeObject<AzureData>(content);
            var devid = data.deviceId;
            var current_dt = DateTime.UtcNow;
            var filepath = "/aggregated-data/" + devid + current_dt.Year.ToString() + "/Weekly/"+Helper.Helper.GetIso8601WeekOfYear( current_dt)+".json";
            if (blobStorageService.CheckBlobExistAsync(filepath))
            {
                var datajson = await blobStorageService.ReadFileToBlobAsync(filepath);
                var dataSeries = JsonConvert.DeserializeObject<DataSeries>(datajson);



                dataSeries.Samples++;
                dataSeries.IsFirstPeriod = false;
                int count = 0;
                foreach (var zone in data.Zones)
                {

                    dataSeries.ZoneTimeSeries[count++].TimeSeries.Add(new TankTime() { CurrentTemperature = zone.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = zone.minHeatingTemperature, SetTemperature = zone.targetHeatingTemperature });

                }
                dataSeries.TankTimeSeries.Add(new TankTime() { CurrentTemperature = data.tank.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = data.tank.maxHeatingTemperature, SetTemperature = data.tank.targetHeatingTemperature });
                blobStorageService.DeleteBlobData(filepath);
                blobStorageService.UploadFileToBlobAsync(filepath, JsonConvert.SerializeObject(dataSeries));

            }
            else
            {
                DataSeries dataSeries = new DataSeries() { IsFirstPeriod = true, Samples = 1 };
                foreach (var zone in data.Zones)
                {
                    List<TankTime> tankTimes = new List<TankTime>();
                    tankTimes.Add(new TankTime() { CurrentTemperature = zone.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = zone.minHeatingTemperature, SetTemperature = zone.targetHeatingTemperature });
                    dataSeries.ZoneTimeSeries.Add(item: new ZoneTimeSeries { ZoneId = "Zone" + zone.Id.ToString(), Name = "Zone" + zone.Id.ToString(), Samples = 1, TimeSeries = tankTimes });

                }
                dataSeries.TankTimeSeries.Add(new TankTime() { CurrentTemperature = data.tank.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = data.tank.maxHeatingTemperature, SetTemperature = data.tank.targetHeatingTemperature });
                blobStorageService.UploadFileToBlobAsync(filepath, JsonConvert.SerializeObject(dataSeries));
            }
            return true;

        }

        public async Task<bool> MonthlyWorkerAsync(string content)
        {

            var data = JsonConvert.DeserializeObject<AzureData>(content);
            var devid = data.deviceId;
            var current_dt = DateTime.UtcNow;
            var filepath = "/aggregated-data/" + devid + current_dt.Year.ToString() + "/monthly/" + current_dt.Month.ToString() +".json";
            if (blobStorageService.CheckBlobExistAsync(filepath))
            {
                var datajson = await blobStorageService.ReadFileToBlobAsync(filepath);
                var dataSeries = JsonConvert.DeserializeObject<DataSeries>(datajson);



                dataSeries.Samples++;
                dataSeries.IsFirstPeriod = false;
                int count = 0;
                foreach (var zone in data.Zones)
                {

                    dataSeries.ZoneTimeSeries[count++].TimeSeries.Add(new TankTime() { CurrentTemperature = zone.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = zone.minHeatingTemperature, SetTemperature = zone.targetHeatingTemperature });

                }
                dataSeries.TankTimeSeries.Add(new TankTime() { CurrentTemperature = data.tank.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = data.tank.maxHeatingTemperature, SetTemperature = data.tank.targetHeatingTemperature });
                blobStorageService.DeleteBlobData(filepath);
                blobStorageService.UploadFileToBlobAsync(filepath, JsonConvert.SerializeObject(dataSeries));

            }
            else
            {
                DataSeries dataSeries = new DataSeries() { IsFirstPeriod = true, Samples = 1 };
                foreach (var zone in data.Zones)
                {
                    List<TankTime> tankTimes = new List<TankTime>();
                    tankTimes.Add(new TankTime() { CurrentTemperature = zone.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = zone.minHeatingTemperature, SetTemperature = zone.targetHeatingTemperature });
                    dataSeries.ZoneTimeSeries.Add(item: new ZoneTimeSeries { ZoneId = "Zone" + zone.Id.ToString(), Name = "Zone" + zone.Id.ToString(), Samples = 1, TimeSeries = tankTimes });

                }
                dataSeries.TankTimeSeries.Add(new TankTime() { CurrentTemperature = data.tank.measuredTemperature, DateTime = DateTime.UtcNow, OutsideTemperature = data.tank.maxHeatingTemperature, SetTemperature = data.tank.targetHeatingTemperature });
                blobStorageService.UploadFileToBlobAsync(filepath, JsonConvert.SerializeObject(dataSeries));
            }
            return true;

        }
    }
}
