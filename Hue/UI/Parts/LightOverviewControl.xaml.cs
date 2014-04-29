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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hue.UI.Parts
{
    public sealed partial class LightOverviewControl : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LightOverviewControl()
        {
            this.InitializeComponent();

            var stops = 7;
            var step = Light.MaxHue / stops;
            for(int i = 0; i < stops; i++)
            {
                var gs = new GradientStop();
                gs.Offset = (float)i / stops;
                gs.Color = HSBColor.FromHSB(i * step, 100, 255);
                HueGradient.GradientStops.Add(gs);
            }
        }

        public void UpdateDisplayList()
        {
            List<int> hueList = new List<int>();
            ThumbCanvas.Children.Clear();

            foreach(var light in BridgeManager.Instance.CurrentBridge.LightList)
            {
                if (!hueList.Contains(light.Hue))
                {
                    hueList.Add(light.Hue);
                }                
            }

            for (int i = 0; i < hueList.Count; i++ )
            {
                var thumb = new Image();
                var offset = hueList[i] / (float)Light.MaxHue;
                thumb.Source = new BitmapImage(new Uri("ms-appx:///Assets/LightOverviewThumb.png"));
                thumb.Width = 10;
                thumb.Height = 10;
                thumb.SetValue(Canvas.LeftProperty, offset * ActualWidth);
                ThumbCanvas.Children.Add(thumb);
            }

        }

    }
}
