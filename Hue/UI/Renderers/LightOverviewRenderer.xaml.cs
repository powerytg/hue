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
    public sealed partial class LightOverviewRenderer : UserControl
    {        
        private static SolidColorBrush onStroke = new SolidColorBrush(Color.FromArgb(0x00, 0x66, 0x66, 0x66));
        private static SolidColorBrush offStroke = new SolidColorBrush(Color.FromArgb(0xff, 0x33, 0x33, 0x33));
        private static SolidColorBrush offFill = new SolidColorBrush(Color.FromArgb(0x00, 0x66, 0x66, 0x66));

        private static SolidColorBrush onLabelBrush = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0xff));
        private static SolidColorBrush offLabelBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x96, 0x96, 0x96));


        public static readonly DependencyProperty LightSourceProperty = DependencyProperty.Register(
        "LightSource",
        typeof(Light),
        typeof(LightOverviewRenderer),
        new PropertyMetadata(null, OnLightSourcePropertyChanged));

        public Light LightSource
        {
            get { return (Light)GetValue(LightSourceProperty); }
            set { SetValue(LightSourceProperty, value); }
        }

        private static void OnLightSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (LightOverviewRenderer)sender;
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
        public LightOverviewRenderer()
        {
            this.InitializeComponent();
        }

        private void NameLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Force refresh
            LightEditor.LightSource = null;
            LightEditor.LightSource = LightSource;
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void OnLightPropertyChanged(object sender, EventArgs e)
        {
            UpdateDisplayList();
        }

        private void UpdateDisplayList()
        {
            if (LightSource.IsOn)
            {
                NameLabel.Foreground = onLabelBrush;

                Color rgbColor = HSBColor.FromHSB(LightSource.Hue, LightSource.Saturation, LightSource.Brightness);
                ColorIndicator.Fill = new SolidColorBrush(rgbColor);
                ColorIndicator.Stroke = onStroke;
            }
            else
            {
                NameLabel.Foreground = offLabelBrush;

                ColorIndicator.Fill = offFill;
                ColorIndicator.Stroke = offStroke;
            }

            NameLabel.Text = LightSource.Name;
        }

        private void ColorIndicator_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleLightAsync();
        }

        private async void ToggleLightAsync()
        {
            LightSource.IsOn = !LightSource.IsOn;
            LightSource.InvalidateLightProperties();

            var attrs = new { on = LightSource.IsOn };
            await HueAPI.Instance.SetLightStateAsync(LightSource.LightId, attrs);            
        }
    }
}
