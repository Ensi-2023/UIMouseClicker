using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace UIMouseAndKeyClicker.wind
{
    /// <summary>
    /// Логика взаимодействия для w_SwitchKey.xaml
    /// </summary>
    public partial class w_SwitchKey : Window
    {
        
        static w_SwitchKey Instance;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;


        private Key SetPressed;
        private Key SetNameKey;

        public w_SwitchKey()
        {
            InitializeComponent();
            Instance = this;
           _hookID = SetHook(_proc);
            PreviewKeyDown += W_SwitchKey_PreviewKeyDown;    
        }

        private void W_SwitchKey_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            SetNameKey = e.Key;
            Set();
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
                Instance.SetPressed = (Key)vkCode;
                         
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private void Set()
        {
            if (SetNameKey == Key.Escape)
            {
                newkey.Text = "Ожидаю нажатия...";
                saveButton.Visibility = Visibility.Collapsed;
                return;
            }

            newkey.Text = $"Нажата кнопка: {SetNameKey}";
            saveButton.Visibility = Visibility.Visible;

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

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            PreviewKeyDown -= W_SwitchKey_PreviewKeyDown;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.SetKey(SetNameKey, SetPressed);
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
