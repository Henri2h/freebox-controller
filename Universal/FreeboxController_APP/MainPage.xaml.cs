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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FreeboxController_APP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            StartAsync();
        }

        async void StartAsync()
        {
            try
            {
                Task t = AppCore.StartAsync();
                await t;


                System.Diagnostics.Debug.WriteLine("Going to navigate");
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(UI.HomePage));
            }
            catch
            {
                var dialog = new MessageDialog("Could not connect");
                await dialog.ShowAsync();
            }
        }

        private void UIBtText_Click(object sender, RoutedEventArgs e)
        {
            StartAsync();
        }
    }
}
