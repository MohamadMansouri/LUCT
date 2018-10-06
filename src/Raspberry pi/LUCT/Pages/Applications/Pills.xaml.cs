using LUCT.Helpers;
using LUCT.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Telerik.UI.Xaml.Controls.Input;
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
    public sealed partial class Pills : Page
    {
        AppDbContext _database;
        RelativePanel _configure, _manage,_taken;
        public List<string> Items { get; set; }
        public Pills()
        {
            _database = new AppDbContext();
            this.InitializeComponent();

            PillsData.ItemsSource =  _database.GetModel<LUCT.Models.Database.Pills>();

            LeftMenuControl.Tapped += LeftMenuControl_Tapped;
            Items = ApplicationHelper.GetLeftMenuItems(ApplicationType.Pills);
            LeftMenuControl.ItemsSource = Items;

            _configure = configure;
            _manage = manage;
            _taken = pillsTaken;

            calenderPicker.SelectionMode = Telerik.UI.Xaml.Controls.Input.CalendarSelectionMode.Multiple;

            parent.Children.Remove(_configure);
            parent.Children.Remove(_manage);
            parent.Children.Remove(_taken);
        }

        private void LeftMenuControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LeftMenuControl.SelectedItem != null)
            {
                switch (LeftMenuControl.SelectedItem.ToString())
                {
                    case "Configure":
                        if(parent.Children.Contains(_manage))
                            parent.Children.Remove(_manage);
                        if (parent.Children.Contains(_taken))
                            parent.Children.Remove(_taken);
                        parent.Children.Add(_configure);
                        break;
                    case "Manage":
                        if (parent.Children.Contains(_configure))
                            parent.Children.Remove(_configure);
                        if (parent.Children.Contains(_taken))
                            parent.Children.Remove(_taken);
                        parent.Children.Add(_manage);
                        PillsData.ItemsSource = _database.GetModel<LUCT.Models.Database.Pills>();
                        break;
                    case "Pills Taken":
                        if (parent.Children.Contains(_configure))
                            parent.Children.Remove(_configure);
                        if (parent.Children.Contains(_manage))
                            parent.Children.Remove(_manage);
                        pillsTakenData.ItemsSource = _database.GetModel<LUCT.Models.Database.PillsTaken>();
                        break;

                }
            }
        }

        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void Delete_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var b = (Button)sender;
            var id = Int32.Parse(b.DataContext.ToString());
            _database.DeleteModel<LUCT.Models.Database.Pills>(id);
            PillsData.ItemsSource = _database.GetModel<LUCT.Models.Database.Pills>();
        }
        private void Delete1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var b = (Button)sender;
            var id = Int32.Parse(b.DataContext.ToString());
            _database.DeleteModel<LUCT.Models.Database.PillsTaken>(id);
            PillsData.ItemsSource = _database.GetModel<LUCT.Models.Database.PillsTaken>();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.DataContext = sender;
            ParentedPopup.IsOpen = true;
        }

        private async void save_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (timePicker.Value != null && calenderPicker.SelectedDateRange != null)
            {
                foreach(var c in calenderPicker.SelectedDateRange.Value)
                {
                    _database.InsertModel<LUCT.Models.Database.Pills>(new Models.Database.Pills() { Name = "Pill", Time = c.AddDays(1) });
                }
                var cd = new ContentDialog()
                {
                    Title = "Saved",
                    PrimaryButtonText = "Saved",
                };
                await cd.ShowAsync();
            }
        }
    }
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            DateTime dt = DateTime.Parse(value.ToString());
            return dt.ToString("MM/dd/yyyy a\\t h:mm tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
