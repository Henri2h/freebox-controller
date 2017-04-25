using System;
using System.Threading.Tasks;

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
                return NRS;
                // if (Core.login.Session_token == "" || Core.login.Session_token == null) return true;
                // return false;
            }
        }
        bool NRS = false;

        public Authentification()
        {
            if (Core.login == null)
            {


                Core.login = new Login()
                {
                    DeviceName = "Freebox Controller",
                    App_name = "freeboxController",
                    App_id = Guid.NewGuid().ToString("N"),
                    Version = "1.1.0"
                };

                NRS = true;
            }
        }

        public async Task ConnectAppAsync()
        {
            await Core.login.GetSessionChallengeAsync();
        }

        // string appName, string appId, string version, string machineName
        public async Task RegisterAppAsync()
        {
            await Core.login.AuthorizeAppAsync();
            SettingManager.SaveData();
        }

        ~Authentification()
        {
            SettingManager.SaveData();
            dataRegister = true;
        }
    }
}
