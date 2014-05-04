using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Hue.API.Hue.Themes.SystemThemes
{
    public class DeepBlueThemeTemplate : HueTheme
    {
        public DeepBlueThemeTemplate() : base()
        {
            Name = "deep blue";
            IsSystemTheme = true;
            BannerImage = "/Assets/Themes/Blue.png";

            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x0d, 0x7e, 0xff)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x0b, 0x74, 0xe9)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x0a, 0x65, 0xcc)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x09, 0x58, 0xb3)));

            foreach (var color in DefaultColorList)
            {
                ColorList.Add(color.Clone());
            }
        }

    }
}
