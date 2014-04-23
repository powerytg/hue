using Hue.API.Hue;
using HueSaturation.API.Hue;
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

            // Events
            HueAPI.Instance.GetBridgeConfigurationsComplete += OnLightsUpdated;
        }

        private void OnLightsUpdated(object sender, EventArgs e)
        {
            LightListView.ItemsSource = BridgeManager.Instance.CurrentBridge.LightList;
        }
    }
}
