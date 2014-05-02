using Hue.API.Hue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hue.UI.Renderers
{
    public class ScheduleRendererBase : UserControl
    {
        public static readonly DependencyProperty ScheduleSourceProperty = DependencyProperty.Register(
        "ScheduleSource",
        typeof(Schedule),
        typeof(ScheduleRendererBase),
        new PropertyMetadata(null, OnScheduleSourcePropertyChanged));

        public Schedule ScheduleSource
        {
            get { return (Schedule)GetValue(ScheduleSourceProperty); }
            set { SetValue(ScheduleSourceProperty, value); }
        }

        private static void OnScheduleSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (ScheduleRendererBase)sender;
            target.OnScheduleSourceChanged();
        }

        protected virtual void OnScheduleSourceChanged()
        {

        }

    }
}
