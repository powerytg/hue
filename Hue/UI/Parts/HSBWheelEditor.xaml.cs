using Hue.API.Hue;
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
    public sealed partial class HSBWheelEditor : HSBColorEditorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HSBWheelEditor()
        {
            this.InitializeComponent();

            // Events
            HueDialer.ValueChanged += OnHueValueChanged;
        }

        protected override void OnHSBColorSourceChanged()
        {
            HueDialer.HSBColorSource = HSBColorSource;
        }

        private void OnHueValueChanged(object sender, EventArgs e)
        {
            var attrs = new { hue = HueDialer.CurrentValue };
            HueAPI.Instance.SetLightStateAsync(LightSource.LightId, attrs);
        }

    }
}
