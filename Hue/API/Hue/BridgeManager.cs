using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using HueSaturation.API.UPNP;
using Hue.API.Hue;

namespace HueSaturation.API.Hue
{
    public class BridgeManager
    {
        public Bridge CurrentBridge { get; set; }
        public List<Bridge> DiscoveredBridges { get; set; }

        private static volatile BridgeManager instance;
        private static object syncRoot = new Object();

        // Events
        public EventHandler LightsOnOffStateChanged;
        public EventHandler BridgePropertyChanged;
        public EventHandler LightPropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        private BridgeManager() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public static BridgeManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new BridgeManager();
                    }
                }

                return instance;
            }
        }

        public async Task<bool> TurnOffAllLightsAsync()
        {
            foreach (var light in BridgeManager.Instance.CurrentBridge.LightList)
            {
                var attrs = new { on = false };
                await HueAPI.Instance.SetLightStateAsync(light.LightId, attrs);

                light.IsOn = false;
                InvalidateLightProperties(light);
            }

            InvalidateAllLightsOnOffState();

            return true;
        }

        public async Task<bool> TurnOnAllLightsAsync()
        {
            foreach (var light in BridgeManager.Instance.CurrentBridge.LightList)
            {
                var attrs = new { on = true };
                await HueAPI.Instance.SetLightStateAsync(light.LightId, attrs);

                light.IsOn = true;
                InvalidateLightProperties(light);
            }

            InvalidateAllLightsOnOffState();

            return true;
        }

        public void InvalidateAllLightsOnOffState()
        {
            if (LightsOnOffStateChanged != null)
            {
                LightsOnOffStateChanged(CurrentBridge, null);
            }
        }


        public void InvalidateBridgeProperties()
        {
            if (BridgePropertyChanged != null)
            {
                BridgePropertyChanged(CurrentBridge, null);
            }
        }

        public void InvalidateLightProperties(Light light)
        {
            if (LightPropertyChanged != null)
            {
                LightPropertyChanged(light, null);
            }
        }

        public int GetActiveLightCount()
        {
            int lightCount = CurrentBridge.LightList.Count;
            if (lightCount == 0)
            {
                return 0;
            }

            int onCount = 0;
            foreach (var light in CurrentBridge.LightList)
            {
                if (light.IsOn)
                {
                    onCount++;
                }
            }

            return onCount;
        }
    }
}
