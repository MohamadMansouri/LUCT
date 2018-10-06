using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LUCT
{
    public  class AppKeys : INotifyPropertyChanged
    {
        bool k = false;
        public AppKeys()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Timer_Tick(object sender, object e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Slave1Reachable"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Slave2Reachable"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Slave1State"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Slave2State"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Switch1"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Switch1State"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Switch2State"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Switch2"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PillState"));

        }
        public static SolidColorBrush Slave1Reachable { get; set; } = new SolidColorBrush(Colors.Red);
        public static SolidColorBrush Slave2Reachable { get; set; } = new SolidColorBrush(Colors.Red);
        public static bool Switch1 { get; set; } = false;
        public static bool Switch2 { get; set; } = false;
        public static bool Switch1State { get; set; }
        public static bool Switch2State { get; set; }
        public static bool PillState { get; set; } = false;
        public static string Slave1State { get; set; } = "Checking...";
        public static string Slave2State { get; set; } = "Checking...";
    }
}
