using Hue.API.Hue;
using Hue.API.Media;
using HueSaturation.API.Hue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hue.UI.Parts
{
    public sealed partial class BridgeControl : UserControl
    {
        private static string LightOffText = "Lights Off";
        private static string LightOnText = "Lights On";
        //private static string LightNoneText = "No Lights";

        /// <summary>
        /// Constructor
        /// </summary>
        public BridgeControl()
        {
            this.InitializeComponent();

            // Events
            BridgeManager.Instance.LightsOnOffStateChanged += OnLightsOnOffChanged;
        }

        public void UpdateLightWidgets()
        {
            RingGrid.Children.Clear();

            if (BridgeManager.Instance.CurrentBridge.LightList.Count == 0)
            {
                LightControlLabel.Text = "No Light";
                return;
            }

            var radius = (this.Width - OutterRing.Margin.Left - OutterRing.Margin.Right) / 2;
            var angleStep = Math.PI * 2 / BridgeManager.Instance.CurrentBridge.LightList.Count;

            int i = 0;
            foreach(var light in BridgeManager.Instance.CurrentBridge.LightList)
            {
                var indicator = new LightIndicatorControl();
                indicator.Width = 10;
                indicator.Height = 10;
                indicator.LightSource = light;

                RingGrid.Children.Add(indicator);

                // Position the light indicator                
                double angle = i * angleStep;
                var tf = new TranslateTransform();

                tf.X = radius * Math.Cos(angle);
                tf.Y = radius * Math.Sin(angle);
                indicator.RenderTransform = tf;

                i++;
            }

            UpdateLightControlLabel();

            int onCount = BridgeManager.Instance.GetActiveLightCount();
            if(onCount > 0)
            {
                BridgeAnimation.Begin();
            }
        }

        private void UpdateLightControlLabel()
        {
            int onCount = BridgeManager.Instance.GetActiveLightCount();
            if (onCount > 0)
            {
                LightControlLabel.Text = LightOffText;
            }
            else
            {
                LightControlLabel.Text = LightOnText;
            }

        }

        private void LightControl_Tapped(object sender, TappedRoutedEventArgs e)
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
            LightToggle.IsHitTestVisible = false;
            LightToggle.Opacity = 0.5;

            await BridgeManager.Instance.TurnOffAllLightsAsync();

            LightToggle.Opacity = 1;
            LightToggle.IsHitTestVisible = true;

            UpdateLightControlLabel();
            BridgeAnimation.Stop();
        }

        private async void TurnOnAllLights()
        {
            LightToggle.IsHitTestVisible = false;
            LightToggle.Opacity = 0.5;

            await BridgeManager.Instance.TurnOnAllLightsAsync();

            LightToggle.Opacity = 1;
            LightToggle.IsHitTestVisible = true;

            UpdateLightControlLabel();
            BridgeAnimation.Begin();
        }

        private void OnLightsOnOffChanged(object sender, EventArgs e)
        {
            UpdateLightControlLabel();

            int onCount = BridgeManager.Instance.GetActiveLightCount();
            if (onCount > 0)
            {
                BridgeAnimation.Begin();
            }
            else
            {
                BridgeAnimation.Stop();
            }

        }
    }
}
