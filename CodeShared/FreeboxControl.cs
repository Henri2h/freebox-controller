using CodeShared.methods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShared
{
    public class FreeboxControl
    {

        //saving
        public bool autoSave = true;

        public Authentification authentification;
        public Wifi wifi;

        public FreeboxControl(string Host, string fileDir)
        {
            if (Host == "" || Host == null) Host = "http://mafreebox.freebox.fr";

            Core.Host = Host;

            SettingManager.FileDir = fileDir;
           Core.login = SettingManager.TryToLoad();

            // different classes :
            authentification = new Authentification();
            wifi = new Wifi();
        }


        // setting manager
        public bool Exist()
        {
            return SettingManager.FileExist();
        }
        public void SaveSettings()
        {
            SettingManager.SaveData();
            Core.dataRegister = true;
        }
    }
}
