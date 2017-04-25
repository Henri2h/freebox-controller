using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class CallPage : Page
    {
        public CallPage()
        {
            this.InitializeComponent();
            var _ = LoadCallsAsync();
        }

        async System.Threading.Tasks.Task LoadCallsAsync()
        {
            UICalls.Children.Clear();
            List<CodeShared.methods.CallEntry> calls = await AppCore.FreeboxController.call.GetCallsAsync();
            foreach (CodeShared.methods.CallEntry call in calls)
            {
                TextBlock tb = new TextBlock();
                tb.Text = call.name;
                UICalls.Children.Add(tb);
            }
        }

        private async void UIRefreshClickAsync(object sender, RoutedEventArgs e)
        {
            await LoadCallsAsync();
        }
    }
}
