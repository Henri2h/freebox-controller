using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeboxController
{
    public class FreeboxController
    {
        public CodeShared.FreeboxControl fb { get; set; }
        public FreeboxController(string Host, string fileDir)
        {
            fb = new CodeShared.FreeboxControl(Host, fileDir);
        }
    }
}
