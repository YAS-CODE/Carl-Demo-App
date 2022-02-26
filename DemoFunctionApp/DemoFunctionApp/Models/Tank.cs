using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFunctionApp.Models
{
    public class Tank
    {
        public bool operationStatus { get; set; }
        public int measuredTemperature { get; set; }

        public int targetHeatingTemperature { get; set; }

        public int maxHeatingTemperature { get; set; }

        public int minHeatingTemperature { get; set; }
    }
}
