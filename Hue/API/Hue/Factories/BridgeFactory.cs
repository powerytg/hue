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
    public class BridgeFactory
    {
        public static void UpdateBridgeWithJSON(Bridge bridge, string jsonString)
        {
            try
            {
                JObject json = JObject.Parse(jsonString);
                // Config section
                JToken configValue;
                if(json.TryGetValue("config", out configValue))
                {
                    UpdateBridgeConfigurationsWithJObject(bridge, (JObject)json["config"]);
                }

                // Light section
                JToken lightsValue;
                if (json.TryGetValue("lights", out lightsValue))
                {
                    UpdateBridgeLightsWithJObject(bridge, (JObject)json["lights"]);
                }

                // Schedule section
                JToken scheduleValue;
                if (json.TryGetValue("schedules", out scheduleValue))
                {
                    UpdateBridgeSchedulesWithJObject(bridge, (JObject)json["schedules"]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void UpdateBridgeConfigurationsWithJObject(Bridge bridge, JObject configJson)
        {
            try
            {
                bridge.Name = configJson["name"].ToString();

                // Version update info
                bridge.Version = configJson["swversion"].ToString();
                JToken updateValue;
                if (configJson.TryGetValue("swupdate", out updateValue))
                {
                    JObject updateJson = (JObject)configJson["swupdate"];
                    bridge.HasSoftwareUpdate = (updateJson["updatestate"].ToString() == "1");
                    bridge.SoftwareUpdateText = updateJson["text"].ToString();
                    bridge.SoftwareUpdateUrl = updateValue["url"].ToString();
                }

                // White list
                JToken whitelistValue;
                if (configJson.TryGetValue("whitelist", out whitelistValue))
                {
                    bridge.WhiteList.Clear();
                    JObject whitelistJson = (JObject)configJson["whitelist"];
                    IList<string> appKeys = whitelistJson.Properties().Select(p => p.Name).ToList();
                    bridge.WhiteList.AddRange(appKeys);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void UpdateBridgeLightsWithJObject(Bridge bridge, JObject json)
        {
            try
            {
                // Get light keys
                IList<string> lightKeys = json.Properties().Select(p => p.Name).ToList();

                bridge.LightList.Clear();
                bridge.LightCache.Clear();

                foreach(var lightId in lightKeys)
                {
                    Light light = new Light();
                    light.LightId = lightId;
                    bridge.LightList.Add(light);
                    bridge.LightCache[lightId] = light;

                    JObject lightJson = (JObject)json[lightId];
                    LightFactory.UpdateLightWithJObject(light, lightJson);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public static void UpdateBridgeSchedulesWithJObject(Bridge bridge, JObject json)
        {
            try
            {
                // Get schedule keys
                IList<string> scheduleKeys = json.Properties().Select(p => p.Name).ToList();
                bridge.ScheduleList.Clear();

                foreach (var scheduleId in scheduleKeys)
                {
                    Schedule schedule = new Schedule();
                    schedule.ScheduleId = scheduleId;
                    bridge.ScheduleList.Add(schedule);

                    JObject scheduleJson = (JObject)json[scheduleId];
                    ScheduleFactory.UpdateScheduleWithJObject(schedule, scheduleJson);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

    }
}
