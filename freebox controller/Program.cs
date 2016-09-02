using System;
using Newtonsoft.Json;
using System.IO;

namespace freebox_controller
{
    class Program
    {
        public static string host = "http://mafreebox.freebox.fr";
        public static string currentDirectory = System.Environment.CurrentDirectory;

        public static string appVersion = "0.0.3";

        static FreeboxControl freeboxController = new FreeboxControl();

        public static void Main(string[] args)
        {
            
            HTTP_Request.setHost(host);

            Console.WriteLine("Starting...");
            //preparing login

            registerApp();

            Console.ReadLine();
        }
        static void registerApp()
        {
            freeboxController.deviceName = "my pc";
            freeboxController.app_name = "freebox controller";
            freeboxController.app_id = "fr.freebox.controller";
            freeboxController.version = appVersion;

            freeboxController.authorizeApp();
            freeboxController.getSessionToken();

            string Out = JsonConvert.SerializeObject(freeboxController);
            File.WriteAllText(currentDirectory + "values.json", Out);
        }


    }
}
