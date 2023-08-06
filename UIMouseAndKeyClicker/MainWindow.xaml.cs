using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIMouseAndKeyClicker.wind;
using static System.Net.Mime.MediaTypeNames;
using UIMouseAndKeyClicker.Properties;
using System.Xml.Linq;
using System.Globalization;
using static System.Windows.Forms.DataFormats;
using System.Threading;
using static UIMouseAndKeyClicker.MouseEvent;
using System.Windows.Forms;
using System.Reflection.Metadata;

namespace UIMouseAndKeyClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        bool isSettingOpen = false;

        private Key KeyActive = Key.None;
        private Key KeyPressed = Key.None;
 
        static public MainWindow Instance { get { return _instance; } }
        static private MainWindow _instance { set; get; }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private bool IsStart;
        private MouseEvent _mouseEvent = new MouseEvent();
        public MainWindow()
        {
            InitializeComponent();
            _instance = this;              

            
        }

        public void SetKey(Key _keyActive, Key _keyPressed)
        {
            KeyActive = _keyActive;
            KeyPressed = _keyPressed;

            keyActive.Text = KeyActive.ToString();
            Settings.Default["KeyActive"] = (int)KeyActive;
            Settings.Default["KeyPressed"] = (int)KeyPressed;
            Settings.Default.Save();

            taskIcon.ToolTipText = $"Кнопка активации: {KeyActive}";         
            taskIcon.ShowBalloonTip("", $"Кнопка активации: {KeyActive}",Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
           
        }
        private void Tray()
        {
            if ((bool)Settings.Default["MinimizeStart"]==false)
            {
               return;
            }
            
            this.Hide();
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);
        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {


                int vkCode = Marshal.ReadInt32(lParam);
                if ((Key)vkCode == Instance.KeyPressed)
                {
                    Instance.StartOrStopHook();
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void StartOrStopHook()
        {
            if (isSettingOpen) return;
            var obj = Settings.Default["Sound"];
            var obj2 = Settings.Default["SingleClick"];
            var obj3 = Settings.Default["MouseMove"];
         
            var obj4 = Settings.Default["SpeedCursor"].ToString().Length>0?int.Parse(Settings.Default["SpeedCursor"].ToString()):5;
            var obj5 = Settings.Default["UpdateThread"].ToString().Length>0?int.Parse(Settings.Default["UpdateThread"].ToString()):80;
            taskIcon.HideBalloonTip();

            if (IsStart)
            {
         
                status.Text = "Не активен";
                status.Foreground = Brushes.Red;
                switchButton.IsEnabled= true;
                Stop();

                if((bool)obj) System.Media.SystemSounds.Hand.Play();


                return;
            }
            else
            {
                status.Text = "Активен";
         
                status.Foreground = Brushes.LimeGreen;
                switchButton.IsEnabled= false;
                Start((bool)obj2,(bool)obj3, obj4, obj5);
                if ((bool)obj) System.Media.SystemSounds.Exclamation.Play();
                return;
            }

        }

        public ButtomEnum ReturnButtom()
        {

            var obj = Settings.Default["IntexMouseButton"];
            switch ((int)obj) 
            {
                case 1: return ButtomEnum.left;
                case 2: return ButtomEnum.right;
                case 3: return ButtomEnum.middle;
                    
            }

            return ButtomEnum.left;
           
        }

   


        private void Start(bool issingle = false,bool mousemove=false,int speedCursor=4,int updateThread = 80)
        {

            IsStart = true;
            Task.Factory.StartNew(async () => 
            {
                while (IsStart)
                {
                    Random rnd = new Random();
                    _mouseEvent.Click(issingle, ReturnButtom());
                    if(mousemove) _mouseEvent.Move(speedCursor, updateThread);
                    await Task.Delay(rnd.Next(50,250));
                }
            });
        }

        private void Stop()
       { 
            IsStart = false;
            _mouseEvent.Stop();
        }


        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            Environment.Exit(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            this.Hide();
        }

        private void taskIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            this.Show();
            WindowState = WindowState.Normal;
            Topmost= true;
            Topmost = false;
        }

        private void switchButton_Click(object sender, RoutedEventArgs e)
        {
            isSettingOpen = true;
            var wind = new w_SwitchKey();
            wind.ShowDialog();
            isSettingOpen =false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KeyActive = (Key)int.Parse(Settings.Default["KeyActive"].ToString());
            KeyPressed = (Key)int.Parse(Settings.Default["KeyPressed"].ToString());

            if (KeyActive == Key.None)
            {
                KeyActive = Key.F6;
                Settings.Default["KeyActive"] = (int)KeyActive;
                Settings.Default.Save();

                keyActive.Text = KeyActive.ToString();
            }

            if (KeyPressed == Key.None)
            {
                KeyPressed = Key.RightShift;
                Settings.Default["KeyPressed"] = (int)KeyPressed;
                Settings.Default.Save();
            }

            SetKey(KeyActive, KeyPressed);

            Tray();

            _mouseEvent = new MouseEvent();


            _hookID = SetHook(_proc);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var wind = new w_Setting();
            wind.ShowDialog();


        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {

                taskIcon.ShowBalloonTip("", $"Я тут!", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
                WindowState = WindowState.Minimized;
                this.Hide();
            }
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
    LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

       
    }
}
