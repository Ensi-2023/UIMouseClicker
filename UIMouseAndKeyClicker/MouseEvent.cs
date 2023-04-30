using System;
using System.Runtime.InteropServices;


namespace UIMouseAndKeyClicker
{
    public class MouseEvent
    {

        public const UInt32 MOVE = 0x0001;
        public const UInt32 LEFTDOWN = 0x0002;
        public const UInt32 LEFTUP = 0x0004;
        public const UInt32 RIGHTDOWN = 0x0008;
        public const UInt32 RIGHTUP = 0x0010;
        public const UInt32 MIDDLEDOWN = 0x0020;
        public const UInt32 MIDDLEUP = 0x0040;
        public const UInt32 WHEEL = 0x0800;
        public const UInt32 ABSOLUTE = 0x8000;

        [DllImport("user32.dll")] static public extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);


        public void Click()
        {
            mouse_event(LEFTDOWN | LEFTUP, 0, 0, 0, IntPtr.Zero);
        }

    }
}
