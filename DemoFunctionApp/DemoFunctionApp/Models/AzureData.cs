using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFunctionApp.Models
{
    public class AzureData
    {
        public string deviceId { get; set; }

        public string deviceType { get; set; }

        public IList<Zone> Zones { get; set; }

        public Tank tank { get; set; }
        public string operationMode { get; set; }

        public int quietLevel { get; set; }

        public string specialOperationMode { get; set; }

        public string heatpumpDirection { get; set; }

        public bool boilerStatus { get; set; }

        public bool deicingStatus { get; set; }

        public bool forceHeater { get; set; }

        public int outdoorTemperature { get; set; }
    }
}
