using System;
using System.Collections.Generic;
using System.Text;

namespace UIShared
{
    public class Core
    {
        static FreeboxController.FreeboxController fb { get; set; }
        public static void start(string Host, string fileDir)
        {
            fb = new FreeboxController.FreeboxController(Host, fileDir);
            
        }
    }
}
