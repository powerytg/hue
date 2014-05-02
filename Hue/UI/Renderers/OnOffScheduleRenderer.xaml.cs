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
    public sealed partial class OnOffScheduleRenderer : ScheduleRendererBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnOffScheduleRenderer()
        {
            this.InitializeComponent();
        }

        protected override void OnScheduleSourceChanged()
        {
            base.OnScheduleSourceChanged();

            NameLabel.Text = ScheduleSource.Name;

            if (ScheduleSource.LocalTime != null)
            {
                TimeLabelContent.Text = ScheduleSource.LocalTime;
            }
            else
            {
                TimeLabelContent.Text = "Select Time";
            }

            if (ScheduleSource.ScheduleId == null)
            {
                EnabledCheckBox.IsChecked = false;
            }
            else
            {
                EnabledCheckBox.IsChecked = true;
            }
        }

        private void TimeLabel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowTimePickerAsync();            
        }

        private async void ShowTimePickerAsync()
        {
            var picker = new TimePickerFlyout();
            picker.TimePicked += (sender, e) =>
            {
                TimeLabelContent.Text = picker.Time.ToString();
                ScheduleSource.SetLocalTimeWithTimeSpan(picker.Time);

                if (EnabledCheckBox.IsChecked == false)
                {
                    EnabledCheckBox.IsChecked = true;
                }
                else
                {
                    UpdateScheduleAsync();
                }

            };

            picker.Closed += (sender, e) =>
            {
                if (EnabledCheckBox.IsChecked == true && ScheduleSource.LocalTime == null)
                {
                    EnabledCheckBox.IsChecked = false;
                }
            };

            await picker.ShowAtAsync(TimeLabel);
        }

        private void EnabledCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Auto show time picker if no time specified
            if(ScheduleSource.LocalTime == null)
            {
                ShowTimePickerAsync();
            }
            else if(ScheduleSource.ScheduleId == null)
            {
                CreateScheduleAsync();
            }
        }

        private void EnabledCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if(ScheduleSource.ScheduleId != null)
            {
                DeleteScheduleAsync();
            }
        }

        private async void CreateScheduleAsync()
        {
            if (ScheduleSource.ScheduleId != null)
            {
                return;
            }

            var attrs = new { name = ScheduleSource.Name, description = ScheduleSource.Description, command = ScheduleSource.Command, localtime = ScheduleSource.LocalTime };
            
            // scheduleId can be null
            string scheduleId = await HueAPI.Instance.CreateScheduleAsync(attrs);
            
            ScheduleSource.ScheduleId = scheduleId;
        }

        private async void UpdateScheduleAsync()
        {
            if (ScheduleSource.ScheduleId == null)
            {
                return;
            }

            var attrs = new { localtime = ScheduleSource.LocalTime };
            await HueAPI.Instance.CreateScheduleAsync(attrs);               
        }

        private async void DeleteScheduleAsync()
        {
            if (ScheduleSource.ScheduleId == null)
            {
                return;
            }

            await HueAPI.Instance.DeleteScheduleAsync(ScheduleSource.ScheduleId);
            ScheduleSource.ScheduleId = null;
        }
    }
}
