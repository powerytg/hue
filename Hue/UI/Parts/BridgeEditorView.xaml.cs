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
    public sealed partial class BridgeEditorView : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BridgeEditorView()
        {
            this.InitializeComponent();
        }

        public void UpdateDisplayList()
        {
            NameInput.Text = BridgeManager.Instance.CurrentBridge.Name;
        }

        private async void UpdateBridgeName(string newName)
        {
            Bridge currentBridge = BridgeManager.Instance.CurrentBridge;

            if (newName.Trim().Length == 0)
            {
                // Revert to original name
                NameInput.Text = currentBridge.Name;
            }
            else
            {
                // The API allows no longer than 16 characters for the name, and no less than 4
                string truncatedName;
                if(newName.Length < 4)
                {
                    truncatedName = currentBridge.Name;
                }
                else if(newName.Length > 16)
                {
                    truncatedName = newName.Substring(0, 16);
                }
                else
                {
                    truncatedName = newName;
                }

                var attrs = new { name = truncatedName };
                await HueAPI.Instance.SetBridgeConfigurationsAsync(attrs);

                currentBridge.Name = truncatedName;
                BridgeManager.Instance.InvalidateBridgeProperties();
            }
        }

        private void NameInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                UpdateBridgeName(NameInput.Text);
            }
        }
    }
}
