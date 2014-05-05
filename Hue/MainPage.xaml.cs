using Hue.API.Hue;
using Hue.API.Hue.Themes;
using Hue.Common;
using Hue.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Hue
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static EventHandler RefreshBridgeRequested;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Events
            HueAPI.Instance.GetBridgeConfigurationsComplete += OnBridgeUpdated;
            HueAPI.Instance.GetBridgeConfigurationsFailed += OnBridgeUpdateFailed;

            RefreshBridgeRequested += OnRefreshBridgeRequested;

            // Refresh bridge configurations
            RefreshConfigurationsAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            RefreshBridgeRequested -= OnRefreshBridgeRequested;

            base.OnNavigatedFrom(e);
        }

        private void OnRefreshBridgeRequested(object sender, EventArgs e)
        {
            RefreshConfigurationsAsync();
        }

        private async void OnBridgeUpdated(object sender, EventArgs e)
        {
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.HideAsync();
        }

        private async void OnBridgeUpdateFailed(object sender, EventArgs e)
        {
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.HideAsync();
        }

        private async void RefreshConfigurationsAsync()
        {
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.ShowAsync();

            HueAPI.Instance.GetAllConfigurationsAsync();
        }

        private void AddThemeButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewThemeAsync();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshConfigurationsAsync();
        }

        private async void CreateNewThemeAsync()
        {
            var newTheme = await ThemeManager.Instance.CreateThemeAsync();
            await ThemeManager.Instance.ApplyThemeAsync(newTheme);

            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(ThemePage), newTheme);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAboutViewAsync();
        }

        private async void ShowAboutViewAsync()
        {
            var text = "Hue Lumixury 1.0\n";
            text += "Elegant, easy and free\n\n";
            text += "From the creator of Indulged :)\n\n";
            text += "2014 Tiangong You, all rights reserved\n";
            
            var msg = new MessageDialog(text);
            await msg.ShowAsync();
        }

    }
}
