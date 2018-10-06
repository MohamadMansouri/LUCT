using LUCT.Models;
using System;
using System.Collections.Generic;
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
    public sealed partial class CommonAppPage : UserControl
    {
        public List<LeftMenuItem> Items { get; set; }
        public RadListView LeftMenu { get; set; }
        public CommonAppPage()
        {
            this.InitializeComponent();
            LeftMenu = leftMenuListControl;
        }
    }
}
