using LUCT.Helpers;
using LUCT.Services;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Automation.Peers;
using Windows.Foundation;
using System.Linq;
using Windows.Storage;
using LUCT.Pages;
using System.Threading.Tasks;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.Media.Capture;
using System.Collections.Generic;
using LUCT.FacialRecognition;
using System.Diagnostics;
using Microsoft.ProjectOxford.Face;

namespace LUCT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int count = 0;
        private const string star = "   *";
        private string password;
        private DateTime CurrentTime;
        AppDbContext database = new AppDbContext();
        public MainPage()
        {
            this.InitializeComponent();
            backButton.IsTapEnabled = false;
            backButton.Opacity = 0;
            CurrentTime = DateTime.Now;
            var tank = new TankHelper();

            var Heightbinding = new Binding();
            Heightbinding.Path = new PropertyPath("TanksHeight");
            Heightbinding.Source = Application.Current.Resources;
            liquid.SetBinding(Canvas.TopProperty, Heightbinding);

            var TanksPercentbinding = new Binding();
            TanksPercentbinding.Path = new PropertyPath("TanksPercent");
            TanksPercentbinding.Source = Application.Current.Resources;
            liquid.SetBinding(Canvas.TopProperty, TanksPercentbinding);


        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await App.LUCTGPIOService.InitializeOxf();
            

        }
        

        private void Border_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if(count < 4)
            {
                var s = (Border)sender;
                var v = s.DataContext;
                password += v.ToString();
                if (count == 0)
                    PasswordStars.Text = "*";
                else
                    PasswordStars.Text += star;
                if(count == 3)
                {
                    var db = new AppDbContext();
                    MessageDialog d;
                    if (db.CheckPassword(password))
                    {
                        d = new MessageDialog("True");
                        HideLockScreen.Begin();
                        App.Authorized = true;
                        Frame.Navigate(typeof(HomePage));
                    }
                    else
                    {
                        d = new MessageDialog("False");
                        d.ShowAsync();
                    }
                    count = 0;
                    password = "";
                    PasswordStars.Text = "";
                    homeButton.IsTapEnabled = true;
                    return;
                }
                count++;
            }
        }
        
        private void Image_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            count = 0;
            password = "";
            PasswordStars.Text = "";
        }

        private void backButton_Click(object sender, TappedRoutedEventArgs e)
        {
            HideLockScreen.Begin();
            homeButton.IsTapEnabled = true;
            backButton.IsTapEnabled = false;
        }

        private async void homeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowLockScreen.Begin();
            homeButton.IsTapEnabled = false;
            backButton.IsTapEnabled = true;
            await I2cHelper.WriteRead(new Models.Slave() { I2cAddress = 8 }, I2cHelper.Mode.Mode1);
        }
        //private static async Task<BitmapImage> LoadImage(StorageFile file)
        //{
        //    BitmapImage bitmapImage = new BitmapImage();
        //    FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

        //    bitmapImage.SetSource(stream);

        //    return bitmapImage;

        //}
    }
}
