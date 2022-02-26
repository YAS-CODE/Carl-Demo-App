using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFunctionApp.Models
{
    public class TankTime
    {
        public DateTime DateTime { get; set; }
        public float SetTemperature { get; set; }

        public int CurrentTemperature { get; set; }

        public int OutsideTemperature { get; set; }
    }
}
