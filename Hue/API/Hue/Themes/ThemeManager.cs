using Hue.API.Hue.Themes.SystemThemes;
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


    }
}
