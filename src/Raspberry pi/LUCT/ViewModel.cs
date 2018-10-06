using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LUCT
{
    public class ViewModel : INotifyPropertyChanged
    {
        private double temperature;

        public ViewModel()
        {
            this.Temperature = 35;
            this.Min = 20;
            this.Average = 30;
            this.Max = 50;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double Temperature
        {
            get
            {
                return this.temperature;
            }
            set
            {
                if (this.temperature != value)
                {
                    this.temperature = value;
                    this.OnPropertyChanged("Temperature");
                }
            }
        }

        public double Min { get; set; }

        public double Average { get; set; }

        public double Max { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
