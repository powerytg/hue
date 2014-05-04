using Hue.API.Hue.Themes;
using Hue.API.Media;
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
using Windows.UI;
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
            UpdateThemeList();
            ThemeListView.ItemsSource = themeCollection;

            // Events
            ThemeManager.Instance.ThemeListChanged += OnThemeListChanged;
        }

        private void OnThemeListChanged(object sender, EventArgs e)
        {
            UpdateThemeList();
        }

        private void UpdateThemeList()
        {
            themeCollection.Clear();
            var systemThemes = new List<HueTheme>();
            var userThemes = new List<HueTheme>();

            foreach (var theme in ThemeManager.Instance.Themes)
            {
                if (theme.IsSystemTheme)
                {
                    systemThemes.Add(theme);
                }
                else
                {
                    userThemes.Add(theme);
                }
            }

            foreach (var theme in systemThemes)
            {
                themeCollection.Add(theme);
            }

            themeCollection.Add(new NewThemeModel());

            foreach (var theme in userThemes)
            {
                themeCollection.Add(theme);
            }

        }

        private void ThemeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeListView.SelectedItem == null)
            {
                return;
            }

            IHueTheme selectedOption = ThemeListView.SelectedItem as IHueTheme;
            ThemeListView.SelectedItem = null;

            if (selectedOption is NewThemeModel)
            {
                // Create a new theme
                CreateNewThemeAsync();
            }
            else
            {
                // Apply theme
                EditThemeAsync(selectedOption as HueTheme);
            }
                
           
        }

        private async void CreateNewThemeAsync()
        {
            var newTheme = await ThemeManager.Instance.CreateThemeAsync();
            await ThemeManager.Instance.ApplyThemeAsync(newTheme);

            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(ThemePage), newTheme);
        }

        private async void EditThemeAsync(HueTheme theme)
        {
            await ThemeManager.Instance.ApplyThemeAsync(theme as HueTheme);

            var frame = Window.Current.Content as Frame;
            frame.Navigate(typeof(ThemePage), theme);
        }

    }
}
