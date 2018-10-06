﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace LUCT.TouchPanels.Devices
{
    public interface ITouchDevice
    {
        int Pressure { get; }
        Point TouchPosition { get; }
        void ReadTouchpoints();
        Point RawTouchPosition { get; }
    }
}
