using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;

namespace FreeboxController_APP
{
    public class AppCore
    {
        public static CodeShared.FreeboxControl FreeboxController { get; set; }
        public static Windows.Storage.StorageFolder AppStorageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        public static string AppDataFolder { get { return Path.Combine(AppStorageFolder.Path, "Data"); } }

        public static async Task StartAsync()
        {
            FreeboxController = new CodeShared.FreeboxControl("http://mafreebox.freebox.fr", AppDataFolder);


            if (FreeboxController.authentification.NeedRegistration)
            {
                System.Diagnostics.Debug.WriteLine("Registering app");

                var hostNames = NetworkInformation.GetHostNames();
                var hostName = hostNames.FirstOrDefault(name => name.Type == HostNameType.DomainName)?.DisplayName ?? "???";

                FreeboxController.MachineName = hostName;
                Task r = FreeboxController.authentification.RegisterAppAsync();
                await r;
            }
            await FreeboxController.authentification.ConnectAppAsync();
        }
    }
}
