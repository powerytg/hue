using Hue.API.Hue;
using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hue.UI.Renderers
{
    public sealed partial class ColorRenderer : UserControl
    {        
        public static readonly DependencyProperty HSBColorSourceProperty = DependencyProperty.Register(
        "HSBColorSource",
        typeof(HSBColor),
        typeof(ColorRenderer),
        new PropertyMetadata(null, OnHSBColorSourcePropertyChanged));

        public HSBColor HSBColorSource
        {
            get { return (HSBColor)GetValue(HSBColorSourceProperty); }
            set { SetValue(HSBColorSourceProperty, value); }
        }

        private static void OnHSBColorSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (ColorRenderer)sender;
            target.OnHSBColorSourceChanged();
        }

        private void OnHSBColorSourceChanged()
        {
            UpdateDisplayList();
        }
       
        /// <summary>
        /// Constructor
        /// </summary>
        public ColorRenderer()
        {
            this.InitializeComponent();
        }

        private void NameLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {            
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
       
        private void UpdateDisplayList()
        {
            Color rgbColor = HSBColor.FromHSB((int)HSBColorSource.H, (int)HSBColorSource.S, (int)HSBColorSource.B);
            ColorIndicator.Fill = new SolidColorBrush(rgbColor);

            NameLabel.Text = rgbColor.ToString();
        }

    }
}
