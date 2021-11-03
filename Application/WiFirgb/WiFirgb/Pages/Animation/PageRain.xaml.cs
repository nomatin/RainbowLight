using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wifirgb.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WiFirgb.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageRain : ContentPage
    {
        public PageRain()
        {
            InitializeComponent();
            pickerRain.SelectedIndex = 0;
        }

        private void Speed_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SpeedText.Text = "Скорость:" + ((int)Speed.Value).ToString();
            SetSettingsWifi.setRain(pickerRain.SelectedIndex + 2, (int)Speed.Value);
            SetSettingsWifi.setBrightness((int)Brightness.Value);
        }

        private void Brightness_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            BrightnessText.Text = "Яркость:" + ((int)Brightness.Value).ToString();
            SetSettingsWifi.setRain(pickerRain.SelectedIndex + 2, (int)Speed.Value);
            SetSettingsWifi.setBrightness((int)Brightness.Value);
        }

        private void pickerRain_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SetSettingsWifi.setRain(pickerRain.SelectedIndex + 2, (int)Speed.Value);
            SetSettingsWifi.setBrightness((int)Brightness.Value);
        }
    }
}