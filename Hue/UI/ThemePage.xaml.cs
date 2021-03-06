﻿using Hue.API.Hue.Themes;
using Hue.API.Media;
using Hue.Common;
using Hue.UI.Renderers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Hue.UI
{
    public sealed partial class ThemePage : Page
    {
        private NavigationHelper navigationHelper;
        private HueTheme theme;

        public ObservableCollection<HSBColor> localColorList = new ObservableCollection<HSBColor>();

        public ThemePage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            // Get theme object
            if(e.Parameter != null)
            {
                theme = e.Parameter as HueTheme;
            }
            else
            {
                theme = new HueTheme();
                theme.Name = "New Theme";
            }

            if (theme.IsSystemTheme)
            {
                EditNameButton.Visibility = Visibility.Collapsed;
                RevertSystemThemeButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Collapsed;
            }
            else 
            {
                EditNameButton.Visibility = Visibility.Visible;
                RevertSystemThemeButton.Visibility = Visibility.Collapsed;
                DeleteButton.Visibility = Visibility.Visible;
            }

            TitleLabel.Text = theme.Name;

            // Make a clone of colors
            foreach (var color in theme.ColorList)
            {
                localColorList.Add(color.Clone());
            }

            ColorListView.ItemsSource = localColorList;

            // Events
            ColorRenderer.ColorChanged += OnThemeColorChanged;
            ColorRenderer.ColorDeleted += OnThemeColorDeleted;
            ThemeManager.Instance.ThemeChanged += OnThemeChanged;
        }

        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ColorRenderer.ColorChanged -= OnThemeColorChanged;
            ColorRenderer.ColorDeleted -= OnThemeColorDeleted;

            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void EditNameButton_Click(object sender, RoutedEventArgs e)
        {
            ThemeNameEditor.ThemeSource = null;
            ThemeNameEditor.ThemeSource = theme;
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void OnThemeColorChanged(object sender, EventArgs e)
        {
            if (NotificationView.Visibility == Visibility.Visible)
            {
                return;
            }

            bool isDirty = false;
            if (localColorList.Count != theme.ColorList.Count)
            {
                isDirty = true;
            }
            else
            {
                for (int i = 0; i < localColorList.Count; i++)
                {
                    if (!localColorList[i].IsEqualToColor(theme.ColorList[i]))
                    {
                        isDirty = true;
                        break;
                    }
                }
            }

            if (isDirty)
            {
                ShowNotificationView();
            }
            
        }

        private void OnThemeColorDeleted(object sender, EventArgs e)
        {
            var color = (HSBColor) sender;
            if (localColorList.Contains(color))
            {
                localColorList.Remove(color);

                OnThemeColorChanged(this, null);
            }
        }

        private void ShowNotificationView() 
        {
            NotificationView.Visibility = Visibility.Visible;
        }

        private void hideNotificationView()
        {
            NotificationView.Visibility = Visibility.Collapsed;
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyThemeAsync();
            hideNotificationView();
        }

        private void RevertButton_Click(object sender, RoutedEventArgs e)
        {
            RevertTheme();
            hideNotificationView();
        }

        private async void ApplyThemeAsync()
        {
            theme.ColorList.Clear();
            foreach (var color in localColorList)
            {
                theme.ColorList.Add(color.Clone());
            }

            // Save to file
            await ThemeManager.Instance.UpdateThemeAsync(theme);

            // Apply light colors
            await ThemeManager.Instance.ApplyThemeAsync(theme);
        }
            

        private void RevertTheme()
        {
            localColorList.Clear();
            foreach (var color in theme.ColorList)
            {
                localColorList.Add(color.Clone());
            }
        }

        private void OnThemeChanged(object sender, EventArgs e)
        {
            var target = sender as HueTheme;
            if (target != theme)
            {
                return;
            }

            TitleLabel.Text = theme.Name;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var msg = new MessageDialog("Are you sure to delete this theme?");
            msg.Commands.Add(new UICommand("Yes", delegate(IUICommand command)
            {
                DeleteThemeAsync();
            }));

            msg.Commands.Add(new UICommand("No"));
            msg.ShowAsync();
        }

        private async void DeleteThemeAsync()
        {
            await ThemeManager.Instance.DeleteThemeAsync(theme);

            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void RevertSystemThemeButton_Click(object sender, RoutedEventArgs e)
        {
            RevertToSystemThemeAsync();
        }

        private async void RevertToSystemThemeAsync()
        {
            await ThemeManager.Instance.RevertThemeAsync(theme);
            RevertTheme();
            hideNotificationView();

            await ThemeManager.Instance.ApplyThemeAsync(theme);
        }

        private void AddColorButton_Click(object sender, RoutedEventArgs e)
        {
            var newColor = HSBColor.FromColor(Color.FromArgb(0xff, 0xec, 0xcd, 0x67));
            localColorList.Add(newColor);

            OnThemeColorChanged(this, null);
        }

    }
}
