using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeboxController_APP
{
  public   class AppCore
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
                await FreeboxController.authentification.RegisterAppAsync();
            }

            System.Diagnostics.Debug.WriteLine("App registered");
            FreeboxController.authentification.ConnectAppAsync();

            System.Diagnostics.Debug.WriteLine("END");
        }
    }
}
