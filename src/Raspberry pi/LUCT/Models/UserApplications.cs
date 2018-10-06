using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.Models
{
    public class UserApplications
    {
        private string _base = "../Assets/Apps/";
        public string Image { get; set; }
        public string Label { get; set; }

        public List<UserApplications> GetSampleData()
        {
            return new List<UserApplications>()
            {
                new UserApplications()
                {
                    Label = "Security",
                    Image = _base+ "Security.png"
                },
                new UserApplications()
                {
                    Label = "Monitor",
                    Image = _base+ "Monitor.png"
                },
                new UserApplications()
                {
                    Label = "Control",
                    Image = _base+ "Control.png"
                },
                new UserApplications()
                {
                    Label = "Sms",
                    Image = _base+ "Sms.png"
                },
                new UserApplications()
                {
                    Label = "Pills",
                    Image = _base+ "Pills.png"
                },
                new UserApplications()
                {
                    Label = "Logs",
                    Image = _base+ "Logs.png"
                },
                new UserApplications()
                {
                    Label = "Events",
                    Image = _base+ "About.png"
                },
                new UserApplications()
                {
                    Label = "Settings",
                    Image = _base+ "Settings.png"
                }
            };
        }
    }
}
