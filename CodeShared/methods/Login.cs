using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShared.methods
{
    public class Login
    {
        public async System.Threading.Tasks.Task<bool> LoginAutoAsync()
        {
            try
            {
                Core.login = SettingManager.GetData();
                await Core.login.GetChallengeValueAsync();
                await Core.login.GetSessionChallengeAsync();
                return true;
            }
            catch (Exception ex) { throw new Exception("Error in the auto login feature", ex); }

        }

        //login
        public string App_token { get; set; }
        public string Session_token { get; set; }

        public string Track_id { get; set; }
        public bool loggedIn = false;
        public string Challenge { get; set; }
        public JToken Permissions { get; set; }

        // app declaration informations
        public string App_id { get; set; } // id of the app like : "fr.freebox.controller"
        public string App_name { get; set; } // name displayed for the app like : "freebox controller"
        public string Version { get; set; } // version  of th app like : "0.0.1"
        public string DeviceName { get; set; } // device name like : "computer of henri"

        public async System.Threading.Tasks.Task<bool> GetSessionChallengeAsync()
        {
            try
            {
                if (App_token == null || App_token == "")
                {
                    throw new TokenNull();

                }

                System.Diagnostics.Debug.WriteLine("app_token : " + this.App_token);
                System.Diagnostics.Debug.WriteLine("app_id : " + this.App_id);

                if (Challenge == "" || Challenge == null)
                {
                    Challenge = await GetChallengeValueAsync();
                }

                //password
                string password = Crypt.Encode(Challenge, App_token);
                bool success = await this.OpenSessionAsync(password);

                if (success)
                {
                    //  requests.permission appPermissions = response.result.permissions;
                    // set Fbx_Hedaer for other call
                    HTTP_Request.Fbx_Header = this.Session_token;

                }
                else
                    throw new LoginFailedException();
                return success;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Message : " + ex.Message);
                System.Diagnostics.Debug.WriteLine("Source : " + ex.Source);
                System.Diagnostics.Debug.WriteLine("Stack trace : " + ex.StackTrace);
                return false;
            }
        }

        public async System.Threading.Tasks.Task<bool> OpenSessionAsync(string password)
        {
            JObject request = new JObject
            {
                { "app_id", App_id },
                { "password", password }
            };
            System.Diagnostics.Debug.WriteLine("Request calling : " + request.ToString());

            // post request
            HTTP_Request.SessionToken = this.Session_token;
            string JsonResponse = await HTTP_Request.HTTP_POSTAsync(Core.Host, "/api/v3/login/session/", request.ToString());
            HTTP_Request.SessionToken = "";


            JObject response = JObject.Parse(JsonResponse);

            bool success = (bool)response["success"];
            if (success)
            {
                this.Challenge = (string)response["result"]["challenge"];
                this.Session_token = (string)response["result"]["session_token"];
                this.Permissions = response["result"]["permissions"];
            }
            else
            {
                string msg = (string)response["msg"];
                string error_code = (string)response["error_code"];
                System.Diagnostics.Debug.WriteLine("Cannot connect : " + msg + " " + error_code);
                this.Challenge = (string)response["result"]["challenge"];
            }
            return success;
        }

        public async System.Threading.Tasks.Task<string> GetChallengeValueAsync()
        {
            string challenge = "";
            /*
             {
                "success": true,
                "result": {
                    "logged_in": false,
                    "challenge": "VzhbtpR4r8CLaJle2QgJBEkyd8JPb0zL"
                }
               }
             */

            //update challenge and return if we are loggin or not
            HTTP_Request.SessionToken = this.Session_token;
            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/login", null);
            HTTP_Request.SessionToken = "";


            JObject response = JObject.Parse(JsonResponse);

            bool responseSucess = (bool)response["success"];
            if (responseSucess)
            {
                challenge = (string)response["result"]["challenge"];
                this.loggedIn = (bool)response["result"]["logged_in"];

                System.Diagnostics.Debug.WriteLine("Challenge : " + Challenge);

            }
            else
            {
                throw new LoginFailedException();
            }
            return challenge;
        }

        public async System.Threading.Tasks.Task<bool> AuthorizeAppAsync()
        {
            // filling the data in order to send them
            requests.authorisation authorisationRequest = new requests.authorisation()
            {
                app_id = App_id,
                app_name = App_name,
                app_version = Version,
                device_name = DeviceName
            };

            //serializing
            string authReq = JsonConvert.SerializeObject(authorisationRequest);

            string authorisation = await HTTP_Request.HTTP_POSTAsync(Core.Host, "/api/v3/login/authorize", authReq);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(authorisation);
            if (response.success == "true")
            {

                App_token = response.result.app_token;
                Track_id = response.result.track_id;

                System.Diagnostics.Debug.WriteLine("Track pending ...");
                //tracking pending ...
                bool requestEnded = false;

                while (requestEnded == false)
                {
                    authorisation = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/login/authorize/" + Track_id, null);
                    response = JsonConvert.DeserializeObject<requests.response>(authorisation);

                    JObject authorisationJObject = JObject.Parse(authorisation);

                    System.Diagnostics.Debug.WriteLine(authorisationJObject["success"]);

                    if ((bool)authorisationJObject["success"] == true)
                    {
                        System.Diagnostics.Debug.WriteLine("Success request");

                        string status = response.result.status;

                        //checking the status
                        if (status == "granted") { return true; }
                        else if (status == "timeout") { throw new Exception("Request timeout"); }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(authorisationJObject["msg"]);
                        System.Diagnostics.Debug.WriteLine(authorisationJObject["error_code"]);
                    }
                }
            }
            return false;
        }

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
}
