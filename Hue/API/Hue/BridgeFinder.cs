using HueSaturation.API.UPNP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace HueSaturation.API.Hue
{
    public class BridgeFinder
    {
        public static string ProxyUPNPUrl = "http://www.meethue.com/api/nupnp";

        public async Task<List<Bridge>> SearchBridgesAsync()
        {
            List<Bridge> discoveredBridges = await SearchBridgesUsingProxyAsync();

            // Fall back to upnp method if failed
            if (discoveredBridges.Count == 0)
            {
                discoveredBridges = await SearchBridgesUsingUPNPAsync();
            }

            return discoveredBridges;
        }

        protected async Task<List<Bridge>> SearchBridgesUsingProxyAsync()
        {
            // First, we'll attempt to use the proxy upnp server provided by Hue
            var discoveredBridges = new List<Bridge>();

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.GetAsync(ProxyUPNPUrl);
                resp.EnsureSuccessStatusCode();

                // Try to parse JSON response
                var result = await resp.Content.ReadAsStringAsync();
                JsonArray bridgeArray = JsonArray.Parse(result);
                foreach (var bridgeValue in bridgeArray)
                {
                    var bridgeObject = bridgeValue.GetObject();

                    Bridge bridge = new Bridge();
                    bridge.BridgeId = bridgeObject.GetNamedString("id");
                    bridge.IPAddress = bridgeObject.GetNamedString("internalipaddress");

                    discoveredBridges.Add(bridge);

                    Debug.WriteLine("Found bridge {0} at {1}", bridge.BridgeId, bridge.IPAddress);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            
            return discoveredBridges;
        }

        public async Task<List<Bridge>> SearchBridgesUsingUPNPAsync()
        {
            var discoveredBridges = new List<Bridge>();
            DeviceFinder finder = new DeviceFinder();
            await finder.DiscoverDevices();

            foreach(var url in finder.DiscoveredUrls)
            {
                if (!url.EndsWith("/description.xml"))
                {
                    continue;
                }

                Bridge bridge = await BridgeFromDescriptionAsync(url);
                if (bridge != null)
                {
                    discoveredBridges.Add(bridge);
                }
            }

            return discoveredBridges;
        }

        protected async Task<Bridge> BridgeFromDescriptionAsync(string url)
        {
            Bridge bridge = null;

            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(3);
                HttpResponseMessage resp = await client.GetAsync(url);
                resp.EnsureSuccessStatusCode();

                // Try to parse JSON response
                var result = await resp.Content.ReadAsStringAsync();
                if (result.Contains("Philips hue bridge"))
                {
                    var ip = url.Replace("http://", "").Replace("/description.xml", "");
                    bridge = new Bridge();
                    bridge.IPAddress = ip;

                    Debug.WriteLine("Found bridge via UPNP: {0}", ip);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                bridge = null;
            }

            return bridge;
        }

    }
}
