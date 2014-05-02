using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue
{
    public partial class BridgeManager
    {
        public bool HasSupportedSchedules
        {
            get
            {
                foreach (var schedule in CurrentBridge.ScheduleList)
                {
                    if (schedule.IsSupportedSchedule)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public async void DeleteScheduleAsync(Schedule schedule)
        {
            if (schedule.ScheduleId == null)
            {
                return;
            }

            await HueAPI.Instance.DeleteScheduleAsync(schedule.ScheduleId);

            if (CurrentBridge.ScheduleList.Contains(schedule))
            {
                CurrentBridge.ScheduleList.Remove(schedule);

                if (UnsupportedScheduleRemoved != null)
                {
                    UnsupportedScheduleRemoved(schedule, null);
                }
            }
        }
    }
}
