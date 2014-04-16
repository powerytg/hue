using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;

namespace HueSaturation.API.UPNP
{
    /// <summary>
    /// 
    /// Part of code originates from the following sources (and big thanks to the authors!):
    /// 
    /// @see http://blogs.msdn.com/b/andypennell/archive/2011/08/24/attempting-upnp-on-windows-phone-7-5-mango-part-1-ssdp-discovery.aspx
    /// Also https://github.com/cDima/Hue/blob/master/Hue/UPnP.cs
    /// Also http://www.johnthom.com/implementing-ssdp-search-in-windows-phone-8-using-winrt/
    /// 
    /// </summary>
    public class DeviceFinder
    {
        public List<string> DiscoveredUrls { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DeviceFinder()
        {
            DiscoveredUrls = new List<string>();
        }

        /// <summary>
        /// @see http://www.johnthom.com/implementing-ssdp-search-in-windows-phone-8-using-winrt/
        /// </summary>
        public async Task DiscoverDevices()
        {
            int timeout = 3000;
            var remoteIp = "239.255.255.250";
            var remoteIpHostName = new Windows.Networking.HostName(remoteIp);
            var port = "1900";

            string query = "M-SEARCH * HTTP/1.1\r\n" +
                         "HOST: 239.255.255.250:1900\r\n" +
                         "ST: upnp:rootdevice\r\n" +
                         "MAN: \"ssdp:discover\"\r\n" +
                         "MX: " + timeout.ToString() + "\r\n\r\n";

            var buffer = Encoding.UTF8.GetBytes(query);

            using (var socket = new DatagramSocket())
            {
                socket.MessageReceived += (sender, args) => {
                    Task.Run(() => {
                        using (var reader = args.GetDataReader())
                        {
                            byte[] respBuff = new byte[reader.UnconsumedBufferLength];
                            reader.ReadBytes(respBuff);
                            string response = Encoding.UTF8.GetString(respBuff, 0, respBuff.Length).ToLower();
                            response.Trim('\0');

                            ProcessSSDPResponse(response);
                        }
                    });
                };

                await socket.BindEndpointAsync(null, "");
                socket.JoinMulticastGroup(remoteIpHostName);
                
                using (var stream = await socket.GetOutputStreamAsync(remoteIpHostName, port))
                {
                    await stream.WriteAsync(buffer.AsBuffer());
                }
                
                // Execute within timeout
                await Task.Delay(timeout);
            }
        }

        protected void ProcessSSDPResponse(String response)
        {
            // Extract out the IP addresses
            try
            {
                var url = response.Substring(response.ToLower().IndexOf("location:", System.StringComparison.Ordinal) + 9);
                url = url.Substring(0, url.IndexOf("\r", System.StringComparison.Ordinal)).Trim();
                Debug.WriteLine(url);

                if (!DiscoveredUrls.Contains(url))
                {
                    DiscoveredUrls.Add(url);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        
    }
}
