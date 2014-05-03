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
    public sealed partial class ThemeColorEditor : UserControl
    {
        // Events
        public EventHandler ValueChanged;

        public static readonly DependencyProperty HSBColorSourceProperty = DependencyProperty.Register(
       "HSBColorSource",
       typeof(HSBColor),
       typeof(ThemeColorEditor),
       new PropertyMetadata(null, OnHSBColorSourcePropertyChanged));

        public HSBColor HSBColorSource
        {
            get { return (HSBColor)GetValue(HSBColorSourceProperty); }
            set { SetValue(HSBColorSourceProperty, value); }
        }

        private static void OnHSBColorSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (ThemeColorEditor)sender;
            target.OnHSBColorSourceChanged();
        }

        private void OnHSBColorSourceChanged()
        {
            if (HSBColorSource == null)
            {
                return;
            }

            UpdateColorPreview();
            Editor.HSBColorSource = HSBColorSource;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ThemeColorEditor()
        {
            this.InitializeComponent();

            Editor.ValueChanged += OnColorChanged;
        }

        private void UpdateColorPreview()
        {
            Color fillColor = HSBColor.FromHSB(HSBColorSource);
            ColorNameLabel.Text = HSBColorSource.ToRGBString();

            double maxSize = PreviewBorder.Width - 15;
            double previewSize = maxSize * (HSBColorSource.B / Light.MaxBrightness);
            ThumbnailView.Width = previewSize;
            ThumbnailView.Height = previewSize;
            ThumbnailFill.Color = fillColor;
        }

        private void OnColorChanged(object sender, EventArgs e)
        {
            UpdateColorPreview();

            if (ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

    }
}
