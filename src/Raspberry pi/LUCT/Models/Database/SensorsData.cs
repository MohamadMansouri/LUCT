using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.DatabaseModels
{
    public class SensorsData
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public double Humidity { get; set; }
        public double Temperature { get; set; }
        public double TankLevel { get; set; }
        public DateTime Time { get; set; }
        public double Current { get; set; }
    }
}
