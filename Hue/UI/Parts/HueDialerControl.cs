using Hue.API.Hue;
using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Hue.UI.Parts
{
    public class HueDialerControl : DialerControlBase
    {
        protected override List<GradientStop> GetBorderGradient()
        {
            int stops = 7;
            var stopList = new List<GradientStop>();
            for (int i = 0; i < stops; i++)
            {
                GradientStop gs = new GradientStop();
                gs.Offset = (float)i / stops;

                int hue = (int)Math.Floor(Light.MaxHue * gs.Offset);

                // Use only half of the sat to look more accurate.
                int sat = (int)HSBColorSource.S / 2;

                gs.Color = HSBColor.FromHSB(hue, sat, (int)HSBColorSource.B);
                stopList.Add(gs);
            }

            return stopList;
        }

        protected override BitmapImage GetDialerImage()
        {
            return new BitmapImage(new Uri("ms-appx:/Assets/HSBRing.png"));
        }

        protected override void OnHSBColorSourceChanged()
        {
            base.OnHSBColorSourceChanged();

            // Rotate to current value
            float percent = (float)HSBColorSource.H / Light.MaxHue;

            CurrentIndex = (int)Math.Floor(percent * SupportedValues.Count);
            CurrentValue = SupportedValues[CurrentIndex];

            RotateToCurrentValue();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HueDialerControl() : base()
        {
            var stopList = new List<int>();
            int stops = 180;
            int interval = Light.MaxHue / stops;
            for (int i = 0; i < stops; i++)
            {
                stopList.Add(i * interval);
            }

            SupportedValues = stopList;
        }


    }
}
