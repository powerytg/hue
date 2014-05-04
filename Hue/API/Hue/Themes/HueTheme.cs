using Hue.API.Media;
using Newtonsoft.Json;
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

        public string FileName { get; set; }

        public ObservableCollection<HSBColor> ColorList { get; set; }

        public HueTheme()
        {
            ColorList = new ObservableCollection<HSBColor>();
        }

        public string ToJsonString()
        {
            var colors = new List<object>();
            foreach (var color in ColorList)
            {
                colors.Add(new { h = (int)color.H, s = (int)color.S, b = (int)color.B });
            }

            var json = new { name = Name, is_system_default = IsSystemTheme, banner_image = BannerImage, colors = colors };
            var jsonString = JsonConvert.SerializeObject(json);
            return jsonString;
        }
    }
}
