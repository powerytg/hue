using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue
{
    public class Light
    {
        public string LightId { get; set; }
        public string Name { get; set; }

        public bool IsReachable { get; set; }
        public bool IsOn { get; set; }

        /// <summary>
        /// uint8.
        /// 
        /// Min 0
        /// Max 255
        /// </summary>
        public int Brightness { get; set; }

        public static int MinBrightness = 0;
        public static int MaxBrightness = 255;

        /// <summary>
        /// Min 0
        /// Max 65535
        /// </summary>
        public int Hue { get; set; }

        public static int MinHue = 0;
        public static int MaxHue = 65535;

        /// <summary>
        /// Min 0 (white)
        /// Max 255 (color)
        /// </summary>
        public int Saturation { get; set; }

        public static int MinSaturation = 0;
        public static int MaxSaturation = 255;

        /// <summary>
        /// X in CIE color space, between 0 and 1
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y in CIE color space, between 0 and 1
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Min 153 (6500K)
        /// Max 500 (2000K)
        /// </summary>
        public int Temperature { get; set; }

        public static int MinTemperature = 153;
        public static int MaxTemperature = 500;

        /// <summary>
        /// none, select (breathe), lselect (cycle)
        /// </summary>
        public string AlertEffect { get; set; }

        public static string AlertEffectNone = "none";
        public static string AlertEffectSelect = "select";
        public static string AlertEffectLSelect = "lselect";

        /// <summary>
        /// none or "colorloop"
        /// </summary>
        public string Effect { get; set; }

        public static string EffectNone = "none";
        public static string EffectColorLoop = "colorloop";

        /// <summary>
        /// default is 4 (400ms). unit is 100ms
        /// </summary>
        public int TransitionTime { get; set; }

        /// <summary>
        /// xy (for XY), ct (for temperature) or hs (for hue saturation)
        /// </summary>
        public string ColorMode { get; set; }

        public static string ColorModeXY = "xy";
        public static string ColorModeTemperature = "ct";
        public static string ColorModeHueSaturation = "hs";

        // Events
        public EventHandler LightPropertyChanged;

        public void InvalidateLightProperties()
        {
            if (LightPropertyChanged != null)
            {
                LightPropertyChanged(this, null);
            }
        }
    }
}
