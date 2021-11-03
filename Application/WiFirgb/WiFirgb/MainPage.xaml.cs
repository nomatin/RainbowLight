using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using wifirgb.Models;
using Xamarin.Forms;

namespace WiFirgb
{
    public partial class MainPage : Shell
    {
        delegate void Message(string text);
        public MainPage()
        {
            InitializeComponent();
            back.BackgroundColor = Color.FromHex("#ff618b");
            Message message = mes;
            SetSettingsWifi.StartSearch(message);
        }
        protected override bool OnBackButtonPressed()
        {
            Console.WriteLine("назад");
            Shell.Current.GoToAsync("//fire");
            return true;
        }
        private void mes(string text)
        {
            back.BackgroundColor = Color.PowderBlue;
            Console.WriteLine(text);
        }
    }
}
