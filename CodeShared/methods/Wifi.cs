using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShared.methods
{
    public class Wifi
    {
        public async System.Threading.Tasks.Task<bool> getWifiInfoAsync()
        {
            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v2/wifi/config/", null);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                if (response.result.enabled == "true")
                {
                    return true;
                }
            }
            return false;
        }
        public async System.Threading.Tasks.Task<bool> setWifiAsync(bool enabled)
        {
            requests.wifi.globalConfig.wifi authorisationRequest = new requests.wifi.globalConfig.wifi();
            authorisationRequest.enabled = enabled;

            //serializing
            string content = JsonConvert.SerializeObject(authorisationRequest);
            string JsonResponse = await HTTP_Request.HTTP_PUTAsync(Core.Host, "/api/v2/wifi/config/", content);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                if (response.result.enabled == "true")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
