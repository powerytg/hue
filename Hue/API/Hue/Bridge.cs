using Hue.API.Hue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueSaturation.API.Hue
{
    public class Bridge
    {
        public string BridgeId { get; set; }
        public string IPAddress { get; set; }
        public string Name { get; set; }

        public string Version { get; set; }
        public bool HasSoftwareUpdate { get; set; }
        public string SoftwareUpdateText { get; set; }
        public string SoftwareUpdateUrl { get; set; }

        public List<string> WhiteList { get; set; }

        public List<Light> LightList { get; set; }
        public Dictionary<string, Light> LightCache { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Bridge()
        {
            LightList = new List<Light>();
            LightCache = new Dictionary<string, Light>();
            WhiteList = new List<string>();
        }

        // Events
        public EventHandler BridgePropertyChanged;

        public void InvalidateBridgeProperties()
        {
            if (BridgePropertyChanged != null)
            {
                BridgePropertyChanged(this, null);
            }
        }
    }
}
