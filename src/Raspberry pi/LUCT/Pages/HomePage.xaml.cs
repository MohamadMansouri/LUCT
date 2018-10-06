using LUCT.Models;
using LUCT.Models.Enums;
using LUCT.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using LUCT.Pages.Applications;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LUCT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public List<UserApplications> Applications { get; set; }
        public HomePage()
        {
            this.InitializeComponent();
            Applications = new UserApplications().GetSampleData();
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.Background = App.BackgroundImage;
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var t = ((StackPanel)sender).DataContext.ToString();
            if (t == "Monitor")
                Frame.Navigate(typeof(Monitor));
            else if (t == "Logs")
                Frame.Navigate(typeof(Logs));
            else if (t == "Settings")
                Frame.Navigate(typeof(Settings));
            else if (t == "Security")
                Frame.Navigate(typeof(LUCT.Pages.Applications.Security));
            else if (t == "Sms")
                Frame.Navigate(typeof(Sms));
            else if (t == "Pills")
                Frame.Navigate(typeof(Pills));
            else if (t == "Control")
                Frame.Navigate(typeof(LUCT.Pages.Applications.Control));
            else if (t == "About")
                Frame.Navigate(typeof(LUCT.Pages.Applications.Events));
            else
               Frame.Navigate(typeof(ApplicationPage),t);
        }

        private void StackPanel_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {

        }

        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
