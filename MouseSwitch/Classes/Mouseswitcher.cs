using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wpfbgtest1.Hotkeys
{
    public class Mouseswitcher
    {
        public const UInt32 SPI_SETMOUSESPEED = 0x0071;
        [DllImport("user32.dll")]
        public static extern Boolean SystemParametersInfo(
            UInt32 uiAction,
            UInt32 uiParam,
            UInt32 pvParam,
            UInt32 fWinIni);
    }
}
