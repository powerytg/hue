using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Hue.API.Hue.Themes.SystemThemes
{
    public class StarryThemeTemplate : HueTheme
    {
        public StarryThemeTemplate() : base()
        {
            Name = "starry nights";
            IsSystemTheme = true;
            BannerImage = "/Assets/Themes/Starry.png";

            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x00, 0xbf, 0xf3)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x00, 0xa9, 0x9d)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xf8, 0x94, 0x1d)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xf2, 0x6d, 0x7d)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x85, 0x60, 0xa9)));

            foreach (var color in DefaultColorList)
            {
                ColorList.Add(color.Clone());
            }
        }

    }
}
