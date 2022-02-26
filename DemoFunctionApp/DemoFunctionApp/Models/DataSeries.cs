using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoFunctionApp.Models
{
    class DataSeries
    {
        public bool IsFirstPeriod { get; set; }
        public int Samples { get; set; }

        public IList<TankTime> TankTimeSeries = new List<TankTime>();

        public IList<ZoneTimeSeries> ZoneTimeSeries = new List<ZoneTimeSeries>();
    }
}
