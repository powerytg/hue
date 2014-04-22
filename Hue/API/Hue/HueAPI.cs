using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Hue.API.Hue
{
    public class HueAPI
    {
        public string DeviceType { get; set; }
        public string AppKey { get; set; }
        public string BridgeIp { get; set; }

        private static volatile HueAPI instance;
        private static object syncRoot = new Object();

        private HueAPI() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public static HueAPI Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new HueAPI();
                    }
                }

                return instance;
            }
        }

        public string BaseUrl
        {
            get
            {
                return "http://" + BridgeIp + "/api/" + AppKey;
            }
        }

        public async Task<bool> TestConnectAsync()
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
                    return false;
                }

                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }

            return true;
        }

        public async Task<bool> RegisterUserAsync()
        {
            var url = "http://" + BridgeIp + "/api";
            var body = new { devicetype = DeviceType, username = AppKey };
            var bodyJson = JsonConvert.SerializeObject(body);
            Debug.WriteLine(bodyJson);
            
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.PostAsync(url, new StringContent(bodyJson));
                resp.EnsureSuccessStatusCode();

                // Try to parse JSON response
                var result = await resp.Content.ReadAsStringAsync();
                Debug.WriteLine(result);
                if (result.Contains("101"))
                {
                    return false;
                }
                else if(result.Contains("success"))
                {
                    return true;
                }

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
