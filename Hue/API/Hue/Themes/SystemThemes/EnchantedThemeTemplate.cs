using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Hue.API.Hue.Themes.SystemThemes
{
    public class EhchantedThemeTemplate : HueTheme
    {
        public EhchantedThemeTemplate()
            : base()
        {
            Name = "enchanted";
            IsSystemTheme = true;
            BannerImage = "/Assets/Themes/Enchanted.png";

            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xf7, 0x97, 0x79)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xf2, 0x65, 0x22)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xf4, 0x9a, 0xc2)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xf2, 0x6d, 0x7d)));
            DefaultColorList.Add(HSBColor.FromColor(Color.FromArgb(0xff, 0xee, 0x14, 0x5b)));

            foreach (var color in DefaultColorList)
            {
                ColorList.Add(color.Clone());
            }
        }

    }
}
