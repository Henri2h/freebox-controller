using freebox_controller;
using System;
using System.IO;

namespace freebox_controller
{
    class Program
    {
        public static string host = "http://mafreebox.freebox.fr";

        public static string appVersion = "0.0.4";

        static FreeboxControl freeboxController = new FreeboxControl();

        public static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    setWifiOff();
                }
                catch
                {
                    Console.WriteLine("error");
                }
            }
           // Console.ReadLine();
        }

        public static void setWifiOff()
        {


            HTTP_Request.setHost(host);

            Console.WriteLine("Starting...");
            //preparing login
            if (freeboxController.Authentification.exist() == false)
            {
                freeboxController.Authentification.registerApp("freebox controller", "fr.freebox.controller", appVersion);
                Console.WriteLine("Please change settings");
                Console.ReadLine();
            }
            else
            {
                freeboxController.Authentification.loginAuto();
                Console.WriteLine("Logged in");
            }
            freeboxController.Authentification.saveSettings();

            // set wifi on
            //Console.WriteLine("Wifi enabled : " + freeboxController.Wifi.setWifi(true));

            // lan browser
            /*
            requests.configuration.lan_browser.LanInterfaces interfaces = freeboxController.Configuration.Lan_Browser.getLanInterfaces();
            foreach (requests.configuration.lan_browser.LanInterfaces.Result te in interfaces.result)
            {
                Console.WriteLine("lan interface : " + te.name + " host_count : " + te.host_count);
            }
            Console.WriteLine();

            requests.configuration.lan_browser.LanHostObject objects = freeboxController.Configuration.Lan_Browser.getListLanHostObject();
            foreach (requests.configuration.lan_browser.LanHostObject.Result te in objects.result)
            {
                Console.WriteLine("Device : " + te.primary_name);
                Console.WriteLine("ID : " + te.l2ident.id + " type : " + te.l2ident.type);
                Console.WriteLine("last_activity : " + te.last_activity + " last_time_reachable : " + te.last_time_reachable);
                Console.WriteLine("reachable : " + te.reachable);
                Console.WriteLine("active : " + te.active);
                Console.WriteLine("persistent : " + te.persistent);
                if (te.l3connectivies != null)
                {
                    foreach (requests.configuration.lan_browser.LanHostObject.Result.L3connectivities test in te.l3connectivies)
                    {
                        Console.WriteLine("addr : " + test.addr + " af : " + test.af);
                    }
                }
                else
                {
                    Console.WriteLine("l3connectivities :  null");
                }*/
            bool enabled = true;
            while (true)
            {
                enabled = freeboxController.Wifi.getWifiInfo();
                if (enabled == true)
                {
                    Console.WriteLine("It is on !!! " + DateTime.Now.ToString());
                    enabled = freeboxController.Wifi.setWifi(false);
                    Console.WriteLine(" Now : " + enabled + Environment.NewLine);
                }
            }

        }


    }
}
