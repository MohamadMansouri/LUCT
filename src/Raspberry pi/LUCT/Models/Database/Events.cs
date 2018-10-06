using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.Models.Database
{
    public class Events
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public bool IsLightsOnEnabled { get; set; }
        public string LightsEnabled { get
            {
                if (IsLightsOnEnabled)
                    return "Lights On";
                return "None";
            }
        }
        public bool IsConditionerOnEnabled { get; set; }
        public string ConditionerEnabled
        {
            get
            {
                if (IsConditionerOnEnabled)
                    return "Conditioner On";
                return "None";
            }
        }
        public bool IsPipeOnEnabled { get; set; }
        public string PipeEnabled
        {
            get
            {
                if (IsPipeOnEnabled)
                    return "Pipe On";
                return "None";
            }
        }
        public bool IsShutdownEnabled { get; set; }
        public string ShutdownEnabled
        {
            get
            {
                if (IsShutdownEnabled)
                    return "Shutdown On";
                return "None";
            }
        }
        public State State { get; set; }
        public string StringState { get
            {
                return Enum.GetName(typeof(State), State);
            }
        }
        public float Value { get; set; }
        public Variable Variable { get; set; }
        public string StringVariable
        {
            get
            {
                return Enum.GetName(typeof(Variable), Variable);
            }
        }

    }
    public enum State
    {
        None,
        Below,
        Above
    }
    public enum Variable
    {
        None,
        Temperature,
        Humidity,
        TankLevel,
        PowerConsumption
    }
}
