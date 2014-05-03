using Hue.API.Hue.Themes;
using Hue.UI.Renderers;
using Hue.UI.SupportClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ThemeHubSection : UserControl
    {
        private ObservableCollection<IHueTheme> themeCollection = new ObservableCollection<IHueTheme>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ThemeHubSection()
        {
            this.InitializeComponent();

            // Initialize themes
            foreach (var theme in ThemeManager.Instance.Themes)
            {
                themeCollection.Add(theme);
            }

            // Add "create new theme" model to bottom
            themeCollection.Add(new NewThemeModel());

            ThemeListView.ItemsSource = themeCollection;
        }

        private void ThemeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeListView.SelectedItem == null)
            {
                return;
            }

            IHueTheme theme = ThemeListView.SelectedItem as IHueTheme;
            ThemeListView.SelectedItem = null;

            // Apply theme
            ThemeManager.Instance.ApplyThemeAsync(theme as HueTheme);

            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(ThemePage), theme);
        }
    }
}
