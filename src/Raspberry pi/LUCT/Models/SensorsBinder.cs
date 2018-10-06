using LUCT.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using LUCT.Services;
using LUCT.ViewModels;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.Devices.Enumeration;
using System.Diagnostics;
using LUCT.DatabaseModels;
using LUCT.Models.Database;

namespace LUCT.Models
{
    public class SensorsBinder : INotifyPropertyChanged
    {
        float i = 0;
        TankHelper _tankHelper;
        SensorsData data;
        /// <summary>
        //private SpiDevice Spi_port;
        //GpioPin nRF_CE;
        //private const string SPI_CONTROLLER_NAME = "SPI0";  /* nRF Connected to SPI0                        */
        //private const Int32 SPI_CHIP_SELECT_LINE = 0;
        //nRF nrf;
        /// </summary>
        /// 
        public static SensorsData dt { get; set; } = new SensorsData();
        public SensorsBinder()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            dt = App.LUCTDatabase.GetModelLast<SensorsData>();
            var d = App.LUCTDatabase.GetModel<SensorsData>();
            //var gpio = GpioController.GetDefault();
            //nRF_CE = gpio.OpenPin(26);
            try
            {
                //init_spi();

                //nrf = new nRF(nRF_CE, Spi_port);    //nrf object
                //nrf.init_nRF();
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            _tankHelper = new TankHelper();
        }
        
        private async Task init_spi()
        {
            try
            {
                //var settings = new SpiConnectionSettings(SPI_CHIP_SELECT_LINE); /* Create SPI initialization settings                               */
                //settings.ClockFrequency = 1000000;                             /*     */
                //settings.Mode = SpiMode.Mode0;

                //string spiAqs = SpiDevice.GetDeviceSelector(SPI_CONTROLLER_NAME);       /* Find the selector string for the SPI bus controller          */
                //var devicesInfo = await DeviceInformation.FindAllAsync(spiAqs);         /* Find the SPI bus controller device with our selector string  */
                //Spi_port = await SpiDevice.FromIdAsync(devicesInfo[0].Id, settings);  /* Create an SpiDevice with our bus controller and SPI settings */
            }

            catch (Exception excep)
            {
                throw new Exception("SPI Initialization Failed", excep);
            }
        }
        private void Timer_Tick(object sender, object e)
        {
            i += 0.5f ;
            GetValues();

            try
            {
                //var b = nrf.RX_PAYLOAD();
                //Debug.WriteLine(b);
                //var bytes = nrf.SPI_READ();
                //Debug.WriteLine(bytes[0]);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TanksHeight"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TanksPercent"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Temperature"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Humidity"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Current"));

        }

        private void GetValues()
        {

            //Setting Tank Height
            try
            {
                var perc = 0.3f * i;
                TankHelper.TankVars tv;
                if (dt.TankLevel > 10)
                {
                    tv = _tankHelper.ChangeLevel(0);
                }
                else
                {
                    tv = _tankHelper.ChangeLevel(10 - (float)dt.TankLevel);
                }
                TanksHeight = (float)tv.height;

                //setting tankPercent
                var p = i * 0.3;
                TanksPercent = ((10 - dt.TankLevel) / 10) * 100 + "%";
                var level = dt.TankLevel;
                if (perc > 1 || p > 1)
                    i = 0;


            }
            catch(Exception ex)
            {

            }

            //setting temp and humidity
            //Temperature += i * 3;
            //Humidity += 3 * i + 2.3f;

            Temperature = (float)dt.Temperature;
            Humidity = (float)dt.Humidity;
            Current = (float)dt.Current;

            App.LUCTDatabase.InsertModel(dt);

            var events = App.LUCTDatabase.GetModel<Events>();
            foreach(var e in events)
            {
                if(e.Variable == Variable.TankLevel)
                {
                    if(e.State == State.Below)
                    {
                        if((10 -(float)dt.TankLevel) < e.Value)
                        {
                            if (e.IsLightsOnEnabled)
                            {
                                AppKeys.Switch1 = true;
                                AppKeys.Switch2 = true;
                            }
                        }
                    }
                    if (e.State == State.Above)
                    {
                        if ((10 - (float)dt.TankLevel) > e.Value)
                        {
                            if (e.IsLightsOnEnabled)
                            {
                                AppKeys.Switch1 = true;
                                AppKeys.Switch2 = true;
                            }
                        }
                    }
                }
            }

            //var model = new SensorsViewModel()
            //{
            //    Humidity = this.Humidity,
            //    Temperature = this.Temperature,
            //    TankLevel = level,
            //    Time = DateTime.Now
            //};
        }

        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float TanksHeight { get; set; }
        public string TanksPercent { get; set; }
        public double Current { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
