using System;
using System.Drawing;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;


namespace UIMouseAndKeyClicker
{
    public class MouseEvent
    {
        public enum ButtomEnum
        {
            left, right, middle
        }


        public const UInt32 MOVE = 0x0001;
        public const UInt32 LEFTDOWN = 0x0002;
        public const UInt32 LEFTUP = 0x0004;
        public const UInt32 RIGHTDOWN = 0x0008;
        public const UInt32 RIGHTUP = 0x0010;
        public const UInt32 MIDDLEDOWN = 0x0020;
        public const UInt32 MIDDLEUP = 0x0040;
        public const UInt32 WHEEL = 0x0800;
        public const UInt32 ABSOLUTE = 0x8000;

        [DllImport("user32.dll")] static public extern void mouse_event(UInt32 dwFlags, int dx, int dy, UInt32 dwData, IntPtr dwExtraInfo);

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int X, int Y)
            {
                x = X;
                y = Y;
            }
        }

        public bool IsMoveStop { get; private set; }

        bool IsStartingSingle = false;

        public void Click(bool single = false, ButtomEnum button = ButtomEnum.left)
        {

            if (IsStartingSingle == true) return;

            if (single && IsStartingSingle == false)
            {
                switch (button)
                {
                    case ButtomEnum.left: mouse_event(LEFTDOWN, 0, 0, 0, IntPtr.Zero); break;
                    case ButtomEnum.right: mouse_event(RIGHTDOWN, 0, 0, 0, IntPtr.Zero); break;
                    case ButtomEnum.middle: mouse_event(MIDDLEDOWN, 0, 0, 0, IntPtr.Zero); break;
                }
                IsStartingSingle = true;
                return;
            }

            switch (button) {
                case ButtomEnum.left: mouse_event(LEFTDOWN | LEFTUP, 0, 0, 0, IntPtr.Zero); break;
                case ButtomEnum.right: mouse_event(RIGHTDOWN | RIGHTUP, 0, 0, 0, IntPtr.Zero); break;
                case ButtomEnum.middle: mouse_event(MIDDLEDOWN | MIDDLEUP, 0, 0, 0, IntPtr.Zero); break;
            }


        }

        internal void Stop()
        {
            mouse_event(LEFTUP, 0, 0, 0, IntPtr.Zero);
            mouse_event(RIGHTUP, 0, 0, 0, IntPtr.Zero);
            mouse_event(MIDDLEUP, 0, 0, 0, IntPtr.Zero);
            IsMoveStop= true;
            IsStartingSingle = false;
            DX = 0;
            DY = 0;
        }

        double DX = 0, DY = 0, dx, dy;
        bool isMove = false;
        Random r = new Random();
        internal async void Move(int speedCursor,int updateThread)
        {
         
            if (isMove == false)
            {
          
                 dx = (int)r.Next(-speedCursor, speedCursor);
                 dy = (int)r.Next(-speedCursor, speedCursor);

                IsMoveStop = false;
            }

            while (IsMoveStop==false)
            {
                isMove = true;

                if (dx >= 0 && DX!=dx) DX=DX+0.5;
                if (dy >= 0 && DY!=dy) DY=DY + 0.5;

                if (dx <= 0 && DX != dx) DX = DX - 0.5;
                if (dy <= 0 && DY != dy) DY = DY - 0.5;

                if (dx == DX && DY == dy) break;

                mouse_event(MOVE, (int)DX, (int)DY, 0, IntPtr.Zero);

                await Task.Delay(r.Next(updateThread-r.Next(0,15), updateThread+r.Next(0,20)));
            }

            isMove = false;

            DX = 0;
            DY = 0;

        }
        
    }
}
