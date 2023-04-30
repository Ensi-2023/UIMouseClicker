using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace UIMouseAndKeyClicker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            string appName = AppDomain.CurrentDomain.FriendlyName;
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application
                MessageBox.Show("Приложение уже запущено","Ошибка",MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            base.OnStartup(e);
        }
    }
}
