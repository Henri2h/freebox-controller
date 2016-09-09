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

            HTTP_Request.setHost(host);

            Console.WriteLine("Starting...");
            //preparing login
            if (freeboxController.Authentification.exist() == false)
            {
                freeboxController.Authentification.registerApp("ordi henri", "freebox controller", "fr.freebox.controller", appVersion);
                Console.WriteLine("Please change settings");
                Console.ReadLine();
            }
            else
            {
                freeboxController.Authentification.loginAuto();
                Console.WriteLine("Logged in");
            }
            freeboxController.Authentification.saveSettings();


            requests.configuration.lan_browser.LanInterfaces interfaces = freeboxController.Configuration.Lan_Browser.getLanInterfaces();
            requests.configuration.lan_browser.LanHostObject objects = freeboxController.Configuration.Lan_Browser.getListLanHostObject();
            
            /*
            bool enabled = true;
            while (true)
            {
                enabled = freeboxController.Wifi.getWifiInfo();
                if (enabled == true)
                {
                    Console.WriteLine("It is on !!! " + DateTime.Now.ToString());
                    enabled = freeboxController.Wifi.setFalse();
                    Console.WriteLine(" Now : " + enabled + Environment.NewLine);
                }
            }*/
            Console.ReadLine();
        }




    }
}
