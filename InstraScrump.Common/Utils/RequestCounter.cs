using InstaScrump.Common.Constants;
using Extension;
using InstaScrump.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InstaScrump.Common.Utils
{
    public class RequestCounter
    {
        private static readonly object m_lock = new Object();

        private static volatile RequestCounter instance = null;
        public static RequestCounter getInstance(IConfig config)
        {
            // DoubleLock 
            if (instance == null)
                lock (m_lock) { if (instance == null) instance = new RequestCounter(config); }

            return instance;
        }

        private RequestCounter(IConfig config)
        {
            RequestLimitMinute = config.Read<long>(ConfigKey.Request_Limit_Per_Minute_Key, "Config");
            RequestLimitHours = config.Read<long>(ConfigKey.Request_Limit_Per_Hour_Key, "Config");

            RequestCount = new List<DateTime>();
        }


        private static long RequestLimitHours;
        private static long RequestLimitMinute;
        private static List<DateTime> RequestCount { get; set; }

        public void CheckRequestLimit()
        {
            if (instance == null || RequestCount == null)
                throw new Exception("Singelton class needs to be initialized!");

            while (RequestCount.Count(t => t.IsBetween(DateTime.Now.AddHours(-1), DateTime.Now)) >= RequestLimitHours || RequestCount.Count(t => t.IsBetween(DateTime.Now.AddMinutes(-1), DateTime.Now)) >= RequestLimitMinute)
            {
                Sleeper.Sleep(250);
            }

            RequestCount.Add(DateTime.Now);

            //cleanup ;)
            RequestCount.RemoveAll(d => d.IsSmallerThan(DateTime.Now.AddHours(-1).AddMinutes(-30)));
        }
    }
}
