using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace freebox_controller
{
    class FreeboxControl
    {

        //login
        public string app_token;
        public string session_token;

        public string track_id;
        public bool loggedIn = false;
        public string challenge;

        // app declaration informations
        public string app_id = ""; // id of the app like : "fr.freebox.controller"
        public string app_name = ""; // name displayed for the app like : "freebox controller"
        public string version = ""; // version  of th app like : "0.0.1"
        public string deviceName = ""; // device name like : "computer of henri"

        public bool authorizeApp()
        {

            requests.authorisation authorisationRequest = new requests.authorisation();
            authorisationRequest.app_id = app_id;
            authorisationRequest.app_name = app_name;
            authorisationRequest.app_version = version;
            authorisationRequest.device_name = deviceName;

            //serializing
            string authReq = JsonConvert.SerializeObject(authorisationRequest);

            string authorisation = HTTP_Request.HTTP_POST("/api/v3/login/authorize", authReq);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(authorisation);
            if (response.success == "true")
            {
                //printing advertissement
                Console.WriteLine("Accept the binding request on the screen of your freebox revolution");
                app_token = response.result.app_token;
                track_id = response.result.track_id;

                //tracking pending ...
                bool requestEnded = false;
                bool printPending = false;
                int pendingState = 0;

                while (requestEnded == false)
                {
                    authorisation = HTTP_Request.HTTP_GET("/api/v3/login/authorize/" + track_id, null);
                    response = JsonConvert.DeserializeObject<requests.response>(authorisation);
                    if (response.success == "true")
                    {
                        string status = response.result.status;
                        //if we must return to the line
                        if (status != "pending" && printPending)
                        {
                            Console.WriteLine("");
                        }
                        //checking the status
                        if (status == "unknow")
                        {
                            Console.WriteLine("Status unknow");
                            return false;
                        }
                        else if (status == "pending")
                        {
                            string add = "";

                            if (printPending)
                            {
                                switch (pendingState)
                                {
                                    case 0:
                                        add = "|";
                                        break;
                                    case 1:
                                        add = "/";
                                        break;
                                    case 2:
                                        add = "-";
                                        break;
                                    case 3:
                                        add = "\\";
                                        break;
                                    case 4:
                                        add = "|";
                                        break;
                                    case 5:
                                        add = "/";
                                        break;
                                    case 6:
                                        add = "-";
                                        break;
                                    case 7:
                                        add = "\\";
                                        pendingState = -1;
                                        break;

                                    default:
                                        add = "|";
                                        pendingState = -1;
                                        break;
                                }
                                pendingState++;
                                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                                Console.Write(add);
                            }
                            else
                            {
                                Console.Write("Pending... ");
                                printPending = true;
                            }
                        }
                        else if (status == "timeout")
                        {
                            Console.WriteLine("request timeout");
                            return false;
                        }
                        else if (status == "granted")
                        {
                            requestEnded = true;
                            Console.WriteLine("request ok");
                            return true;
                        }
                        else if (status == "denied")
                        {
                            Console.WriteLine("request denied");
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("unknow request response : " + status);
                            return false;
                        }
                    }
                }
            }
            return false;
        }
        public bool getSessionToken()
        {
            getChallenge();
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
                return true;
            }
            return false;
        }
        public void getChallenge()
        {
            //update challenge and return if we are loggin or not

            string JsonResponse = HTTP_Request.HTTP_GET("/api/v3/login", null);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                challenge = response.result.challenge;
                if (response.result.logged_in == "true")
                {
                    loggedIn = true;
                }
                else if (response.result.logged_in == "false")
                {
                    loggedIn = false;
                }
                else { Console.WriteLine("an error append in getting the loggin"); }
            }
            else
            {
                Console.WriteLine("an error append in getting the loggin");
            }
        }

    }
}
