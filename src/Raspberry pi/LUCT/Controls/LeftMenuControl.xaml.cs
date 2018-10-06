using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Telerik.UI.Xaml.Controls.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LUCT.Controls
{
    public sealed partial class LeftMenuControl : UserControl
    {
        public List<string> Items { get; set; }
        public object ItemsSource
        {
            get { return (object)GetValue(RadListView.ItemsSourceProperty); }
            set { SetValue(RadListView.ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemSource =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(LeftMenuControl), new PropertyMetadata(""));

        public object SelectedItem
        {
            get { return (object)GetValue(RadListView.SelectedItemProperty); }
            set { SetValue(RadListView.SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(LeftMenuControl),new PropertyMetadata(""));

        public LeftMenuControl()
        {
            this.InitializeComponent();
            DataContext = this;
        }
    }
}
