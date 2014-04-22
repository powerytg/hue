using Hue.API.Hue;
using HueSaturation.API.Hue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Hue.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BridgeChooserPage : Page
    {
        private List<Bridge> bridges;

        public BridgeChooserPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            bridges = e.Parameter as List<Bridge>;
            BridgeListView.ItemsSource = bridges;
        }

        private void BridgeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Connect to the selected bridge
            var bridge = BridgeListView.SelectedItem as Bridge;
            BridgeManager.Instance.CurrentBridge = bridge;

            // Verify bridge
            HueAPI.Instance.BridgeIp = bridge.IPAddress;
            TestConnectionAsync();
        }

        private async void TestConnectionAsync()
        {
            bool valid = await HueAPI.Instance.TestConnectAsync();
            Debug.WriteLine(valid);

            if (!valid)
            {
                this.Frame.Navigate(typeof(BridgeConnectPage));
            }
        }
    }
}
