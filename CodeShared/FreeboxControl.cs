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
            Core.Host = Host;
            settingManager.fileDir = fileDir;
            // different classes :
            authentification = new Authentification();
            wifi = new Wifi();
        }


        // setting manager
        public bool exist()
        {
            return settingManager.fileExist();
        }
        public void saveSettings()
        {
            settingManager.saveData();
            Core.dataRegister = true;
        }
    }
}
