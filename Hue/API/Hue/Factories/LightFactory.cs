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
    public class LightFactory
    {
        public static void UpdateLightWithJObject(Light light, JObject json)
        {
            try
            {
                // Name
                JToken nameToken;
                if(json.TryGetValue("name", out nameToken))
                {
                    light.Name = json["name"].ToString();
                }

                // State
                JToken stateToken;
                if (json.TryGetValue("state", out stateToken))
                {
                    JObject stateJson = (JObject)json["state"];
                    light.IsOn = bool.Parse(stateJson["on"].ToString());
                    light.Brightness = int.Parse(stateJson["bri"].ToString());
                    light.Hue = int.Parse(stateJson["hue"].ToString());
                    light.Saturation = int.Parse(stateJson["sat"].ToString());

                    JArray coordArray = (JArray)stateJson["xy"];
                    light.X = double.Parse(coordArray[0].ToString());
                    light.Y = double.Parse(coordArray[1].ToString());

                    light.Temperature = int.Parse(stateJson["ct"].ToString());
                    light.AlertEffect = stateJson["alert"].ToString();
                    light.Effect = stateJson["effect"].ToString();

                    light.ColorMode = stateJson["colormode"].ToString();
                    light.IsReachable = bool.Parse(stateJson["reachable"].ToString());
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
