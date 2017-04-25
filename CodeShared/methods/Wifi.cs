using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShared.methods
{
    public class Wifi
    {
        public async System.Threading.Tasks.Task<bool> GetWifiInfoAsync()
        {
            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/wifi/config/", null);
            System.Diagnostics.Debug.WriteLine(JsonResponse);

            JObject response = JObject.Parse(JsonResponse);
            bool success = (bool)response["success"];
            bool enabled = (bool)response["result"]["enabled"];
            string mac_filter_state = (string)response["result"]["mac_filter_state"];

            return enabled;
        }
        public async System.Threading.Tasks.Task<bool> SetWifiAsync(bool enabled)
        {
            if (HTTP_Request.Fbx_Header != null && HTTP_Request.Fbx_Header != "")
            {
                JObject request = new JObject {
                { "enabled", enabled }
            };

                string JsonResponse = await HTTP_Request.HTTP_PUTAsync(Core.Host, "/api/v3/wifi/config/", request.ToString());
                JObject response = JObject.Parse(JsonResponse);
                bool success = (bool)response["success"];
                if (success)
                {
                    return (bool)response["result"]["enabled"];
                }

            }
            throw new Exception("No FBX Header given");
        }
    }
}
