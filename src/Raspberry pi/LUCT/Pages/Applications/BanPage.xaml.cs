using LUCT.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LUCT.Pages.Applications
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BanPage : Page
    {
        AppDbContext database = new AppDbContext();
        List<Models.Database.Ban> ban = new List<Models.Database.Ban>();
        public BanPage()
        {
            this.InitializeComponent();
            ban = database.GetModel<Models.Database.Ban>();
        }
        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
        private void PhotoGrid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Populate photo grid with visitor ID photos:
            PopulatePhotoGrid();
        }
        private async void PopulatePhotoGrid()
        {
            // Sets max width to allow 6 photos to sit in one row
            var idImageMaxWidth = PhotoGrid.ActualWidth / 6 - 10;
            StorageFolder folder;
            try
            {
                folder = await KnownFolders.PicturesLibrary.GetFolderAsync("Banned");
            }
            catch(Exception ex)
            {
                folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("Banned");
            }



            var userIDImages = new StackPanel[ban.Count];

            for (int i = 0; i < ban.Count; i++)
            {
                var photo = await folder.GetFileAsync(ban[i].Image);
                var photoStream = await photo.OpenAsync(FileAccessMode.Read);
                BitmapImage idImage = new BitmapImage();
                await idImage.SetSourceAsync(photoStream);

                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Vertical;

                Image idImageControl = new Image();
                idImageControl.Source = idImage;
                idImageControl.MaxWidth = idImageMaxWidth;

                TextBlock time = new TextBlock();
                DateTime dt = DateTime.Parse(ban[i].Time.ToString());
                time.Text = dt.ToString("MM/dd/yyyy a\\t h:mm tt");

                panel.Children.Add(idImageControl);
                panel.Children.Add(time);
                userIDImages[i] = panel;
            }

            PhotoGrid.ItemsSource = userIDImages;
        }
    }

}
