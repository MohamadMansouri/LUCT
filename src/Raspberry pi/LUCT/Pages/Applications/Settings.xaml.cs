using LUCT.Helpers;
using LUCT.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LUCT.Pages.Applications
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        private AppDbContext _database;
        public List<string> Items { get; set; }
        public Settings()
        {
            this.InitializeComponent();

            _database = new AppDbContext();
        }


        private void PasswordSaveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var dialog = new MessageDialog("");
            int passwordInt = 0;
            try
            {
                passwordInt = Int32.Parse(PasswordText.Text);
            }
            catch (Exception ex)
            {
                dialog.Content = "Please insert an integer of 4 digits";
            }
            if (!(PasswordText.Text == passwordInt.ToString()) || PasswordText.Text.Length != 4)
            {
                dialog.Content = "Please insert an integer of 4 digits";
            }
            else
            {
                _database.UpdatePassword(PasswordText.Text);
                PasswordStatus.Text = "Password Updated Successfully";
            }
            //dialog.ShowAsync();
        }
        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void PasswordText_GotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.DataContext = sender;
            ParentedPopup.IsOpen = true;
        }
    }
}
