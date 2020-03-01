using System;

namespace InstaScrump.Common.Utils
{
    public static class Sleeper
    {
        public static void Sleep(int time = 500)
        {
            System.Threading.Thread.Sleep(time);
        }

        public static void RandomSleep(int min = 100, int max = 1000)
        {
            Sleep((int)(DateTime.Now.Ticks % (max - min)) + min);
        }
    }
}
