using LUCT.Services;
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
    public sealed partial class Control : Page
    {
        public Control()
        {
            this.InitializeComponent();
        }
        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void conditionerRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private async void lamp2Radio_Checked(object sender, RoutedEventArgs e)
        {
            AppKeys.Switch1 = true;
        }

        private async void lamp1Radio_Checked(object sender, RoutedEventArgs e)
        {
            AppKeys.Switch2 = true;
        }
    }
}
