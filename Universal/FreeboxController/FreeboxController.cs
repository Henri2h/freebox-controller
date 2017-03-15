using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeboxController
{
    public class FreeboxController
    {
        FreeboxController(string Host)
        {
            CodeShared.FreeboxControl fb = new CodeShared.FreeboxControl();
        }
    }
}
