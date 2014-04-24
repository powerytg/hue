using Hue.API.Hue;
using Hue.API.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class LightEditorView : UserControl
    {
        public static readonly DependencyProperty LightSourceProperty = DependencyProperty.Register(
       "LightSource",
       typeof(Light),
       typeof(LightEditorView),
       new PropertyMetadata(null, OnLightSourcePropertyChanged));

        public Light LightSource
        {
            get { return (Light)GetValue(LightSourceProperty); }
            set { SetValue(LightSourceProperty, value); }
        }

        private static void OnLightSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (LightEditorView)sender;
            target.OnLightSourceChanged();
        }

        private void OnLightSourceChanged()
        {
            NameInput.Text = LightSource.Name;

            ColorEditor.LightSource = LightSource;
            ColorEditor.HSBColorSource = new HSBColor(LightSource.Hue, LightSource.Saturation, LightSource.Brightness);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LightEditorView()
        {
            this.InitializeComponent();
        }
    }
}
