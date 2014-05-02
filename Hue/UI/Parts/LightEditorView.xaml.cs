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
            if(LightSource == null)
            {
                return;
            }

            NameInput.Text = LightSource.Name;
            LightToggle.IsOn = LightSource.IsOn;

            ColorEditor.LightSource = LightSource;
            ColorEditor.HSBColorSource = new HSBColor(LightSource.Hue, LightSource.Saturation, LightSource.Brightness);

            if (LightSource.IsOn)
            {
                EnableAllEditorViews();
            }
            else
            {
                DisableAllEditorViews();
            }
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
            if (LightToggle.IsOn)
            {
                EnableAllEditorViews();
            }
            else
            {
                DisableAllEditorViews();
            }

            LightSource.IsOn = LightToggle.IsOn;
            BridgeManager.Instance.InvalidateLightProperties(LightSource);
            ToggleLightAsync();
        }

        private async void ToggleLightAsync()
        {
            var attrs = new { on = LightSource.IsOn };
            await HueAPI.Instance.SetLightStateAsync(LightSource.LightId, attrs);

            BridgeManager.Instance.InvalidateAllLightsOnOffState();
        }

        private void DisableAllEditorViews()
        {
            NameInput.IsEnabled = false;
            ColorEditor.IsEnabled = false;
            ColorEditor.Opacity = 0.5;
        }

        private void EnableAllEditorViews()
        {
            NameInput.IsEnabled = true;
            ColorEditor.IsEnabled = true;
            ColorEditor.Opacity = 1;
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
                BridgeManager.Instance.InvalidateLightProperties(LightSource);
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
