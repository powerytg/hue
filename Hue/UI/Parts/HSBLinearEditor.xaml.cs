using Hue.API.Hue;
using Hue.API.Media;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hue.UI.Parts
{
    public sealed partial class HSBLinearEditor : HSBColorEditorBase
    {
        // Events
        public EventHandler ValueChanged;

        protected override void OnHSBColorSourceChanged()
        {
            if (HSBColorSource == null)
            {
                return;
            }

            int stops = 7;
            HueGradient.GradientStops.Clear();
            for (int i = 0; i <= stops; i++)
            {
                GradientStop gs = new GradientStop();
                gs.Offset = (float)i / stops;

                int hue = (int)Math.Floor(Light.MaxHue * gs.Offset);
                gs.Color = HSBColor.FromHSB(hue, (int)HSBColorSource.S / 2, (int)HSBColorSource.B);
                HueGradient.GradientStops.Add(gs);
            }

            SaturationSliderHighlightBrush.Color = HSBColor.FromHSB(HSBColorSource);

            // Set slider thumb positions
            if (HueSlider.Value != HSBColorSource.H)
            {
                HueSlider.Value = HSBColorSource.H;
            }

            if (SaturationSlider.Value != HSBColorSource.S)
            {
                SaturationSlider.Value = HSBColorSource.S;
            }

            if (BrightnessSlider.Value != HSBColorSource.B)
            {
                BrightnessSlider.Value = HSBColorSource.B;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HSBLinearEditor()
        {
            this.InitializeComponent();
        }

        private void HueSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            HSBColorSource.H = (float)HueSlider.Value;
            SaturationSliderHighlightBrush.Color = HSBColor.FromHSB(HSBColorSource);

            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        private void SaturationSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            HSBColorSource.S = (float)SaturationSlider.Value;
            SaturationSliderHighlightBrush.Color = HSBColor.FromHSB(HSBColorSource);

            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        private void BrightnessSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            HSBColorSource.B = (float)BrightnessSlider.Value;
            SaturationSliderHighlightBrush.Color = HSBColor.FromHSB(HSBColorSource);

            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }
    }
}
