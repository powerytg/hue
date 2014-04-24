using Hue.API.Hue;
using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Hue.UI.Parts
{
    public class HSBColorEditorBase : UserControl
    {
        public static readonly DependencyProperty HSBColorSourceProperty = DependencyProperty.Register(
        "HSBColorSource",
        typeof(HSBColor),
        typeof(HSBColorEditorBase),
        new PropertyMetadata(null, OnHSBColorSourcePropertyChanged));

        public HSBColor HSBColorSource
        {
            get { return (HSBColor)GetValue(HSBColorSourceProperty); }
            set { SetValue(HSBColorSourceProperty, value); }
        }

        private static void OnHSBColorSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (HSBColorEditorBase)sender;
            target.OnHSBColorSourceChanged();
        }

        protected virtual void OnHSBColorSourceChanged()
        {

        }

        public static readonly DependencyProperty LightSourceProperty = DependencyProperty.Register(
        "LightSource",
        typeof(Light),
        typeof(HSBColorEditorBase),
        new PropertyMetadata(null, OnLightSourcePropertyChanged));

        public Light LightSource
        {
            get { return (Light)GetValue(LightSourceProperty); }
            set { SetValue(LightSourceProperty, value); }
        }

        private static void OnLightSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (HSBColorEditorBase)sender;
            target.OnLightSourceChanged();
        }

        protected virtual void OnLightSourceChanged()
        {
        }
    }
}
