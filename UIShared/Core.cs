using System;
using System.Collections.Generic;
using System.Text;

namespace UIShared
{
    public class Core
    {
        static CodeShared.FreeboxControl fb { get; set; }

        public static void Start(string Host, string fileDir)
        {
            fb = new CodeShared.FreeboxControl(Host, fileDir);
            
        }
    }
}
