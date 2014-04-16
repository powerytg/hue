using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using HueSaturation.API.UPNP;

namespace HueSaturation.API.Hue
{
    public class BridgeManager
    {
        

        public Bridge CurrentBridge { get; set; }
        public List<Bridge> DiscoveredBridges { get; set; }

        public BridgeManager()
        {
            
        }

        
    }
}
