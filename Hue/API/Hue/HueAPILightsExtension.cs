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
        public async Task<bool> SetLightStateAsync(string lightId, object attrs)
        {
            var url = BaseUrl + "/lights/" + lightId + "/state";
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

        public async Task<bool> SetLightAttributesAsync(string lightId, object attrs)
        {
            var url = BaseUrl + "/lights/" + lightId;
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
