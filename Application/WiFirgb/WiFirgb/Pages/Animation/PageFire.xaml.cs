using Refractored.FabControl;
using System;
using System.Collections;
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
    public partial class PageFire : ContentPage
    {
        /*
        private readonly FloatingActionButtonView fab;
        private readonly ListView list;
        private int appearingListItemIndex = 0;
        
        public PageFire()
        {
            Title = "Fab Sample XF";
            BackgroundColor = Color.White;

            var data = new List<string>();
            for (var i = 1; i <= 100; i++)
            {
                data.Add(i.ToString());
            }

            list = new ListView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemsSource = data,
            };

            fab = new FloatingActionButtonView()
            {
                ImageName = "ic_add.png",
                ColorNormal = Color.FromHex("ff3498db"),
                ColorPressed = Color.Black,
                ColorRipple = Color.FromHex("ff3498db"),
                Clicked = async (sender, args) =>
                {
                    var animate = await this.DisplayAlert("Fab", "Hide and show the Fab?", "Sure", "Not now");
                    if (!animate) return;

                    fab.Hide();
                    await Task.Delay(1500);
                    fab.Show();
                },
            };

            // Main page layout
            var pageLayout = new StackLayout
            {
                Children =
                {
                    list
                }
            };

            var absolute = new AbsoluteLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Position the pageLayout to fill the entire screen.
            // Manage positioning of child elements on the page by editing the pageLayout.
            AbsoluteLayout.SetLayoutFlags(pageLayout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(pageLayout, new Rectangle(0f, 0f, 1f, 1f));
            absolute.Children.Add(pageLayout);

            // Overlay the FAB in the bottom-right corner
            AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absolute.Children.Add(fab);

            Content = absolute;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            list.ItemAppearing += List_ItemAppearing;
            list.ItemDisappearing += List_ItemDisappearing;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            list.ItemAppearing -= List_ItemAppearing;
            list.ItemDisappearing -= List_ItemDisappearing;
        }

        async void List_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            await Task.Run(() =>
            {
                var items = list.ItemsSource as IList;
                if (items != null)
                {
                    var index = items.IndexOf(e.Item);
                    if (index < appearingListItemIndex)
                    {
                        Device.BeginInvokeOnMainThread(() => fab.Hide());
                    }
                    appearingListItemIndex = index;
                }
            });
        }

        async void List_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await Task.Run(() =>
            {
                var items = list.ItemsSource as IList;
                if (items != null)
                {
                    var index = items.IndexOf(e.Item);
                    if (index < appearingListItemIndex)
                    {
                        Device.BeginInvokeOnMainThread(() => fab.Show());
                    }
                    appearingListItemIndex = index;
                }
            });
        }
        */

        private static int[] history = new int[4] { 0, 0, 0, 0};
        public PageFire()
        {
            
            InitializeComponent();
            history[0] = (int)(ColorFire.SelectedColor.Hue * 255);
            history[1] = (int)(ColorFire.SelectedColor.Luminosity * 255);
            history[2] = (int)(Speed.Value);
            history[3] = (int)(Ratio.Value * 1000);
            Timer aTimer = new Timer(33);
            aTimer.Elapsed += timerColor;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        public void timerColor(Object source, ElapsedEventArgs e)
        {

            int h = (int)(ColorFire.SelectedColor.Hue * 255);
            int l = (int)(ColorFire.SelectedColor.Luminosity * 255);
            int speed = (int)Speed.Value;
            int ratio = (int)(Ratio.Value * 100);
            if ((h != history[0]) || (l != history[1]) || (speed != history[2]) || (ratio != history[3]))
            {
                history[0] = h;
                history[1] = l;
                history[2] = speed;
                history[3] = ratio;
                System.Console.WriteLine(h);
                System.Console.WriteLine("----------");
                System.Console.WriteLine(l);
                System.Console.WriteLine("----------");
                SetSettingsWifi.setFire(h, speed, ratio);
                SetSettingsWifi.setBrightness(l);
            }


        }
        
        private void Speed_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            SpeedText.Text = "Скорость:" + ((int)Speed.Value).ToString();
        }

        private void Ratio_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            RatioText.Text = "Коэфициент:" + Ratio.Value.ToString();
        }
        
    }
}