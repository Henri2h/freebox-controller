using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CodeShared.methods
{
    public class LanBrowser
    {
        public async System.Threading.Tasks.Task<requests.configuration.lan_browser.LanHostObject> GetListLanHostObjectAsync()
        {
            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/lan/browser/pub", null);

            requests.configuration.lan_browser.LanHostObject response = JsonConvert.DeserializeObject<requests.configuration.lan_browser.LanHostObject>(JsonResponse);
            if (response.success == "true")
            {
                return response;
            }
            return null;
        }
        public async System.Threading.Tasks.Task<requests.configuration.lan_browser.LanInterfaces> GetLanInterfacesAsync()
        {
            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/lan/browser/interfaces", null);

            requests.configuration.lan_browser.LanInterfaces response = JsonConvert.DeserializeObject<requests.configuration.lan_browser.LanInterfaces>(JsonResponse);

            if (response.success == "true")
            {
                return response;
            }
            return null;
        }
    }
}


