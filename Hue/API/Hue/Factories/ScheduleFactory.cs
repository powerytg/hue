using Hue.API.Hue;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue.Factories
{
    public class ScheduleFactory
    {
        public static void UpdateScheduleWithJObject(Schedule schedule, JObject json)
        {
            try
            {
                // Name
                JToken nameToken;
                if(json.TryGetValue("name", out nameToken))
                {
                    schedule.Name = json["name"].ToString();
                }

                // Desc
                JToken descToken;
                if (json.TryGetValue("description", out descToken))
                {
                    schedule.Description = json["description"].ToString();
                }

                // Command
                JToken commandToken;
                if(json.TryGetValue("command", out commandToken))
                {
                    schedule.CommandString = json["command"].ToString();

                    // Parse which app created this schedule
                    JObject commandObject = JObject.Parse(schedule.CommandString);
                    JToken addressToken;
                    if(commandObject.TryGetValue("address", out addressToken)){
                        string addressString = commandObject["address"].ToString();

                        // Remove the "/api" prefix
                        if (addressString.StartsWith("/api/"))
                        {
                            addressString = addressString.Substring(5);
                            var parts = addressString.Split('/');
                            schedule.AppName = parts[0];
                        }
                    }
                }

                // Time
                JToken timeToken;
                if (json.TryGetValue("localtime", out timeToken))
                {
                    string timeString = json["localtime"].ToString();
                    if (timeString.StartsWith("PT"))
                    {
                        // One time timer. Ignore this type
                        schedule.Type = ScheduleType.OneTime;
                    }
                    else if (timeString.StartsWith("W"))
                    {
                        // Recurring schedule
                        schedule.Type = ScheduleType.Recurring;

                        string[] parts = timeString.Split('/');
                        string occuringDays = parts[0];

                        // Remove the "T" char at beginning
                        string timeOfDay = parts[1].Substring(1);

                        schedule.LocalTime = timeOfDay;
                    }
                    else
                    {
                        schedule.Type = ScheduleType.Unknown;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static Schedule newAllOnSchedule()
        {
            Schedule schedule = new Schedule();
            schedule.Name = Schedule.DefaultAllOnScheduleName;
            schedule.Description = "Turn all the lights on";
            schedule.AppName = HueAPI.Instance.AppKey;
            schedule.Type = ScheduleType.Recurring;

            var address = "/api/" + HueAPI.Instance.AppKey + "/groups/0/action";
            var bodyParams = new { on = true };
            var commandBody = new { address = address, method = "PUT", body = bodyParams };
            schedule.Command = commandBody;

            return schedule;
        }

        public static Schedule newAllOffSchedule()
        {
            Schedule schedule = new Schedule();
            schedule.Name = Schedule.DefaultAllOffScheduleName;
            schedule.Description = "Turn all the lights off";
            schedule.AppName = HueAPI.Instance.AppKey;
            schedule.Type = ScheduleType.Recurring;
            
            var address = "/api/" + HueAPI.Instance.AppKey + "/groups/0/action";
            var bodyParams = new {on = false};
            var commandBody = new { address = address, method = "PUT", body = bodyParams };
            schedule.Command = commandBody;

            return schedule;
        }

    }
}
