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
            LightToggle.IsOn = LightSource.IsOn;

            ColorEditor.LightSource = LightSource;
            ColorEditor.HSBColorSource = new HSBColor(LightSource.Hue, LightSource.Saturation, LightSource.Brightness);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LightEditorView()
        {
            this.InitializeComponent();

            LightToggle.ValueChanged += OnLightToggleValueChanged;
        }

        private void OnLightToggleValueChanged(object sender, EventArgs e)
        {            
            ToggleLightAsync(LightToggle.IsOn);
        }

        private async void ToggleLightAsync(bool isOn)
        {
            var attrs = new { on = isOn };
            await HueAPI.Instance.SetLightStateAsync(LightSource.LightId, attrs);
        }

        private async void UpdateLightName(string newName)
        {
            if (newName.Trim().Length == 0)
            {
                // Revert to original name
                NameInput.Text = LightSource.Name;
            }
            else
            {
                // The API allows no longer than 32 characters for the name
                var truncatedName = newName.Length > 32 ? newName.Substring(0, 32) : newName;
                var attrs = new { name = truncatedName };
                await HueAPI.Instance.SetLightAttributesAsync(LightSource.LightId, attrs);

                LightSource.Name = truncatedName;
                LightSource.InvalidateLightProperties();
            }
        }

        private void NameInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                UpdateLightName(NameInput.Text);
            }
        }

    }
}
