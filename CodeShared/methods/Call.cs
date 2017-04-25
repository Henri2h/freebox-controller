using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeShared.methods
{
    public class Call
    {
        public async Task<List<CallEntry>> GetCallsAsync()
        {
            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/call/log/", null);
            JObject response = JObject.Parse(JsonResponse);

            bool success = (bool)response["success"];
            if (success)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CallEntry>>(response["result"].ToString());
            }
            return null;
        }

    }
    public class CallEntry
    {
        public string number { get; set; }
        public string type { get; set; }
        public int id { get; set; }
        public int duration { get; set; }
        public int datetime { get; set; }
        public int contact_id { get; set; }
        public int line_id { get; set; }
        public string name { get; set; }
        public bool NewCall { get; set; }
    }
}
