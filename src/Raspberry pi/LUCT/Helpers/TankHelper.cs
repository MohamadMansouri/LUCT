using LUCT.Models.Database;
using LUCT.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace LUCT.Helpers
{
    public class TankHelper
    {
        double _containerHeight, _liquidHeight;
        public TankHelper()
        {
            _liquidHeight = GetHeight();
            _containerHeight = new AppDbContext().GetFirstModel<TankLevel>().Height ;
        }

        public TankVars ChangeLevel(float liquidPercent)
        {
            var tv = new TankVars();
            var oldHeight = _liquidHeight;
            _liquidHeight = liquidPercent * _containerHeight;
            var difference = oldHeight - _liquidHeight;
            tv.height = _liquidHeight;
            tv.difference = difference;

            return tv;
        }

      public double GetHeight()
        {
            return 0;
        }

        public struct TankVars
        {
            public double height;
            public double difference;
        }
    }
}
