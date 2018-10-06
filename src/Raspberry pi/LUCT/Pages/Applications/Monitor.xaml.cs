using LUCT.Helpers;
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
    public sealed partial class Monitor : Page
    {
        public List<string> Items { get; set; }
        public Monitor()
        {
            this.InitializeComponent();

            LeftMenuControl.Tapped += LeftMenuControl_Tapped;
            Items = ApplicationHelper.GetLeftMenuItems(ApplicationType.Monitor);
            LeftMenuControl.ItemsSource = Items;

        }
        private void LeftMenuControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LeftMenuControl.SelectedItem != null)
            {
                slave2Block.Opacity = 0;
                slave1Block.Opacity = 0;
                switch (LeftMenuControl.SelectedItem.ToString())
                {
                    case "Arduino Slave 1":
                        slave1Block.Opacity = 1;
                        break;
                    case "Arduino Slave 2":
                        slave2Block.Opacity = 1;
                        break;
                }
            }
        }
        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
