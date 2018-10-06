using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace LUCT
{
    public class Ticker : INotifyPropertyChanged
    {
        public Ticker()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, object e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Now"));
        }

        public string Now
        {
            get { return string.Format("{0:HH:mm:ss tt}", DateTime.Now); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
