using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace freebox_controller
{
    public static class requests
    {
        public class login
        {
            public class session
            {

                public class Request
                {
                    public string app_id;
                    public string password;
                }
            }
        }
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
            
            // for invallid password :

            public string error_code;
            public string msg;
            public string uid;
        }
        public class result
        {
            //request authorisation
            public string app_token;
            public string session_token;
            public string track_id;

            //track authorisation progress
            public string status;
            public string challenge;
            public permission permissions;
            //login
            public string logged_in;
        }

        //permission for the account
        public class permission
        {
            public bool settings = false;
            public bool contacts = false;
            public bool calls = false;
            public bool explorer = false;
            public bool downloader = false;
            public bool parental = false;
            public bool pvr = false;
        }
    }
}
