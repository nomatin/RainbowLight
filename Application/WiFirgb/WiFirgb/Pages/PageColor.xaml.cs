using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using wifirgb.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WiFirgb.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageColor : ContentPage
    {
        private static int[] history = new int[3] { 0, 0, 0 };
        public PageColor()
        {
            InitializeComponent();
            System.Timers.Timer aTimer = new System.Timers.Timer(40);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += timerColor;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        public void timerColor(Object source, ElapsedEventArgs e)
        {
            int r = (int)(ColorStatic.SelectedColor.R * 255);
            int g = (int)(ColorStatic.SelectedColor.G * 255);
            int b = (int)(ColorStatic.SelectedColor.B * 255);
            if ((r != history[0]) || (g != history[1]) || (b != history[2]))
            {
                history[0] = r;
                history[1] = g;
                history[2] = b;
                SetSettingsWifi.setRGB(r, g, b);
            }


        }
    }
}