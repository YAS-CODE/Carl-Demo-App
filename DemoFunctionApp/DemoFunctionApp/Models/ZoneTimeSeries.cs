using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFunctionApp.Models
{
    public class ZoneTimeSeries
    {
        public string ZoneId { get; set; }
        public string Name { get; set; }

        public int Samples { get; set; }

        public IList<TankTime> TimeSeries { get; set; }
    }
}
