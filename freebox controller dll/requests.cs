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
        public class wifi
        {
            public class globalConfig
            {
                public class update
                {
                    public string enabled;
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
        public class configuration
        {


            public class lan_browser
            {
                public class LanInterfaces : serverResponse
                {
                    public IEnumerable<Result> result;

                    public class Result
                    {
                        public string name;
                        public int host_count;
                    }
                }

                public class LanHostObject : serverResponse
                {
                    public IEnumerable<Result> result;
                    public class Result
                    {
                        //LanHost
                        //public string id;
                        public string primary_name;
                        public enum host_type
                        {
                            workstation,
                            laptop,
                            smartphone,
                            tablet,
                            printer,
                            vg_console,
                            television,
                            nas,
                            ip_camera,
                            ip_phone,
                            freebox_player,
                            freebox_hd,
                            networking_device,
                            multimedia_device,
                            other
                        }
                        public bool primary_name_manual;

                        public L2ident l2ident;
                        public class L2ident
                        {
                            public string id;
                            public string type;
                        }

                        public string vendor_name;
                        public bool persistent;
                        public bool reachable;
                        public string last_time_reachable;
                        public static bool active;
                        public string last_activity;

                        public IEnumerable<Names> names;
                        public class Names
                        {
                            public string name;
                            public string source;
                        }

                        public IEnumerable<L3connectivities> l3connectivies;
                        public class L3connectivities
                        {
                            //LanHostL3Connectivity
                            public string addr;
                            public string af;
                        }

                    }
                }

            }
        }

        public class serverResponse
        {
            public string success;

            // for invallid password :

            public string error_code;
            public string msg;
            public string uid;
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

            // wifi
            public string enabled;
            public string mac_filter_state;
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
