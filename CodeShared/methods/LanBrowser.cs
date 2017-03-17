using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace CodeShared.methods
{
    public class LanBrowser
    {
        public requests.configuration.lan_browser.LanHostObject getListLanHostObject()
        {
            string JsonResponse = HTTP_Request.HTTP_GETAsync("/api/v3/lan/browser/pub", null);

            requests.configuration.lan_browser.LanHostObject response = JsonConvert.DeserializeObject<requests.configuration.lan_browser.LanHostObject>(JsonResponse);
            if (response.success == "true")
            {
                return response;
            }
            return null;
        }
        public requests.configuration.lan_browser.LanInterfaces getLanInterfaces()
        {
            string JsonResponse = HTTP_Request.HTTP_GETAsync("/api/v3/lan/browser/interfaces", null);

            requests.configuration.lan_browser.LanInterfaces response = JsonConvert.DeserializeObject<requests.configuration.lan_browser.LanInterfaces>(JsonResponse);

            if (response.success == "true")
            {
                return response;
            }
            return null;
        }
    }
}

}
