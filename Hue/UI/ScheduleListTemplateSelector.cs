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
    public class ScheduleListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ScheduleTemplate { get; set; }
        public DataTemplate UnsupportedScheduleTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            Schedule schedule = (Schedule)item;
            if (schedule.IsSupportedSchedule)
            {
                return ScheduleTemplate;
            }
            else 
            {
                return UnsupportedScheduleTemplate;
            }
        }
    }
}
