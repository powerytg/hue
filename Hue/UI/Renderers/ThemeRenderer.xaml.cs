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
    public sealed partial class ThemeRenderer : UserControl
    {
        // Theme property
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(
        "Theme", 
        typeof(HueTheme),
        typeof(ThemeRenderer), null);

        public HueTheme Theme
        {
            get { return (HueTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        // Constructor
        public ThemeRenderer()
        {
            this.InitializeComponent();
        }
    }
}
