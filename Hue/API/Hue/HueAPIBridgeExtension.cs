using Hue.API.Hue.Factories;
using HueSaturation.API.Hue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue
{
    public partial class HueAPI
    {
        public async void GetAllConfigurationsAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.GetAsync(BaseUrl);
                resp.EnsureSuccessStatusCode();

                // Try to parse JSON response
                var result = await resp.Content.ReadAsStringAsync();
                if (result.Contains("unauthorized user"))
                {
                    if(GetBridgeConfigurationsFailed != null)
                    {
                        GetBridgeConfigurationsFailed(this, null);
                    }

                    return;
                }

                Debug.WriteLine(result);

                // Do the parsing work
                BridgeFactory.UpdateBridgeWithJSON(BridgeManager.Instance.CurrentBridge, result);

                // Dispatch events
                if(GetBridgeConfigurationsComplete != null)
                {
                    GetBridgeConfigurationsComplete(this, null);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                if (GetBridgeConfigurationsFailed != null)
                {
                    GetBridgeConfigurationsFailed(this, null);
                }

                return;
            }
        }

        public async Task<bool> SetBridgeConfigurationsAsync(object attrs)
        {
            var url = BaseUrl + "/config";
            var bodyJson = JsonConvert.SerializeObject(attrs);
            Debug.WriteLine(bodyJson);

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.PutAsync(url, new StringContent(bodyJson));
                resp.EnsureSuccessStatusCode();

                var result = await resp.Content.ReadAsStringAsync();
                Debug.WriteLine(result);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;
        }
    }
}
