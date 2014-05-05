using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Hue.API.Hue.Themes.SystemThemes
{
    public class PureThemeTemplate : HueTheme
    {
        public PureThemeTemplate()
            : base()
        {
            Name = "arctic";
            IsSystemTheme = true;
            BannerImage = "/Assets/Themes/Pure.png";

            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x6d, 0xd0, 0xf7)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xaa, 0xe0, 0xf5)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xdb, 0xec, 0xf2)));

            foreach (var color in DefaultColorList)
            {
                ColorList.Add(color.Clone());
            }
        }

    }
}
