using System;
using System.Collections.Generic;
using System.Text;

namespace CodeShared.methods
{
    public class Authentification
    {
        // here are all the classes for managing your freebox os, some are blank and not listed in the variable just up there because 
        // they are still under develloppement

        public login Login;
        public bool dataRegister = false;
        
        ~authentification()
        {
            setting_manager.saveData(Login);
            dataRegister = true;
        }

        //login class and help functions
        public class login
        {

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

            public bool authorizeApp()
            {
                // filling the data in order to send them
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
                        authorisation = HTTP_Request.HTTP_GETAsync(Host, "/api/v3/login/authorize/" + track_id, null);
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
                                    // determining add : 
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

        }
