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
    public partial class PageSettings : ContentPage
    {
        public PageSettings()
        {
            InitializeComponent();
        }

        private void Ap_Toggled(object sender, ToggledEventArgs e)
        {
            WiFi_Pass.IsEnabled = !Ap.IsToggled;
            WiFi_Pass.Text = "";
        }

        private async void Send_Clicked(object sender, EventArgs e)
        {
            
            if (WiFi_SSID.Text == null || WiFi_SSID.Text == "Введие название сети")
            {
                WiFi_SSID.TextColor = Color.LightPink;
                WiFi_SSID.Text = "Введие название сети";
            }
            else if ((WiFi_Pass.Text == null || WiFi_Pass.Text == "Введие пароль") && WiFi_Pass.IsEnabled )
            {
                WiFi_Pass.TextColor = Color.LightPink;
                WiFi_Pass.Text = "Введие пароль";
            }
            else
            {
                if(await DisplayAlert("Подтвердить действие", "Отправить настройки?", "Ок", "Отмена"))
                {
                    //отправка потом дописсать!!!!!
                    Console.WriteLine("Отправка");
                    if (Ap.IsToggled)
                    {
                        SetSettingsWifi.setAP(WiFi_SSID.Text);
                    }
                    else
                    {
                        SetSettingsWifi.setWiFi(WiFi_SSID.Text, WiFi_Pass.Text);
                    }
                }
                
                
            }
        }

        private void WiFi_Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(e.NewTextValue != "Введие название сети" || e.NewTextValue != "Введие пароль")
            {
                WiFi_Pass.TextColor = Color.White;
                WiFi_SSID.TextColor = Color.White;
            }
            
        }

        
    }
}