using Hue.API.Hue.Themes;
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
    public sealed partial class ThemeChangeNameView : UserControl
    {
        private string originalName;

        public static readonly DependencyProperty ThemeSourceProperty = DependencyProperty.Register(
       "ThemeSource",
       typeof(HueTheme),
       typeof(ThemeChangeNameView),
       new PropertyMetadata(null, OnThemeSourcePropertyChanged));

        public HueTheme ThemeSource
        {
            get { return (HueTheme)GetValue(ThemeSourceProperty); }
            set { SetValue(ThemeSourceProperty, value); }
        }

        private static void OnThemeSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (ThemeChangeNameView)sender;
            target.OnThemeSourceChanged();
        }

        private void OnThemeSourceChanged()
        {
            if (ThemeSource == null)
            {
                return;
            }

            NameInput.Text = ThemeSource.Name;
            originalName = ThemeSource.Name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ThemeChangeNameView()
        {
            this.InitializeComponent();
        }

        private void NameInput_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (NameInput.Text.Trim().Length == 0)
            {
                NameInput.Text = originalName;
            }
            else 
            {
                UpdateThemeNameAsync(NameInput.Text.Trim());
            }
        }

        private async void UpdateThemeNameAsync(string newName)
        {
            ThemeSource.Name = newName;
            await ThemeManager.Instance.UpdateThemeAsync(ThemeSource);

            ThemeManager.Instance.InvalidateTheme(ThemeSource);
        }

    }
}
