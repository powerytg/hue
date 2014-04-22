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
using Windows.UI.Popups;
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
    public sealed partial class BridgePage : Page
    {
        public BridgePage()
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
            // Search for bridges
            RefreshBridgesAsync();
        }

        private async void RefreshBridgesAsync()
        {
            List<Bridge> foundBridges = await new BridgeFinder().SearchBridgesAsync();

            // Check if the stored bridge is still valid
            if (foundBridges.Count == 0)
            {
                MessageDialog dialog = new MessageDialog("No bridges have been found. Please retry.");
                dialog.Commands.Add(new UICommand("Search Again", OnRetryButtonClicked));
                await dialog.ShowAsync();
                return;
            }
            else if(foundBridges.Count > 1)
            {
                // Multiple bridges found. Need to choose one
                ShowBridgeChooserPage(foundBridges);
                return;
            }
            else
            {
                // Try to connect to bridge
                TestConnectionAsync(foundBridges[0]);
            }
        }

        private void OnRetryButtonClicked(IUICommand command)
        {
            RefreshBridgesAsync();
        }

        private async void TestConnectionAsync(Bridge bridge)
        {
            BridgeManager.Instance.CurrentBridge = bridge;

            // Verify bridge
            HueAPI.Instance.BridgeIp = bridge.IPAddress;            

            bool valid = await HueAPI.Instance.TestConnectAsync();
            Debug.WriteLine(valid);

            if (!valid)
            {
                this.Frame.Navigate(typeof(BridgeConnectPage));
            }
            else
            {
                ShowMainPage();
            }
        }

        private void ShowBridgeChooserPage(List<Bridge> bridges)
        {
            // Wait for a little while and remove this page
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += (sender, e) =>
            {
                timer.Stop();

                // Go to main page
                this.Frame.Navigate(typeof(BridgeChooserPage), bridges);
                if (this.Frame.CanGoBack)
                {
                    this.Frame.BackStack.RemoveAt(0);
                }
            };

            timer.Start();

        }

        private void ShowMainPage()
        {
            // Wait for a little while and remove this page
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += (sender, e) =>
            {
                timer.Stop();

                // Go to main page
                this.Frame.Navigate(typeof(MainPage));
                if (this.Frame.CanGoBack)
                {
                    this.Frame.BackStack.RemoveAt(0);
                }
            };

            timer.Start();

        }
    }
}
