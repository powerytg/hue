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

namespace Hue.UI.Renderers
{
    public sealed partial class UnsupportedScheduleRenderer : ScheduleRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UnsupportedScheduleRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnScheduleSourceChanged()
        {
            base.OnScheduleSourceChanged();

            if (ScheduleSource == null)
            {
                return;
            }

            if (ScheduleSource.Name.Length > 0)
            {
                NameLabel.Visibility = Visibility.Visible;
                NameLabel.Text = ScheduleSource.Name;
            }
            else
            {
                NameLabel.Visibility = Visibility.Collapsed;
            }

            if (ScheduleSource.Description.Length > 0)
            {
                DescLabel.Visibility = Visibility.Visible;
                DescLabel.Text = ScheduleSource.Description;
            }
            else
            {
                DescLabel.Visibility = Visibility.Collapsed;
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            BridgeManager.Instance.DeleteScheduleAsync(ScheduleSource);
        }

    }
}
