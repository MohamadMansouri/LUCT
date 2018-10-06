using LUCT.Models.Enums;
using LUCT.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Telerik.UI.Xaml.Controls.Data.ListView.Commands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Telerik.UI.Xaml.Controls.Chart;
using LUCT.ViewModels;
using Telerik.UI.Xaml.Controls.Data;
using LUCT.Helpers;
using Windows.UI;
using LUCT.Pages.Applications;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LUCT.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ApplicationPage : Page
    {
        public ApplicationModel Model { get; set; }
        public List<string> Items { get; set; }
        private AppDbContext _database;
        //Logging
        public ChartViewModel chartViewModel { get; set; }
        public static DataByType DataType { get; set; }
        //Monitoring
        
        public ApplicationPage()
        {
            
            this.InitializeComponent();

            _database = new AppDbContext();

            Items = new List<string>();

            LeftMenuControl.Tapped += LeftMenuControl_Tapped;

            chartViewModel = new ChartViewModel();

            LeftMenuControl.ItemsSource = Items;
        }

        private void LeftMenuControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HandleLeftMenuTapped(Model);
        }

        private void HandleLeftMenuTapped(ApplicationModel type)
        {
            if (Model.IsMonitor)
            {
                Frame.Navigate(typeof(Monitor));
            }
            if (Model.IsLogs)
            {
            }
            if (Model.IsSecurity)
            {
                if (LeftMenuControl.SelectedItem != null)
                {
                    switch (LeftMenuControl.SelectedItem.ToString())
                    {
                        case "Ban Summary":
                            BanGrid.Opacity = 1;
                            break;
                    }
                }
            }
            if (Model.IsSettings)
            {
                
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var type = e.Parameter.ToString();
            Model = new ApplicationModel();
            switch (type)
            {
                case "About":
                    Model.IsAbout = true;
                    HandleIsAbout();
                    break;
                case "Control":
                    Model.IsControl = true;
                    HandleIsControl();
                    break;
                case "Logs":
                    Model.IsLogs = true;
                    HandleIsLogs();
                    break;
                case "Monitor":
                    Model.IsMonitor = true;
                    HandleIsMonitor();
                    break;
                case "Pills":
                    Model.IsPills = true;
                    HandleIsPills();
                    break;
                case "Security":
                    Model.IsSecurity = true;
                    HandleIsSecurity();
                    break;
                case "Settings":
                    Model.IsSettings = true;
                    HandleIsSettings();
                    break;
                case "Sms":
                    Model.IsSms = true;
                    HandleIsSms();
                    break;
            }
            if (Model.IsLogs)
            {
                
            }
        }

        private void HandleIsSms()
        {   
            throw new NotImplementedException();
        }

        private void HandleIsSettings()
        {

            LeftMenuControl.ItemsSource = ApplicationHelper.GetLeftMenuItems(ApplicationType.Settings);
        }

        private void HandleIsSecurity()
        {
            //SecurityApp.Opacity = 1;

            //LeftMenuControl.ItemsSource = ApplicationHelper.GetLeftMenuItems(ApplicationType.Security);
            //BanGrid.ItemsSource = _database.GetBanData();
            ((Frame)Window.Current.Content).Navigate(typeof(SecurityTestPage));
        }

        private void HandleIsPills()
        {
            throw new NotImplementedException();
        }

        private void HandleIsMonitor()
        {
            //slaveState.Opacity = 1;

            
        }

        private void HandleIsLogs()
        {

            Items = ApplicationHelper.GetLeftMenuItems(ApplicationType.Logs);
            LeftMenuControl.ItemsSource = Items;
        }

        private void HandleIsControl()
        {
        }

        private void HandleIsAbout()
        {
            throw new NotImplementedException();
        }
        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
        public void UpdateChartDataContext()
        {
        }

        
    }
    public class ApplicationModel
    {
        public bool IsSecurity { get; set; } = false;
        public bool IsMonitor { get; set; } = false;
        public bool IsControl { get; set; } = false;
        public bool IsSms { get; set; } = false;
        public bool IsPills { get; set; } = false;
        public bool IsLogs { get; set; } = false;
        public bool IsAbout { get; set; } = false;
        public bool IsSettings { get; set; } = false;
    }
    public enum ApplicationType
    {
        Security,
        Monitor,
        Control,
        Settings,
        Logs,
        About,
        Sms,
        Pills
    }
}
