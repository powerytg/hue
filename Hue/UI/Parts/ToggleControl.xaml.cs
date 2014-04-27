using System;
using System.Collections.Generic;
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

namespace Hue.UI.Parts
{
    public sealed partial class ToggleControl : UserControl
    {
        private SolidColorBrush onBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x0e, 0x95, 0xec));
        private SolidColorBrush offBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x58, 0x5b, 0x60));

        public EventHandler ValueChanged;

        public static readonly DependencyProperty IsOnProperty = DependencyProperty.Register(
        "IsOn",
        typeof(bool),
        typeof(ToggleControl),
        new PropertyMetadata(false, OnIsOnPropertyChanged));

        public bool IsOn
        {
            get { return (bool)GetValue(IsOnProperty); }
            set { SetValue(IsOnProperty, value); }
        }

        private static void OnIsOnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (ToggleControl)sender;
            target.OnIsOnChanged();
        }

        private void OnIsOnChanged()
        {
            if (IsOn)
            {
                OnButton.Foreground = onBrush;
                OffButton.Foreground = offBrush;
            }
            else
            {
                OnButton.Foreground = offBrush;
                OffButton.Foreground = onBrush;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ToggleControl()
        {
            this.InitializeComponent();
            
        }

        private void OnButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!IsOn)
            {
                IsOn = true;

                if (ValueChanged != null)
                {
                    ValueChanged(this, null);
                }
            }
        }

        private void OffButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsOn)
            {
                IsOn = false;

                if (ValueChanged != null)
                {
                    ValueChanged(this, null);
                }
            }
        }
    }
}
