using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace freebox_controller
{
    public static class requests
    {
        public class authorisation
        {
            public string app_id;
            public string app_name;
            public string app_version;
            public string device_name;
        }
        public class response
        {
            public string success;
            public result result;
        }
        public class result {
            //request authorisation
            public string app_token;
            public string track_id;

            //track authorisation progress
            public string status;
            public string challenge;

            //login
            public string logged_in;
        }
    }
}
