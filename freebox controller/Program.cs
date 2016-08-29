using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace freebox_controller
{
    class Program
    {
        public static string host = "http://mafreebox.freebox.fr";

        //login
        public static string app_token;
        public static string track_id;
        public static bool loggedIn = false;
        public static string challenge;

        public static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            //preparing login
            requests.authorisation authorisationRequest = new requests.authorisation();
            authorisationRequest.app_id = "fr.freebox.freeboxcontroller";
            authorisationRequest.app_name = "freebox controller";
            authorisationRequest.app_version = "0.0.1";
            authorisationRequest.device_name = "my pc";
            //serializing
            string authReq = JsonConvert.SerializeObject(authorisationRequest);

            string authorisation = HTTP_POST("/api/v3/login/authorize", authReq);
            requests.response response = JsonConvert.DeserializeObject<requests.response>(authorisation);
            if (response.success == "true")
            {
                //printing advertissement
                Console.WriteLine("Accept the binding request on the screen of your freeboxrevolution");

                app_token = response.result.app_token;
                track_id = response.result.track_id;

                //tracking pending ...
                bool requestEnded = false;
                bool printPending = false;
                int pendingState = 0;

                while (requestEnded == false)
                {


                    authorisation = HTTP_GET("/api/v3/login/authorize/" + track_id, null);
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
                            goto Error;
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
                            goto Error;
                        }
                        else if (status == "granted")
                        {
                            requestEnded = true;
                            Console.WriteLine("request ok");
                        }
                        else if (status == "denied")
                        {

                            Console.WriteLine("request denied");
                            goto Error;
                        }
                        else
                        {
                            Console.WriteLine("unknow request response : " + status);
                            goto Error;
                        }



                    }
                    else
                    {
                        Console.WriteLine("error on request id");
                        goto Error;
                    }

                }


            }
            else
            {
                Console.WriteLine("Error on login");
                goto Error;
            }



            //update challenge and return if we are loggin or not
            getLogin:
            string JsonResponse = HTTP_GET("/api/v3/login", null);
            response = JsonConvert.DeserializeObject<requests.response>(JsonResponse);
            if (response.success == "true")
            {
                challenge = response.result.logged_in = response.result.challenge;
                if (response.result.logged_in == "true")
                {
                    loggedIn = true;
                }
                else if (response.result.logged_in == "false")
                {
                    loggedIn = false;
                }
                else { goto Error; }
            }
            else
            {
                Console.WriteLine("an error append in getting the loggin");
                goto Error;
            }


            gettingSessionToken:

            Error:
            //when an error is throwed
            Console.WriteLine("An error happend");
            prinState();

            End:


            Console.ReadLine();
        }

        public static void prinState()
        {
            Console.WriteLine("Printing the current state : ");
            Console.WriteLine("==============================");
            Console.WriteLine("Logged in : " + loggedIn);
            Console.WriteLine("App token : " + app_token);
            Console.WriteLine("track id  : " + track_id);
            Console.WriteLine("Challenge : " + challenge);
            Console.WriteLine("==============================" + "\n");
        }



        public static string HTTP_POST(string Url, string Data)
        {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(host + Url);
            try
            {
                req.Method = "POST";
                req.Timeout = 100000;
                req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.UTF8.GetBytes(Data);
                req.ContentLength = sentData.Length;
                using (System.IO.Stream sendStream = req.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }
                System.Net.WebResponse res = req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }

        public static string HTTP_GET(string Url, string Data)
        {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(host + Url + (string.IsNullOrEmpty(Data) ? "" : "?" + Data));
            try
            {
                System.Net.WebResponse resp = req.GetResponse();
                using (System.IO.Stream stream = resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        Out = sr.ReadToEnd();
                    }
                }
            }

            catch (ArgumentException ex)
            {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }


            return Out;
        }

    }
}
