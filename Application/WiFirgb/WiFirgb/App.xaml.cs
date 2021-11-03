using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using wifirgb.Models;

namespace WiFirgb
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            SetSettingsWifi.SetIp("226.1.1.1");
            SetSettingsWifi.SetPort(4096);
            SetSettingsWifi.name=100;
            SetSettingsWifi.group = 100;
        }

        protected override void OnSleep()
        {
            SetSettingsWifi.closeServer();
        }

        protected override void OnResume()
        {
            if (SetSettingsWifi.startServer())
            {
                Console.WriteLine("-----------------");
                System.Console.WriteLine("Сервер запущен");
                Console.WriteLine("-----------------");
            }
            else
            {
                Console.WriteLine("-----------------");
                System.Console.WriteLine("Ошибка запуска");
                Console.WriteLine("-----------------");
            }
        }
    }
}
