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

        public authentification Authentification;
        public wifi Wifi;
        public calls_contacts Calls_Contacts;
        public configuration Configuration;
        public string Host { get; set; }

        public FreeboxControl(string Host)
        {
            this.Host = Host;
            // different classes :
            Authentification = new authentification();
            Wifi = new wifi();
            Calls_Contacts = new calls_contacts();
            Configuration = new configuration();
        }

      
                public bool getSessionToken()
                {
                    if (app_token == null || app_token == "")
                    {
                        throw new TokenNull();
                    }
                    if (challenge == "" || challenge == null)
                    {
                        getChallenge();
                    }
                    //password
                    string password = Crypt.Encode(challenge, app_token);
                    //string password = Encode(app_token, challenge);
                    requests.login.session.Request sessionRequest = new requests.login.session.Request();
                    sessionRequest.app_id = app_id;
                    sessionRequest.password = password;


                    // json conversion
                    string sessReq = JsonConvert.SerializeObject(sessionRequest);
                    // post request
                    string JsonResponse = HTTP_Request.HTTP_POST("/api/v3/login/session/", sessReq);

                    requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
                    if (response.success == "true")
                    {
                        session_token = response.result.session_token;
                        challenge = response.result.challenge;
                        requests.permission appPermissions = response.result.permissions;
                        HTTP_Request.Fbx_Header = session_token;
                        getChallenge();
                        return true;
                    }
                    throw new LoginFailedException();
                }
                public void getChallenge()
                {
                    //update challenge and return if we are loggin or not

                    string JsonResponse = HTTP_Request.HTTP_GETAsync(Host, "/api/v3/login", null);
                    requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
                    if (response.success == "true")
                    {
                        challenge = response.result.challenge;
                        System.Diagnostics.Debug.WriteLine("Challenge : " + challenge);
                        if (response.result.logged_in == "true")
                        {
                            loggedIn = true;
                            Console.WriteLine("You are now logged in");
                        }
                        else if (response.result.logged_in == "false")
                        {
                            loggedIn = false;

                        }
                        else { throw new LoginFailedException("an error append in getting the loggin"); }
                    }
                    else
                    {
                        throw new LoginFailedException();
                    }
                }

                // exceptions :
                // todo : redew the structure and add custom messages
                public class LoginFailedException : System.Exception
                {
                    public LoginFailedException() { }
                    public LoginFailedException(string message) : base(message) { }
                }
                public class TokenNull : System.Exception
                {
                    public TokenNull() { }
                    public TokenNull(string message) : base(message) { }
                }
            }
            public void loginAuto()
            {
                try
                {
                    Login = setting_manager.getData();
                    Login.getChallenge();
                    Login.getSessionToken();
                }
                catch (Exception ex) { throw new Exception("Error in the auto login feature", ex); }
            }
            public void registerApp(string appName, string appId, string version)
            {
                Login = new login();

                Login.deviceName = Environment.MachineName;
                Login.app_name = appName;
                Login.app_id = appId;
                Login.version = version;

                Login.authorizeApp();
                Login.getSessionToken();
            }

            // setting manager
            public bool exist()
            {
                return setting_manager.fileExist();
            }
            public void saveSettings()
            {
                setting_manager.saveData(Login);
                dataRegister = true;
            }
            public class setting_manager
            {
                public static bool fileExist()
                {
                    return File.Exists(currentDirectory + "\\values.json");
                }
                public static string currentDirectory = System.Environment.CurrentDirectory;
                public static void saveData(login Login)
                {
                    try
                    {
                        string Out = JsonConvert.SerializeObject(Login);
                        File.WriteAllText(currentDirectory + "\\values.json", Out);
                    }
                    catch (FileNotFoundException ex) { throw ex; }

                    catch
                    {
                        throw new ErrorInGettingTheData("Error in saving the data");
                    }
                }
                public class ErrorInGettingTheData : Exception
                {
                    public ErrorInGettingTheData() : base() { }
                    public ErrorInGettingTheData(string message) : base(message) { }
                }
                public static login getData()
                {
                    try
                    {
                        string Json = File.ReadAllText(currentDirectory + "\\values.json");
                        login data = JsonConvert.DeserializeObject<login>(Json);
                        return data;
                    }
                    catch
                    {
                        throw new ErrorInGettingTheData();
                    }
                }

            }

        }


        public bool getWifiInfo()
        {
            string JsonResponse = HTTP_Request.HTTP_GETAsync("/api/v2/wifi/config/", null);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                if (response.result.enabled == "true")
                {
                    return true;
                }
            }
            return false;
        }
        public bool setWifi(bool enabled)
        {
            requests.wifi.globalConfig.wifi authorisationRequest = new requests.wifi.globalConfig.wifi();
            authorisationRequest.enabled = enabled;

            //serializing
            string content = JsonConvert.SerializeObject(authorisationRequest);
            string JsonResponse = HTTP_Request.HTTP_PUT("/api/v2/wifi/config/", content);
            Console.WriteLine(JsonResponse);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                if (response.result.enabled == "true")
                {
                    return true;
                }
            }
            return false;
        }

        public class parental_filter
        {
            public class parental_control { }
        }
        public class calls_contacts
        {
            contacts Contacts = new contacts();
            call Call = new call();

            public class contacts { }
            public class call { }
        }

        public class configuration
        {
            public lan_browser Lan_Browser;
            public configuration()
            {
                Lan_Browser = new lan_browser();
            }
            public class lan_browser
            {
                public requests.configuration.lan_browser.LanHostObject getListLanHostObject()
                {
                    string JsonResponse = HTTP_Request.HTTP_GETAsync("/api/v3/lan/browser/pub", null);

                    requests.configuration.lan_browser.LanHostObject response = JsonConvert.DeserializeObject<requests.configuration.lan_browser.LanHostObject>(JsonResponse);
                    if (response.success == "true")
                    {
                        return response;
                    }
                    return null;
                }
                public requests.configuration.lan_browser.LanInterfaces getLanInterfaces()
                {
                    string JsonResponse = HTTP_Request.HTTP_GETAsync("/api/v3/lan/browser/interfaces", null);

                    requests.configuration.lan_browser.LanInterfaces response = JsonConvert.DeserializeObject<requests.configuration.lan_browser.LanInterfaces>(JsonResponse);

                    if (response.success == "true")
                    {
                        return response;
                    }
                    return null;
                }
            }
        }
        public class file_system { }
        public class downloads { }
    }
}

