using InstagramApiSharp.API;
using InstaScrump.Common.Constants;
using InstaScrump.Common.Interfaces;
using InstaScrump.Database.Model;
using System;
using Utils;

namespace InstaScrump.Business.Repository
{
    public class BaseRepository
    {
        public BaseRepository(IDbContext<InstaScrumpDB> dbContext, IConfig config)
        {
            DbContext = dbContext;
            Config = config;
            RequestCounter = RequestCounter.getInstance(Config.Read<long>(ConfigKey.Request_Limit_Per_Minute_Key, "Config"), Config.Read<long>(ConfigKey.Request_Limit_Per_Hour_Key, "Config"), Config.Read<long>(ConfigKey.Request_Limit_Per_Day_Key, "Config"));
            LikeCounter = RequestCounter.getInstance(10,80,700);
        }

        protected static IConfig Config { get; private set; }
        protected static IInstaApi InstaApi { get; set; }

        private static RequestCounter RequestCounter { get; set; }
        private static RequestCounter LikeCounter { get; set; }
        protected IDbContext<InstaScrumpDB> DbContext { get; private set; }

        protected static void CheckRequestLimit(EventArgs e = null)
        {
            Sleeper.RandomSleep(10, 300);

            if (RequestCounter != null)
                RequestCounter.CheckRequestLimit(e);
        }

        protected static void CheckLikeLimit(EventArgs e = null)
        {
            Sleeper.RandomSleep(10, 100);

            if (LikeCounter != null)
                LikeCounter.CheckRequestLimit(e);
        }

        protected void SetLikeLimitEvent()
        {
            if(IsLikeLimitEvent != null)
                LikeCounter.IsLimitEvent += IsLikeLimitEvent;     
        }

        protected void SetRequestLimitEvwnt()
        {
            if(IsRequestLimitEvent != null)
                RequestCounter.IsLimitEvent += IsRequestLimitEvent;
        }

        protected event EventHandler IsRequestLimitEvent;
        protected event EventHandler IsLikeLimitEvent;
    }
}
