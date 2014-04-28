using Hue.API.Hue;
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
    public class LightListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LightTemplate { get; set; }
        public DataTemplate RefreshLightListTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is Light)
            {
                return LightTemplate;
            }
            else if (item is RefreshLightListModel)
            {
                return RefreshLightListTemplate;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
