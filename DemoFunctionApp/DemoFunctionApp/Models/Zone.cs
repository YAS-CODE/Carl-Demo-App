using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFunctionApp.Models
{
    public class Zone
    {
        public bool operationStatus { get; set; }
        public bool temperatureConfigurable { get; set; }

        public int targetCoolingTemperature { get; set; }

        public int targetHeatingTemperature { get; set; }

        public int maxHeatingTemperature { get; set; }

        public int maxCoolingTemperature { get; set; }

        public int Id { get; set; }

        public int measuredTemperature { get; set; }

        public int minCoolingTemperature { get; set; }

        public int minHeatingTemperature { get; set; }
    }
}
