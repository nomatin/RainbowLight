using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using wifirgb.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace WiFirgb.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageStrobe : ContentPage
    {
        private static int[] history = new int[4] { 0, 0, 0, 0 };
        public PageStrobe()
        {
            
            InitializeComponent();
            history[0] = (int)(ColorStrob.SelectedColor.R * 255);
            history[1] = (int)(ColorStrob.SelectedColor.G * 255);
            history[2] = (int)(ColorStrob.SelectedColor.B * 255);
            history[3] = (int)(Speed.Value);
            Timer aTimer = new Timer(33);
            aTimer.Elapsed += timerColor;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private void timerColor(Object source, ElapsedEventArgs e)
        {
            int r = (int)(ColorStrob.SelectedColor.R * 255);
            int g = (int)(ColorStrob.SelectedColor.G * 255);
            int b = (int)(ColorStrob.SelectedColor.B * 255);
            int speed = (int)Speed.Value;
            if ((r != history[0]) || (g != history[1]) || (b != history[2]) || (speed != history[3]))
            {
                history[0] = r;
                history[1] = g;
                history[2] = b;
                history[3] = speed;
                System.Console.WriteLine(Speed);
                SetSettingsWifi.setRGB(r, g, b);
                SetSettingsWifi.setStrobe(speed);

            }


        }
        private void Speed_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SpeedText.Text = "Скорость:" + Speed.Value.ToString();
        }
    }
}