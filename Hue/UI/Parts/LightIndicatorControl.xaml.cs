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

namespace Hue.UI.Parts
{
    public sealed partial class LightIndicatorControl : UserControl
    {
        private static SolidColorBrush offFill = new SolidColorBrush(Color.FromArgb(0xff, 0x22, 0x22, 0x22));

        public static readonly DependencyProperty LightSourceProperty = DependencyProperty.Register(
        "LightSource",
        typeof(Light),
        typeof(LightIndicatorControl),
        new PropertyMetadata(null, OnLightSourcePropertyChanged));

        public Light LightSource
        {
            get { return (Light)GetValue(LightSourceProperty); }
            set { SetValue(LightSourceProperty, value); }
        }

        private static void OnLightSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (LightIndicatorControl)sender;
            target.OnLightSourceChanged();
        }

        private void OnLightSourceChanged()
        {
            UpdateDisplayList();

            // Events
            LightSource.LightPropertyChanged += OnLightPropertyChanged;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LightIndicatorControl()
        {
            this.InitializeComponent();
        }

        private void OnLightPropertyChanged(object sender, EventArgs e)
        {
            UpdateDisplayList();
        }

        private void UpdateDisplayList()
        {
            if (LightSource.IsOn)
            {
                Color rgbColor = HSBColor.FromHSB(LightSource.Hue, LightSource.Saturation, LightSource.Brightness);
                ColorIndicator.Fill = new SolidColorBrush(rgbColor);
            }
            else
            {
                ColorIndicator.Fill = offFill;
            }
        }
    }
}
