using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.Models.Database
{
    public class Sms
    {
        [PrimaryKey]
        public int Id { get; set; }
        public float TemperatureBelow { get; set; } = 0;
        public float TemperatureAbove { get; set; } = 0;
        public float HumidityBelow { get; set; } = 0;
        public float HumidityAbove { get; set; } = 0;
        public float TankLevelBelow { get; set; } = 0;
        public float TankLevelAbove { get; set; } = 0;
        public bool TemperatureEnabled { get; set; } = false;
        public bool HumidityEnabled { get; set; } = false;
        public bool TankLevelEnabled { get; set; } = false;
        public bool SmsEnabled { get; set; } = false;
    }
}
