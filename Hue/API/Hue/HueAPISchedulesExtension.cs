using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public async Task<string> CreateScheduleAsync(object attrs)
        {
            var url = BaseUrl + "/schedules";
            var bodyJson = JsonConvert.SerializeObject(attrs);            

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.PostAsync(url, new StringContent(bodyJson));
                resp.EnsureSuccessStatusCode();

                var result = await resp.Content.ReadAsStringAsync();
                Debug.WriteLine(result);

                if (!result.Contains("success"))
                {
                    return null;
                }

                JArray resultArray = JArray.Parse(result);
                JObject idObject = (JObject)resultArray[0];
                string scheduleId = idObject["success"]["id"].ToString();

                return scheduleId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async Task<bool> UpdateScheduleAsync(string scheduleId, object attrs)
        {
            var url = BaseUrl + "/schedules/" + scheduleId;
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

        public async Task<bool> DeleteScheduleAsync(string scheduleId)
        {
            var url = BaseUrl + "/schedules/" + scheduleId;
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage resp = await client.DeleteAsync(url);
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
