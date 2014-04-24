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
        protected override void OnHSBColorSourceChanged()
        {
            int stops = 7;
            HueGradient.GradientStops.Clear();
            for (int i = 0; i < stops; i++)
            {
                GradientStop gs = new GradientStop();
                gs.Offset = (float)i / stops;

                int hue = (int)Math.Floor(Light.MaxHue * gs.Offset);
                gs.Color = HSBColor.FromHSB(hue, (int)HSBColorSource.S, (int)HSBColorSource.B);
                HueGradient.GradientStops.Add(gs);
            }

            SaturationStop.Color = HSBColor.FromHSB((int)HSBColorSource.H, 255, 255);
            BrightnessStop.Color = HSBColor.FromHSB((int)HSBColorSource.H, 255, 255);

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
    }
}
