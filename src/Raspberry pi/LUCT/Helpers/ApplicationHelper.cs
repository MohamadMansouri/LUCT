using LUCT.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT.Helpers
{
    public static class ApplicationHelper
    {
        public static List<string> GetLeftMenuItems(ApplicationType type)
        {
            List<string> items = new List<string>();
            switch (type)
            {
                case ApplicationType.Logs:
                    items = new List<string>()
                    {
                        "Temperature",
                        "Humidity",
                        "Tank Level"
                    };
                    break;
                case ApplicationType.About:
                    items = new List<string>();
                    break;
                case ApplicationType.Control:
                    items = new List<string>();
                    break;
                case ApplicationType.Monitor:
                    items = new List<string>()
                    {
                        "Arduino Slave 1",
                        "Arduino Slave 2"
                    };
                    break;
                case ApplicationType.Pills:
                    items = new List<string>()
                    {
                        "Configure",
                        "Manage",
                        "Pills Taken"
                    };
                    break;
                case ApplicationType.Security:
                    items = new List<string>()
                    {
                        "Rfid",
                        "Camera"
                    };
                    break;
                case ApplicationType.Settings:
                    items = new List<string>()
                    {
                        "LockPad Password",
                        "Card ID"
                    };
                    break;
                case ApplicationType.Sms:
                    items = new List<string>
                    {
                        "Weather Station",
                        "Tank Level",
                        "Slaves",
                        "Security"
                    };
                    break;
            }
            return items;
        }
    }
}
