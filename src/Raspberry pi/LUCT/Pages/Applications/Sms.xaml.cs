using LUCT.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LUCT.Pages.Applications
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sms : Page
    {
        private LUCT.Models.Database.Sms smsData;
        public string Variable
        {
            get { return (string)GetValue(variableProperty); }
            set { SetValue(variableProperty, value); }
        }

        public static readonly DependencyProperty variableProperty =
            DependencyProperty.Register("Variable", typeof(string), typeof(Sms), new PropertyMetadata(null));
        private AppDbContext _database;
        public Sms()
        {
            this.InitializeComponent();
            DataContext = this;
            _database = new AppDbContext();
            Initialize();
        }

        private void Initialize()
        {
            smsData = _database.GetFirstModel<LUCT.Models.Database.Sms>();
            DisableAll.IsOn = smsData.SmsEnabled;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            switch (cb.SelectedIndex)
            {
                case 0:
                    Variable = "Temperature";
                    Below.Text = smsData.TemperatureBelow.ToString();
                    Above.Text = smsData.TemperatureAbove.ToString();
                    DisableVariable.IsOn = smsData.TemperatureEnabled;
                    break;
                case 1:
                    Variable = "Humidity";
                    Below.Text = smsData.HumidityBelow.ToString();
                    Above.Text = smsData.HumidityAbove.ToString();
                    DisableVariable.IsOn = smsData.HumidityEnabled;
                    break;
                case 2:
                    Variable = "Tank Level";
                    Below.Text = smsData.TankLevelBelow.ToString();
                    Above.Text = smsData.TankLevelAbove.ToString();
                    DisableVariable.IsOn = smsData.TankLevelEnabled;
                    break;
            }
            
        }

        private void DisableAll_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                smsData.SmsEnabled = toggleSwitch.IsOn;
                _database.UpdateModel<LUCT.Models.Database.Sms>(smsData);
            }
        }
        private void DisableVariable_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                
            }
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    smsData.TemperatureBelow = float.Parse(Below.Text);
                    smsData.TemperatureAbove = float.Parse(Above.Text);
                    smsData.TemperatureEnabled = DisableVariable.IsOn;
                    break;
                case 1:
                    smsData.HumidityBelow = float.Parse(Below.Text);
                    smsData.HumidityAbove = float.Parse(Above.Text);
                    smsData.HumidityEnabled = DisableVariable.IsOn;
                    break;
                case 2:
                    smsData.TankLevelBelow = float.Parse(Below.Text);
                    smsData.TankLevelAbove = float.Parse(Above.Text);
                    smsData.TankLevelEnabled = DisableVariable.IsOn;
                    break;
            }
            _database.UpdateModel<LUCT.Models.Database.Sms>(smsData);
            status.Text = "Data saved successfully";
        }
        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void GotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.DataContext = sender;
            ParentedPopup.IsOpen = true;
        }
    }
    public sealed class NullToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? false : true;
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
