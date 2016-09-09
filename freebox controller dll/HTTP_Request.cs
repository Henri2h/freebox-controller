using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace freebox_controller
{
    public static class HTTP_Request
    {
        static string host = "";
        public static void setHost(string hostChosen)
        {
            host = hostChosen;
        }
        public static string Fbx_Header = "";
        public static string HTTP_POST(string Url, string Data)
        {
            string Out = String.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host + Url);
            request.Method = "POST";
            request.Timeout = 100000;
            request.ContentType = "application/json";
            request.Accept = "application/json";

            if (Fbx_Header != "")
            {
                request.Headers.Add("X-Fbx-App-Auth", Fbx_Header);
            }

            byte[] sentData = Encoding.UTF8.GetBytes(Data);
            request.ContentLength = sentData.Length;

            using (System.IO.Stream sendStream = request.GetRequestStream())
            {
                sendStream.Write(sentData, 0, sentData.Length);
                sendStream.Close();
            }

            try
            {

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (System.IO.Stream stream = response.GetResponseStream())
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
                Out += Environment.NewLine + "Content : ";

                Stream ReceiveStreame = ex.Response.GetResponseStream();
                using (StreamReader sr = new StreamReader(ReceiveStreame, Encoding.UTF8))
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

                Console.WriteLine(Out);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
            System.Diagnostics.Debug.WriteLine(Out);
            return Out;
        }
        public static string HTTP_GET(string Url, string Data)
        {
            string Out = String.Empty;
            System.Net.WebRequest req = System.Net.WebRequest.Create(host + Url + (string.IsNullOrEmpty(Data) ? "" : "?" + Data));
            if (Fbx_Header != "")
            {
                req.Headers.Add("X-Fbx-App-Auth", Fbx_Header);
            }

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
            /* catch (WebException ex)
             {
                 Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
             }*/
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
            System.Diagnostics.Debug.WriteLine(Out);
            return Out;
        }
        public static string HTTP_PUT(string Url, string Data)
        {
            string Out = String.Empty;
            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host + Url);
                request.Method = "PUT";
                request.Timeout = 100000;
                request.ContentType = "application/json";
                request.Accept = "application/json";

                if (Fbx_Header != "")
                {
                    request.Headers.Add("X-Fbx-App-Auth", Fbx_Header);
                }

                byte[] sentData = Encoding.UTF8.GetBytes(Data);
                request.ContentLength = sentData.Length;

                using (System.IO.Stream sendStream = request.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Close();
                }


                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        return Out = sr.ReadToEnd();
                    }
                }
            }
            catch (ArgumentException ex)
            {

                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            /* catch (WebException ex)
             {
                 Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
             }*/
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }
            return Out;
        }

    }
}

