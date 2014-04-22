using Hue.API.Hue;
using System;
using System.Collections.Generic;
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
    public sealed partial class BridgeConnectPage : Page
    {
        private DispatcherTimer timer;

        public BridgeConnectPage()
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
            BridgeAnimation.Begin();
            TryRegisterUser();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if(timer != null)
            {
                timer.Stop();
                timer = null;
            }

            base.OnNavigatedFrom(e);
        }

        private void TryRegisterUser()
        {
            if (timer != null)
            {
                return;
            }

            TimeView.Value = 30;

            // Start timer
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += (sender, evt) =>
            {
                int value = (int)TimeView.Value;
                value = Math.Max(0, value - 1);
                TimeView.Value = value;

                if (value == 0 && timer != null)
                {
                    timer.Stop();
                    timer = null;
                    ShowTimeoutDialogAsync();
                }
                else
                {
                    // Try to call the registration API every several seconds
                    if (value % 3 == 0)
                    {
                        TryRegisterUserAsync();
                    }
                }
            };

            timer.Start();

        }

        private async void TryRegisterUserAsync()
        {
            bool success = await HueAPI.Instance.RegisterUserAsync();

            if (success)
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer = null;
                }
            }
        }

        private async void ShowTimeoutDialogAsync()
        {
            MessageDialog dialog = new MessageDialog("Time has ran out.\nPlease tap on your bridge again to retry.");
            dialog.Commands.Add(new UICommand("Retry", OnRetryButtonClicked));
            await dialog.ShowAsync();
        }

        private void OnRetryButtonClicked(IUICommand command)
        {            
            TryRegisterUser();
        }

    }
}
