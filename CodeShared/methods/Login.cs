using Newtonsoft.Json;
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
                await Core.login.GetChallengeAsync();
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

        // app declaration informations
        public string app_id { get; set; } // id of the app like : "fr.freebox.controller"
        public string app_name { get; set; } // name displayed for the app like : "freebox controller"
        public string version { get; set; } // version  of th app like : "0.0.1"
        public string deviceName { get; set; } // device name like : "computer of henri"

        public async System.Threading.Tasks.Task<bool> GetSessionChallengeAsync()
        {
            if (App_token == null || App_token == "")
            {
                throw new TokenNull();
            }
            if (Challenge == "" || Challenge == null)
            {
                await GetChallengeAsync();
            }
            //password
            string password = Crypt.Encode(Challenge, App_token);
            //string password = Encode(app_token, challenge);
            requests.login.session.Request sessionRequest = new requests.login.session.Request()
            {
                app_id = app_id,
                password = password
            };


            // json conversion
            string sessReq = JsonConvert.SerializeObject(sessionRequest);
            
            // post request
            string JsonResponse = await HTTP_Request.HTTP_POSTAsync(Core.Host, "/api/v3/login/session/", sessReq);

            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                Session_token = response.result.session_token;
                Challenge = response.result.challenge;

                requests.permission appPermissions = response.result.permissions;

                // set Fbx_Hedaer for other call
                HTTP_Request.Fbx_Header = Session_token;

                await GetChallengeAsync();
                return true;
            }
            throw new LoginFailedException();
        }

        public async System.Threading.Tasks.Task GetChallengeAsync()
        {
            //update challenge and return if we are loggin or not

            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/login", null);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);

            if (response.success == "true")
            {
                Challenge = response.result.challenge;
                System.Diagnostics.Debug.WriteLine("Challenge : " + Challenge);

                if (response.result.logged_in == "true")
                {
                    loggedIn = true;
                }
                else if (response.result.logged_in == "false")
                {
                    loggedIn = false;

                }
                else { throw new LoginFailedException("An error append in getting the loggin"); }
            }
            else
            {
                throw new LoginFailedException();
            }
        }
        public async System.Threading.Tasks.Task<bool> AuthorizeAppAsync()
        {
            // filling the data in order to send them
            requests.authorisation authorisationRequest = new requests.authorisation()
            {
                app_id = app_id,
                app_name = app_name,
                app_version = version,
                device_name = deviceName
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
                    if (response.success == "true")
                    {
                        string status = response.result.status;

                        //checking the status
                        if (status == "success") { return true; }
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
