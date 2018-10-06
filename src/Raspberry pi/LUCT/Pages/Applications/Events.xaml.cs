using LUCT.Models.Database;
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
    public sealed partial class Events : Page
    {
        State state = State.None;
        Variable variable = Variable.None;
        Models.Database.Events ev = new Models.Database.Events();
        AppDbContext database = new AppDbContext();
        StackPanel eventPrep, eventTrig;
        ListView list;
        bool listShown = false;
        public Events()
        {
            this.InitializeComponent();

            eventsData.ItemsSource = database.GetModel<Models.Database.Events>();

            eventPrep = variablePanel;
            eventTrig = eventsPanel;

            list = eventsData;

            //parent.Children.Remove(eventPrep);
            //parent.Children.Remove(eventTrig);
            parent.Children.Remove(list);
        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCombo1 = combo.SelectedIndex;
            switch(selectedCombo1)
            {
                case 0:
                    variable = Variable.Temperature;
                    break;
                case 1:
                    variable = Variable.Humidity;
                    break;
                case 2:
                    variable = Variable.PowerConsumption;
                    break;
                case 3:
                    variable = Variable.TankLevel;
                    break;
            }
            ev.Variable = variable;
        }

        private void comboBA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCombo1 = comboBA.SelectedIndex;
            switch (selectedCombo1)
            {
                case 0:
                    state = State.Below;
                    break;
                case 1:
                    state = State.Above;
                    break;
            }
            ev.State = state;
        }

        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!listShown)
            {
                parent.Children.Remove(eventPrep);
                parent.Children.Remove(eventTrig);
                parent.Children.Add(list);
                eventsData.ItemsSource = database.GetModel<Models.Database.Events>();
                ((Button)sender).Content = "Create Event";
                listShown = true;
            }
            else
            {
                parent.Children.Add(eventPrep);
                parent.Children.Add(eventTrig);
                parent.Children.Remove(list);
                ((Button)sender).Content = "Show Saved Events";
                listShown = false;
            }
        }

        private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            int id = Int32.Parse(((Button)sender).DataContext.ToString());
            database.DeleteModel<Models.Database.Events>(id);
            eventsData.ItemsSource = database.GetModel<Models.Database.Events>();
        }

        private void banSubmit_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(state == State.None || variable == Variable.None)
            {
                status.Text = "An error occured check your input";
            }
            else
            {
                if (!t0.IsOn && !t1.IsOn && !t2.IsOn && !t3.IsOn)
                    status.Text = "You haven't chosen any events";
                else
                {
                    if (t0.IsOn)
                        ev.IsLightsOnEnabled = true;
                    if (t1.IsOn)
                        ev.IsConditionerOnEnabled = true;
                    if (t2.IsOn)
                        ev.IsPipeOnEnabled = true;
                    if (t3.IsOn)
                        ev.IsShutdownEnabled = true;
                    try
                    {
                        ev.Value = float.Parse(textData.Text);
                        database.InsertModel<Models.Database.Events>(ev);
                        status.Text = "Event added successfully";
                    }
                    catch(Exception ex)
                    {
                        status.Text = "An error occured check your input";
                    }
                }
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Keyboard.DataContext = sender;
            ParentedPopup.IsOpen = true;
        }
        private void backPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
