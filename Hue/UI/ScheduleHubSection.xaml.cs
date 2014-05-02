using Hue.API.Hue;
using Hue.API.Hue.Factories;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hue.UI
{
    public sealed partial class ScheduleHubSection : UserControl
    {
        private ObservableCollection<Schedule> scheduleList;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScheduleHubSection()
        {
            this.InitializeComponent();

            UpdateDisplayList();

            // Events
            HueAPI.Instance.GetBridgeConfigurationsComplete += OnSchedulesUpdated;
            ScheduleToggle.ValueChanged += OnScheduleToggled;
            BridgeManager.Instance.UnsupportedScheduleRemoved += OnScheduleDeleted;
        }

        private void OnSchedulesUpdated(object sender, EventArgs e)
        {
            UpdateDisplayList();
        }

        private void UpdateDisplayList()
        {
            Schedule onSchedule = null;
            Schedule offSchedule = null;

            // Scan through schedules
            scheduleList = new ObservableCollection<Schedule>();
            var unsupportedSchedules = new List<Schedule>();
            foreach(var schedule in BridgeManager.Instance.CurrentBridge.ScheduleList)
            {
                if (schedule.IsSupportedSchedule)
                {
                    if (schedule.Name == Schedule.DefaultAllOnScheduleName)
                    {
                        // Lights on schedule
                        onSchedule = schedule;
                    }
                    else if (schedule.Name == Schedule.DefaultAllOffScheduleName)
                    {
                        // Lights off schedule
                        offSchedule = schedule;
                    }
                }
                else
                {
                    unsupportedSchedules.Add(schedule);
                }
            }

            // Always make sure the default supported schedules are on top
            if (onSchedule == null)
            {
                onSchedule = ScheduleFactory.newAllOnSchedule();
            }

            scheduleList.Add(onSchedule);

            if (offSchedule == null)
            {
                offSchedule = ScheduleFactory.newAllOffSchedule();
            }

            scheduleList.Add(offSchedule);

            // Append the other unsupported schedules
            foreach (var s in unsupportedSchedules)
            {
                scheduleList.Add(s);
            }

            ScheduleListView.ItemsSource = scheduleList;

            ScheduleToggle.IsEnabled = true;
            
            // Determine the on/off state of the toggle
            if(BridgeManager.Instance.HasSupportedSchedules) {
                ScheduleToggle.IsOn = true;
                EnableScheduleListView();
            }
            else{
                ScheduleToggle.IsOn = false;
                DisableScheduleListView();
            }
        }

        private void OnScheduleDeleted(object sender, EventArgs e)
        {
            Schedule target = (Schedule)sender;
            if (scheduleList.Contains(target))
            {
                scheduleList.Remove(target);
            }
        }

        private void EnableScheduleListView()
        {
            var sb = new Storyboard();
            sb.Duration = new Duration(TimeSpan.FromSeconds(0.2));

            ScheduleListViewTranslate.Y = this.ActualHeight;

            DoubleAnimation yAnimation = new DoubleAnimation();
            yAnimation.Duration = sb.Duration;
            yAnimation.To = 0;
            yAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(yAnimation, ScheduleListView);
            Storyboard.SetTargetProperty(yAnimation, "(UIElement.RenderTransform).(TranslateTransform.Y)");
            sb.Children.Add(yAnimation);

            DoubleAnimation opactiyAnimation = new DoubleAnimation();
            opactiyAnimation.Duration = sb.Duration;
            opactiyAnimation.To = 1;
            opactiyAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseIn };
            Storyboard.SetTarget(opactiyAnimation, ScheduleListView);
            Storyboard.SetTargetProperty(opactiyAnimation, "Opacity");
            sb.Children.Add(opactiyAnimation);
            sb.Begin();

            ScheduleListView.IsHitTestVisible = true;
        }

        private void DisableScheduleListView() 
        {
            ScheduleListView.Opacity = 0;
            ScheduleListView.IsHitTestVisible = false;
        }

        private void OnScheduleToggled(object sender, EventArgs e)
        {
            if(ScheduleToggle.IsOn)
            {
                EnableScheduleListView();
            }
            else
            {
                DisableScheduleListView();
                DeleteAllSchedulesAsync();
            }
        }

        private async void DeleteAllSchedulesAsync()
        {
            // Only delete supported schedules. 
            foreach (var schedule in scheduleList)
            {
                if (schedule.IsSupportedSchedule)
                {
                    await HueAPI.Instance.DeleteScheduleAsync(schedule.ScheduleId);
                    schedule.ScheduleId = null;
                }
            }
        }
    }
}
