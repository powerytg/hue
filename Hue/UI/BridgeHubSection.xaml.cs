﻿using Hue.API.Hue;
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

namespace Hue.UI
{
    public sealed partial class BridgeHubSection : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BridgeHubSection()
        {
            this.InitializeComponent();

            UpdateDisplayList();

            // Events
            HueAPI.Instance.GetBridgeConfigurationsComplete += OnConfigUpdated;
            BridgeManager.Instance.BridgePropertyChanged += OnBridgePropertyChanged;
        }

        private void UpdateDisplayList()
        {
            Bridge currentBridge = BridgeManager.Instance.CurrentBridge;

            NameLabel.Text = currentBridge.Name;
            VersionLabel.Text = currentBridge.Version;

            BridgeWidget.UpdateLightWidgets();
        }

        private void OnConfigUpdated(object sender, EventArgs e)
        {
            UpdateDisplayList();
        }

        private void OnBridgePropertyChanged(object sender, EventArgs e)
        {
            NameLabel.Text = BridgeManager.Instance.CurrentBridge.Name;
        }

        private void EditNameButton_Click(object sender, RoutedEventArgs e)
        {
            BridgeNameEditor.UpdateDisplayList();
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

    }
}
