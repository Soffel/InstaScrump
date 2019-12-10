using System;
using System.Collections.Generic;
using System.Text;

namespace InstaScrump.Common.Utils
{
    public static class Sleeper
    {
        public static void Sleep(int time = 500)
        {
            System.Threading.Thread.Sleep(time);
        }
    }
}
