using Hue.API.Hue.Themes;
using Hue.API.Media;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue.Factories
{
    public class ThemeFactory
    {
        public static HueTheme CreateThemeFromJson(string content)
        {
            try
            {
                HueTheme theme = new HueTheme();
                JObject json = (JObject)JObject.Parse(content);

                // Name
                JToken nameToken;
                if (json.TryGetValue("name", out nameToken))
                {
                    theme.Name = json["name"].ToString();
                }

                // Is system theme
                JToken isSystemToken;
                if (json.TryGetValue("is_system_default", out isSystemToken))
                {
                    theme.IsSystemTheme = bool.Parse(json["is_system_default"].ToString());
                }

                // Banner image
                JToken imageToken;
                if (json.TryGetValue("banner_image", out imageToken))
                {
                    string imageString = json["banner_image"].ToString();
                    if (imageString.Length == 0)
                    {
                        theme.BannerImage = null;
                    }
                    else
                    {
                        theme.BannerImage = imageString;
                    }
                }

                // Color list
                JToken colorToken;
                if (json.TryGetValue("colors", out colorToken))
                {
                    JArray colorArray = (JArray)json["colors"];
                    foreach (JObject colorJson in colorArray)
                    {
                        int h = int.Parse(colorJson["h"].ToString());
                        int s = int.Parse(colorJson["s"].ToString());
                        int b = int.Parse(colorJson["b"].ToString());
                        var color = new HSBColor(h, s, b);

                        theme.ColorList.Add(color);
                    }                    
                }

                return theme;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
