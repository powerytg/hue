using Hue.API.Hue.Themes.SystemThemes;
using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue.Themes
{
    public class ThemeManager
    {
        private static volatile ThemeManager instance;
        private static object syncRoot = new Object();

        public static ThemeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ThemeManager();
                    }
                }

                return instance;
            }
        }

        public List<HueTheme> Themes { get; set; }

        private ThemeManager()
        {
            Themes = new List<HueTheme>();
            Themes.Add(new DeepBlueTheme());
            Themes.Add(new HueTheme { Name = "Dreaming", IsSystemTheme = true, BannerImage = "/Assets/Themes/Dream.png" });
            Themes.Add(new HueTheme { Name = "Enchanted", IsSystemTheme = true, BannerImage = "/Assets/Themes/Enchanted.png" });
            Themes.Add(new HueTheme { Name = "Pure Essense", IsSystemTheme = true, BannerImage = "/Assets/Themes/Pure.png" });
            Themes.Add(new HueTheme { Name = "Serenity", IsSystemTheme = true, BannerImage = "/Assets/Themes/Serenity.png" });
            Themes.Add(new HueTheme { Name = "Starry Night", IsSystemTheme = true, BannerImage = "/Assets/Themes/Starry.png" });
        }

        public async Task<bool> ApplyThemeAsync(HueTheme theme)
        {
            if (theme.ColorList.Count == 0)
            {
                return true;
            }

            int colorIndex = 0;
            foreach (var light in BridgeManager.Instance.CurrentBridge.LightList)
            {
                HSBColor color = theme.ColorList[colorIndex];
                var attrs = new { hue = (int)color.H, sat = (int)color.S, bri = (int)color.B };
                await HueAPI.Instance.SetLightStateAsync(light.LightId, attrs);

                // Update light attrs
                light.Hue = (int)color.H;
                light.Saturation = (int)color.S;
                light.Brightness = (int)color.B;
                BridgeManager.Instance.InvalidateLightProperties(light);

                if (colorIndex == theme.ColorList.Count - 1)
                {
                    colorIndex = 0;
                }
            }

            return true;
        }

    }
}
