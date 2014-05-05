using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Hue.API.Hue.Themes.SystemThemes
{
    public class SerenityThemeTemplate : HueTheme
    {
        public SerenityThemeTemplate() : base()
        {
            Name = "serenity";
            IsSystemTheme = true;
            BannerImage = "/Assets/Themes/Serenity.png";

            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x92, 0x27, 0x8f)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xed, 0x00, 0x8c)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x00, 0x72, 0xbc)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x2d, 0xa0, 0xea)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x00, 0xa9, 0x9d)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x39, 0xb5, 0x4a)));

            foreach (var color in DefaultColorList)
            {
                ColorList.Add(color.Clone());
            }
        }

    }
}
