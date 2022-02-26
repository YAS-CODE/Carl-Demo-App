using DemoWebApplication.Models;
using DemoWebApplication.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDataController : ControllerBase
    {
        private IBlobStorageService _blobStorageService;
        public DeviceDataController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }
        [HttpGet("GetDailyData")]
        public async Task<List<DailyResponse>> GetDailyDataAsync()
        {
            var dailyres = new List<DailyResponse>();
            var aggreatefolder = await _blobStorageService.GetListAsync("/",false,null, null);
            if (aggreatefolder.Contains("aggregated-data/"))
            {
                var devfolders = await _blobStorageService.GetListAsync("/" , false, "aggregated-data/", null);
                foreach (var devfolder in devfolders)
                {
                    var eventfolders = await _blobStorageService.GetListAsync("/" , false, devfolder, null);
                    if (eventfolders.Contains(devfolder+"daily/"))
                    {
                        var monthfolders = await _blobStorageService.GetListAsync("/" , false, eventfolders.Find(x=> x.EndsWith("daily/")), null);
                        foreach(var monthfolder in monthfolders)
                        {
                            var dayfiles = await _blobStorageService.GetListAsync("/", true, monthfolder, null);
                            foreach (var dayfile in dayfiles)
                            {
                                var data = await _blobStorageService.ReadFileToBlobAsync(dayfile);
                                var raw_data = dayfile.Split("/");
                                dailyres.Add(new DailyResponse { DeviceId = raw_data[1], Month = raw_data[3], Day = raw_data[4].Split(".")[0], Data = data });
                            }

                        }
                        
                    }
                }
            }

            return dailyres;
        }

        [HttpGet("GetMonthlyData")]
        public async Task<List<MonthlyResponse>> GetMonthlyDataAsync()
        {
            var monthlyres = new List<MonthlyResponse>();
            var aggreatefolder = await _blobStorageService.GetListAsync("/", false, null, null);
            if (aggreatefolder.Contains("aggregated-data/"))
            {
                var devfolders = await _blobStorageService.GetListAsync("/", false, "aggregated-data/", null);
                foreach (var devfolder in devfolders)
                {
                    var eventfolders = await _blobStorageService.GetListAsync("/", false, devfolder, null);
                    if (eventfolders.Contains(devfolder + "monthly/"))
                    {
                        var monthfolders = await _blobStorageService.GetListAsync("/", true, eventfolders.Find(x => x.EndsWith("monthly/")), null);
                        foreach (var monthfile in monthfolders)
                        {
                            var data = await _blobStorageService.ReadFileToBlobAsync(monthfile);
                            var raw_data = monthfile.Split("/");
                            monthlyres.Add(new MonthlyResponse { DeviceId = raw_data[1], Month = raw_data[3].Split(".")[0], Data = data });
                            

                        }

                    }
                }
            }

            return monthlyres;
        }

        [HttpGet("GetWeeklyData")]
        public async Task<List<WeeklyResponse>> GetWeeklyDataAsync()
        {
            var weeklyres = new List<WeeklyResponse>();
            var aggreatefolder = await _blobStorageService.GetListAsync("/", false, null, null);
            if (aggreatefolder.Contains("aggregated-data/"))
            {
                var devfolders = await _blobStorageService.GetListAsync("/", false, "aggregated-data/", null);
                foreach (var devfolder in devfolders)
                {
                    var eventfolders = await _blobStorageService.GetListAsync("/", false, devfolder, null);
                    if (eventfolders.Contains(devfolder + "Weekly/"))
                    {
                        var monthfolders = await _blobStorageService.GetListAsync("/", true, eventfolders.Find(x => x.EndsWith("Weekly/")), null);
                        foreach (var monthfile in monthfolders)
                        {
                            var data = await _blobStorageService.ReadFileToBlobAsync(monthfile);
                            var raw_data = monthfile.Split("/");
                            weeklyres.Add(new WeeklyResponse { DeviceId = raw_data[1], Week = raw_data[3].Split(".")[0], Data = data });


                        }

                    }
                }
            }

            return weeklyres;
        }
    }
}
