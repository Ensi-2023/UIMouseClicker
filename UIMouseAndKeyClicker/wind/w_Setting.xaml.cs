using System;
using System.Collections.Generic;
using System.Linq;
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
using UIMouseAndKeyClicker.Properties;

namespace UIMouseAndKeyClicker.wind
{
    /// <summary>
    /// Логика взаимодействия для w_Setting.xaml
    /// </summary>
    public partial class w_Setting : Window
    {
        public w_Setting()
        {
            InitializeComponent();


            var KeyActive = (Key)int.Parse(Settings.Default["KeyActive"].ToString());
            keyActive.Text = KeyActive.ToString();

            var obj =  Settings.Default["Sound"];
            var obj2 =  Settings.Default["SingleClick"];
            var obj3 =  Settings.Default["IntexMouseButton"];
            var obj4 =  Settings.Default["MouseMove"];
            var obj5 =  Settings.Default["MinimizeStart"];
            var obj6 =  Settings.Default["SpeedCursor"];
            var obj7 =  Settings.Default["UpdateThread"];


            if (obj6.ToString().Length == 0)
            {
                Settings.Default["SpeedCursor"] = "4";
                Settings.Default.Save();
                obj6 = "4";
            }


            if (obj7.ToString().Length == 0)
            {
                Settings.Default["UpdateThread"] = "80";
                Settings.Default.Save();
                obj7 = "80";
            }


            speedCursor.Text = obj6.ToString();
            updateThread.Text= obj7.ToString();

            if ((bool)obj)
            {
                v1.IsChecked = true;
            } else
            { 
                v2.IsChecked = true;
            }

                if ((bool)obj5)
            {
                v7.IsChecked = true;
            } else
            { 
                v8.IsChecked = true;
            }


            if ((bool)obj4)
            {
                v5.IsChecked = true;
            }
            else
            {
                v6.IsChecked = true;
            }


            if ((bool)obj2)
            {
                v3.IsChecked = true;
            }
            else
            {
                v4.IsChecked = true;
            }

            combo.SelectedIndex = (int)obj3 - 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var obj = (RadioButton)sender;

            switch (obj.Uid)
            {
                case "1": SaveSound(true); break;
                case "2": SaveSound(false); break;
                case "3": SaveSingleClick(true); break;
                case "4": SaveSingleClick(false); break;  
                case "5": SaveMouseMove(true); mouseMoveSetting.Visibility = Visibility.Visible; break;
                case "6": SaveMouseMove(false); mouseMoveSetting.Visibility = Visibility.Collapsed; break;
                case "7": SaveMinimizeStart(true); break;
                case "8": SaveMinimizeStart(false); break;
            }
        }

        private void SaveSound(bool value)
        {
            Settings.Default["Sound"] = value;
            Settings.Default.Save();
        }   
        private void SaveMinimizeStart(bool value)
        {
            Settings.Default["MinimizeStart"] = value;
            Settings.Default.Save();
        }       
        private void SaveMouseMove(bool value)
        {
            Settings.Default["MouseMove"] = value;
            Settings.Default.Save();
        } 
        private void SaveSingleClick(bool value)
        {
            Settings.Default["SingleClick"] = value;
            Settings.Default.Save();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var obj = sender as ComboBox;

            switch(obj.SelectedIndex)
            {
                case 0: Settings.Default["IntexMouseButton"] = 1; break;
                case 1: Settings.Default["IntexMouseButton"] = 2; break;
                case 2: Settings.Default["IntexMouseButton"] = 3; break;

            }

            Settings.Default.Save();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape) Close();
        }

        private void switchButton_Click(object sender, RoutedEventArgs e)
        {
            var wind = new w_SwitchKey();
            wind.ShowDialog();

            var KeyActive = (Key)int.Parse(Settings.Default["KeyActive"].ToString());
            keyActive.Text = KeyActive.ToString();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var obj = (TextBox)sender;

            if (!((e.Key.GetHashCode() >= 34) && (e.Key.GetHashCode() <= 43)) && !((e.Key.GetHashCode() >= 74) && (e.Key.GetHashCode() <= 83)) && !(e.Key.GetHashCode() == 3))
            {
                e.Handled = true;
            }
    
        }

        private void speedCursor_TextChanged(object sender, TextChangedEventArgs e)
        {
            var obj = (TextBox)sender;
            Settings.Default["SpeedCursor"] = obj.Text;
            Settings.Default.Save();
        }

        private void updateThread_TextChanged(object sender, TextChangedEventArgs e)
        {
            var obj = (TextBox)sender;
            Settings.Default["UpdateThread"] = obj.Text;
            Settings.Default.Save();

        }
    }
}
