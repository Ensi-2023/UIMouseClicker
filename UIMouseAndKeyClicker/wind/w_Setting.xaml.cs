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

            var obj =  Settings.Default["Sound"];

            if ((bool)obj)
            {
                v1.IsChecked = true;
            } else
            { 
                v2.IsChecked = true;
            }

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
                case "1": Save(true); break;
                case "2": Save(false); break;
            }
        }

        private void Save(bool value)
        {
            Settings.Default["Sound"] = value;
            Settings.Default.Save();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
