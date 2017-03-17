using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShared.methods
{
    public class Login
    {
        public async System.Threading.Tasks.Task<bool> loginAutoAsync()
        {
            try
            {
                Core.login = settingManager.getData();
                await Core.login.getChallengeAsync();
                await Core.login.getSessionTokenAsync();
                return true;
            }
            catch (Exception ex) { throw new Exception("Error in the auto login feature", ex); }

        }

        //login
        public string app_token { get; set; }
        public string session_token { get; set; }

        public string track_id { get; set; }
        public bool loggedIn = false;
        public string challenge { get; set; }

        // app declaration informations
        public string app_id { get; set; } // id of the app like : "fr.freebox.controller"
        public string app_name { get; set; } // name displayed for the app like : "freebox controller"
        public string version { get; set; } // version  of th app like : "0.0.1"
        public string deviceName { get; set; } // device name like : "computer of henri"

        public async System.Threading.Tasks.Task<bool> getSessionTokenAsync()
        {
            if (app_token == null || app_token == "")
            {
                throw new TokenNull();
            }
            if (challenge == "" || challenge == null)
            {
                await getChallengeAsync();
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
            string JsonResponse = await HTTP_Request.HTTP_POSTAsync(Core.Host, "/api/v3/login/session/", sessReq);

            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                session_token = response.result.session_token;
                challenge = response.result.challenge;
                requests.permission appPermissions = response.result.permissions;
                HTTP_Request.Fbx_Header = session_token;
                await getChallengeAsync();
                return true;
            }
            throw new LoginFailedException();
        }

        public async System.Threading.Tasks.Task getChallengeAsync()
        {
            //update challenge and return if we are loggin or not

            string JsonResponse = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/login", null);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                challenge = response.result.challenge;
                System.Diagnostics.Debug.WriteLine("Challenge : " + challenge);
                if (response.result.logged_in == "true")
                {
                    loggedIn = true;
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
        public async System.Threading.Tasks.Task<bool> authorizeAppAsync()
        {
            // filling the data in order to send them
            requests.authorisation authorisationRequest = new requests.authorisation();
            authorisationRequest.app_id = app_id;
            authorisationRequest.app_name = app_name;
            authorisationRequest.app_version = version;
            authorisationRequest.device_name = deviceName;

            //serializing
            string authReq = JsonConvert.SerializeObject(authorisationRequest);

            string authorisation = await HTTP_Request.HTTP_POSTAsync(Core.Host, "/api/v3/login/authorize", authReq);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(authorisation);
            if (response.success == "true")
            {

                app_token = response.result.app_token;
                track_id = response.result.track_id;

                //tracking pending ...
                bool requestEnded = false;

                while (requestEnded == false)
                {
                    authorisation = await HTTP_Request.HTTP_GETAsync(Core.Host, "/api/v3/login/authorize/" + track_id, null);
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
