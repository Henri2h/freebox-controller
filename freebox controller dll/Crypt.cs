using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace freebox_controller
{
    class Crypt
    {
        public static string Encode(string challenge, string app_token)
        {
            try
            {
                var enc = Encoding.ASCII;
                HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(app_token));
                hmac.Initialize();

                byte[] buffer = enc.GetBytes(challenge);
                return BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
            }
            catch { throw; }
        }

    }
}
