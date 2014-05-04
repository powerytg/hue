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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hue.UI.Renderers
{
    public sealed partial class ColorRenderer : UserControl
    {
        // Events
        public static EventHandler ColorChanged;
        public static EventHandler ColorDeleted;

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

            // Events
            Editor.ValueChanged += OnColorChanged;
        }

        private void OnColorChanged(object sender, EventArgs e)
        {
            if (HSBColorSource == null)
            {
                return;
            }

            UpdateDisplayList();

            if (ColorChanged != null)
            {
                ColorChanged(this, null);
            }
        }


        private void NameLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Editor.HSBColorSource = null;
            Editor.HSBColorSource = HSBColorSource;
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void UpdateDisplayList()
        {
            if (HSBColorSource == null)
            {
                return;
            }

            Color rgbColor = HSBColor.FromHSB((int)HSBColorSource.H, (int)HSBColorSource.S, (int)HSBColorSource.B);
            ColorIndicator.Fill = new SolidColorBrush(rgbColor);

            NameLabel.Text = HSBColorSource.ToRGBString();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ColorDeleted != null)
            {
                ColorDeleted(HSBColorSource, null);
            }
        }

       
    }
}
