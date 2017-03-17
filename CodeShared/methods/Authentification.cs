using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShared.methods
{
    public class Authentification
    {
        // here are all the classes for managing your freebox os, some are blank and not listed in the variable just up there because 
        // they are still under develloppement

        public bool dataRegister = false;
        public Authentification() { Login log = new Login(); }
        ~authentification()
        {
            settingManager.saveData();
            dataRegister = true;
        }


        public async System.Threading.Tasks.Task RegisterAppAsync(string appName, string appId, string version, string machineName)
        {
            Login login = new Login();

            login.deviceName = machineName;
            login.app_name = appName;
            login.app_id = appId;
            login.version = version;

            await login.authorizeAppAsync();
            await login.getSessionTokenAsync();

            Core.login = login;
        }

    }
}
