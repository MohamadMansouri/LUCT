using LUCT.Helpers;
using LUCT.Services;
using LUCT.ViewModels;
using System;
using System.Collections.Generic;
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
    public sealed partial class Logs : Page
    {
        private AppDbContext _database;
        public List<string> Items { get; set; }
        public ChartViewModel chartViewModel { get; set; }
        public Logs()
        {
            this.InitializeComponent();

            _database = new AppDbContext();


            LeftMenuControl.Tapped += LeftMenuControl_Tapped;
            Items = ApplicationHelper.GetLeftMenuItems(ApplicationType.Logs);
            LeftMenuControl.ItemsSource = Items;

            chartViewModel = new ChartViewModel();

        }

        private void LeftMenuControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LeftMenuControl.SelectedItem != null && combo.SelectedItem != null)
            {
                switch (LeftMenuControl.SelectedItem.ToString())
                {
                    case "Temperature":
                        chartViewModel.DataType = DataByType.Temperature;
                        break;
                    case "Humidity":
                        chartViewModel.DataType = DataByType.Humidity;
                        break;
                    case "Tank Level":
                        chartViewModel.DataType = DataByType.TankLevel;
                        break;
                }
                Chart.DataContext = chartViewModel.GetData;
            }
        }
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;
            if (LeftMenuControl.SelectedItem != null)
            {
                chartViewModel.DataType = ApplicationPage.DataType;
                switch (cb.SelectedIndex)
                {
                    case 0:
                        chartViewModel.DataBy = DataByEnum.Day;
                        chartViewModel.GetDataBy(DataByEnum.Day);
                        TimeLabel.LabelFormat = "{0:HH:mm}";
                        break;
                    case 1:
                        chartViewModel.DataBy = DataByEnum.Week;
                        chartViewModel.GetDataBy(DataByEnum.Week);
                        TimeLabel.LabelFormat = "{0:d}";
                        break;
                    case 2:
                        chartViewModel.DataBy = DataByEnum.Month;
                        chartViewModel.GetDataBy(DataByEnum.Month);
                        TimeLabel.LabelFormat = "{0:MMM}";
                        break;
                }
                Chart.DataContext = chartViewModel.GetData;
            }
        }
        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
