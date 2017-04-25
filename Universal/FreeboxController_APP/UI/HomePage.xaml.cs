using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
        bool wifiEnabled = false;

        public HomePage()
        {
            this.InitializeComponent();
            LoadElementsAsync();
        }
        public async void LoadElementsAsync()
        {
            Task<bool> WifiInfos = AppCore.FreeboxController.wifi.GetWifiInfoAsync();

            this.wifiEnabled = await WifiInfos;

            if (this.wifiEnabled) { UIWifi.Content = "Disable"; }
            else { UIWifi.Content = "Enable"; }

        }

        private async void UIWifi_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                bool result = await AppCore.FreeboxController.wifi.SetWifiAsync(this.wifiEnabled);
                LoadElementsAsync();
            }
            catch
            {
                var dialog = new MessageDialog("Could not turn on the wifi. Verify permissions");
                await dialog.ShowAsync();
            }
        }

        private void UIRefresh(object sender, RoutedEventArgs e)
        {
            LoadElementsAsync();
        }
    }
}
