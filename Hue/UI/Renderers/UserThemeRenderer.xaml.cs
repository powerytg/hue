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

namespace Hue.UI.Renderers
{
    public sealed partial class UserThemeRenderer : UserControl
    {
        public static readonly DependencyProperty ThemeSourceProperty = DependencyProperty.Register(
        "ThemeSource",
        typeof(HueTheme),
        typeof(UserThemeRenderer),
        new PropertyMetadata(null, OnThemeSourcePropertyChanged));

        public HueTheme ThemeSource
        {
            get { return (HueTheme)GetValue(ThemeSourceProperty); }
            set { SetValue(ThemeSourceProperty, value); }
        }

        private static void OnThemeSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (UserThemeRenderer)sender;
            target.OnThemeSourceChanged();
        }

        private void OnThemeSourceChanged()
        {
            NameLabel.Text = ThemeSource.Name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserThemeRenderer()
        {
            this.InitializeComponent();

            ThemeManager.Instance.ThemeChanged += OnThemeChanged;
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            var target = sender as HueTheme;
            if (target != ThemeSource)
            {
                return;
            }

            NameLabel.Text = ThemeSource.Name;
        }
    }
}
