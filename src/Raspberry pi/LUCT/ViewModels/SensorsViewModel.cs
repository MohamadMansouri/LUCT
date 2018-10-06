using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.ViewModels
{
    public class SensorsViewModel
    {
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double TankLevel { get; set; }
        public DateTime Time { get; set; }
    }
}
