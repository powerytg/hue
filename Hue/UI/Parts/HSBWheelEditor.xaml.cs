using Hue.API.Hue;
using Hue.API.Media;
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

namespace Hue.UI.Parts
{
    public sealed partial class HSBWheelEditor : HSBColorEditorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HSBWheelEditor()
        {
            this.InitializeComponent();

            // Events
            HueDialer.ValueChanged += OnHueValueChanged;
        }

        protected override void OnHSBColorSourceChanged()
        {
            HueDialer.HSBColorSource = HSBColorSource;

            // Set slider thumb positions
            SaturationSliderHighlightBrush.Color = HSBColor.FromHSB(HueDialer.CurrentValue, 255, 255);

            if (SaturationSlider.Value != HSBColorSource.S)
            {
                SaturationSlider.Value = HSBColorSource.S;
            }

            if (BrightnessSlider.Value != HSBColorSource.B)
            {
                BrightnessSlider.Value = HSBColorSource.B;
            }

        }

        private void OnHueValueChanged(object sender, EventArgs e)
        {
            LightSource.Hue = HueDialer.CurrentValue;
            LightSource.InvalidateLightProperties();

            var attrs = new { hue = HueDialer.CurrentValue };
            UpdateLightStateAsync(attrs);

            SaturationSliderHighlightBrush.Color = HSBColor.FromHSB(LightSource.Hue, LightSource.Saturation, LightSource.Brightness);
        }

        private void SaturationSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            LightSource.Saturation = (int)SaturationSlider.Value;
            LightSource.InvalidateLightProperties();

            SaturationSliderHighlightBrush.Color = HSBColor.FromHSB(LightSource.Hue, LightSource.Saturation, LightSource.Brightness);

            var attrs = new { sat = LightSource.Saturation };
            UpdateLightStateAsync(attrs);
        }

        private void BrightnessSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            LightSource.Brightness = (int)BrightnessSlider.Value;
            LightSource.InvalidateLightProperties();

            SaturationSliderHighlightBrush.Color = HSBColor.FromHSB(LightSource.Hue, LightSource.Saturation, LightSource.Brightness);

            var attrs = new { bri = LightSource.Brightness };
            UpdateLightStateAsync(attrs);
        }

        private async void UpdateLightStateAsync(object attrs)
        {
            await HueAPI.Instance.SetLightStateAsync(LightSource.LightId, attrs);
        }
    }
}
