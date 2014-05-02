using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue.Themes
{
    public class HueTheme : IHueTheme
    {
        public string Name { get; set; }
        public bool IsSystemTheme { get; set; }
        public string BannerImage { get; set; }

        public ObservableCollection<HSBColor> ColorList { get; set; }

        public HueTheme()
        {
            ColorList = new ObservableCollection<HSBColor>();
        }
    }
}
