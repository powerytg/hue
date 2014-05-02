using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue
{
    public enum ScheduleType { OneTime, Recurring, Unknown };

    public class Schedule
    {
        public static string DefaultAllOnScheduleName = "Lights On";
        public static string DefaultAllOffScheduleName = "Lights Off";

        public static int Sunday = 1;
        public static int Saturday = 2;
        public static int Friday = 4;
        public static int Thursday = 8;
        public static int Wednesday = 16;
        public static int Tuesday = 32;
        public static int Monday = 64;

        public string AppName { get; set; }

        public string ScheduleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ScheduleType Type { get; set; }

        public string LocalTime { get; set; }

        public string CommandString { get; set; }
        public object Command { get; set; }

        /// <summary>
        /// A number between 1 to 127.
        /// 
        /// 1 = Sunday        
        /// 2 = Saturday
        /// 4 = Friday
        /// 8 = Thursday
        /// 16 = Wednesday
        /// 32 = Tuesday
        /// 64 = Monday
        /// 
        /// RecurringDays is the sum of these numbers. For example, 
        /// 
        /// 16 + 8 + 4 = 28 means that Wednesday and Thursday and Friday
        /// </summary>
        
        public int RecurringDays { get; set; }

        public bool IsSupportedSchedule
        {
            get
            {
                return AppName == HueAPI.Instance.AppKey;
            }
        }

        public void SetLocalTimeWithTimeSpan(TimeSpan time)
        {
            var ts = time.ToString();
            LocalTime = "W127/T" + ts;
        }

    }
}
