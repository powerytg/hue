using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Hue.API.Hue.Themes.SystemThemes
{
    public class DreamingThemeTemplate : HueTheme
    {
        public DreamingThemeTemplate()
            : base()
        {
            Name = "dreaming";
            IsSystemTheme = true;
            BannerImage = "/Assets/Themes/Dream.png";

            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x82, 0xca, 0x9c)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x1c, 0xbb, 0xb4)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x00, 0xbf, 0xf3)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0x44, 0x8c, 0xcb)));

            foreach (var color in DefaultColorList)
            {
                ColorList.Add(color.Clone());
            }
        }

    }
}
