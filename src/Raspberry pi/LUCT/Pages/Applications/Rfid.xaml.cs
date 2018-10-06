using LUCT.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class Rfid : Page
    {
        public ObservableCollection<LUCT.Models.Database.Rfid> Items { get; set; }
        AppDbContext _database;
        Models.Security _security;
        
        int _itemsCount;
        public Rfid()
        {
            this.InitializeComponent();

            _database = new AppDbContext();


            UpdateUI();
        }

        private void UpdateUI()
        {
            Items = new ObservableCollection<Models.Database.Rfid>(_database.GetModel<LUCT.Models.Database.Rfid>());
            _itemsCount = Items.Count;
            RfidData.ItemsSource = Items;

            _security = _database.GetModel<Models.Security>()[0];
            banQuota.Text = _security.BanQuota.ToString();
            banTime.Text = _security.BanTime.ToString();
        }

        private void backButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(LUCT.Pages.Applications.Security));
        }


        private async void Add_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_itemsCount > 5)
            {
                var cd = new ContentDialog()
                {
                    Title = "Can't add more than 5 codes",
                    PrimaryButtonText = "Done",
                };
                await cd.ShowAsync();
            }
            else
            {
                _database.InsertModel<Models.Database.Rfid>(new Models.Database.Rfid() { Text = "" });
                UpdateUI();
            }
        }

        private async void Remove_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_itemsCount == 0)
            {
                var cd = new ContentDialog()
                {
                    Title = "Nothing to remove",
                    PrimaryButtonText = "Done",
                };
                await cd.ShowAsync();
            }
            else
            {
                var id = ((LUCT.Models.Database.Rfid)RfidData.Items.Last()).Id;
                _database.DeleteModel<Models.Database.Rfid>(id);
                UpdateUI();
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tx = (TextBox)sender;
            var id = Int32.Parse(tx.DataContext.ToString());
            _database.UpdateModel<LUCT.Models.Database.Rfid>(new Models.Database.Rfid() { Id = id, Text = tx.Text });
        }

        private void banSubmit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                _database.UpdateModel<Models.Security>(new Models.Security() { Password = _security.Password, BanQuota = Int32.Parse(banQuota.Text), BanTime = Int32.Parse(banTime.Text), Id = 0 ,KeyPadPassword = password.Text});
                status.Text = "Data saved successfully";
            }
            catch(Exception ex)
            {
                status.Text = "An error occured, check your data";
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.DataContext = sender;
            ParentedPopup.IsOpen = true;
        }
    }

    public class RfidModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
