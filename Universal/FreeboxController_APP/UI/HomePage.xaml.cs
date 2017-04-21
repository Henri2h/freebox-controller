using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FreeboxController_APP.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }
        public async Task LoadElementsAsync()
        {
            Task<bool> WifiInfos = AppCore.FreeboxController.wifi.getWifiInfoAsync();

            bool wifiInfos = await WifiInfos;
            if (wifiInfos) { UIWifi.Content = "Disable"; }
            else { UIWifi.Content = "Enable"; }

        }

        private async Task UIWifi_ClickAsync(object sender, RoutedEventArgs e)
        {
            bool wifiEnabled = false;
            if (UIWifi.Content.ToString() == "Disable")
            {
                wifiEnabled = true;
            }

            await AppCore.FreeboxController.wifi.setWifiAsync(wifiEnabled);
        }
    }
}
