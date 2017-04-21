using Newtonsoft.Json.Linq;
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

        public bool NeedRegistration
        {
            get
            {
                if (Core.login.Session_token == "" || Core.login.Session_token == null) return true;
                return false;
            }
        }

        public Authentification()
        {
            if (Core.login == null)
            {
                Core.login = new Login()
                {
                    deviceName = "MachineName",
                    app_name = "freeboxController",
                    app_id = Guid.NewGuid().ToString("N"),
                    version = "1.1.0"
                };
            }
        }

        public async void ConnectAppAsync()
        {
            await Core.login.GetSessionChallengeAsync();
        }

        // string appName, string appId, string version, string machineName
        public async System.Threading.Tasks.Task RegisterAppAsync()
        {
            await Core.login.AuthorizeAppAsync();
        }

        ~Authentification()
        {
            settingManager.saveData();
            dataRegister = true;
        }
    }
}
