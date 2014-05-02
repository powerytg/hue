using Hue.API.Hue;
using Hue.UI.SupportClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hue.UI
{
    public sealed partial class LightsHubSection : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LightsHubSection()
        {
            this.InitializeComponent();

            LightsToggleButton.IsEnabled = false;

            UpdateDisplayList();

            // Events
            HueAPI.Instance.GetBridgeConfigurationsComplete += OnLightsUpdated;
            BridgeManager.Instance.LightsOnOffStateChanged += OnLightsOnOffChanged;
        }

        private void OnLightsUpdated(object sender, EventArgs e)
        {
            UpdateDisplayList();
        }

        private void UpdateDisplayList()
        {
            ObservableCollection<object> ds = new ObservableCollection<object>();
            foreach (var light in BridgeManager.Instance.CurrentBridge.LightList)
            {
                ds.Add(light);
            }

            LightListView.ItemsSource = ds;
            HueBar.UpdateDisplayList();

            // Summary
            LightSummaryLabel.Text = ds.Count.ToString() + " LIGHTS";

            // Toggle button            
            UpdateLightControlLabel();
        }

        private void UpdateLightControlLabel()
        {
            var onCount = BridgeManager.Instance.GetActiveLightCount();
            if (BridgeManager.Instance.CurrentBridge.LightList.Count == 0)
            {
                LightsToggleButton.IsEnabled = false;
            }
            else if (onCount > 0)
            {
                LightsToggleButton.Content = "ALL OFF";
                LightsToggleButton.IsEnabled = true;
            }
            else
            {
                LightsToggleButton.Content = "ALL ON";
                LightsToggleButton.IsEnabled = true;
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage.RefreshBridgeRequested(this, null);
        }

        private void LightsToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (BridgeManager.Instance.CurrentBridge.LightList.Count == 0)
            {
                return;
            }

            int onCount = BridgeManager.Instance.GetActiveLightCount();
            if (onCount > 0)
            {
                TurnOffAllLights();
            }
            else
            {
                TurnOnAllLights();
            }
        }

        private async void TurnOffAllLights()
        {
            await BridgeManager.Instance.TurnOffAllLightsAsync();
        }

        private async void TurnOnAllLights()
        {
            await BridgeManager.Instance.TurnOnAllLightsAsync();
        }

        private void OnLightsOnOffChanged(object sender, EventArgs e)
        {
            UpdateLightControlLabel();
        }

    }
}
