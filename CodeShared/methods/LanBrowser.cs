using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace CodeShared.methods
{
    public class LanBrowser
    {
        public async System.Threading.Tasks.Task GetListLanHostObjectAsync()
        {
            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/lan/browser/pub", null);
        }
        public async System.Threading.Tasks.Task GetLanInterfacesAsync()
        {
            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/lan/browser/interfaces", null);
        }
    }
}


