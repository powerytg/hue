using Hue.API.Hue.Themes;
using Hue.UI.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hue.UI
{
    public class ThemeListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ThemeTemplate { get; set; }
        public DataTemplate UserThemeTemplate { get; set; }
        public DataTemplate NewThemeTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is NewThemeModel)
            {
                return NewThemeTemplate;
            }
            else if (item is HueTheme)
            {
                HueTheme theme = item as HueTheme;
                if (theme.IsSystemTheme) 
                { 
                    return ThemeTemplate;
                }
                else 
                {
                    return UserThemeTemplate;
                }

            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
