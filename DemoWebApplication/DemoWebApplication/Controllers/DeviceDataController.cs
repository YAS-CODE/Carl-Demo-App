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
        [HttpGet(Name = "GetDailyData")]
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
                                dailyres.Add(new DailyResponse { DeviceId = devfolder, Month = "1", Day = "2", Data = data });
                            }

                        }
                        
                    }
                }
            }

            return dailyres;
        }
    }
}
