using InstagramApiSharp.API;
using InstaScrump.Common.Interfaces;
using InstaScrump.Database.Model;
using InstaScrump.Common.Utils;

namespace InstaScrump.Business.Repository
{
    public class BaseRepository
    {
        public BaseRepository(IDbContext<InstaScrumpDB> dbContext, IConfig config)
        {
            DbContext = dbContext;
            Config = config;
            RequestCounter = RequestCounter.getInstance(Config);
        }

        protected static IConfig Config { get; private set; }
        protected static IInstaApi InstaApi { get; set; }

        private static RequestCounter RequestCounter { get; set; }
        protected IDbContext<InstaScrumpDB> DbContext { get; private set; }

        protected static void CheckRequestLimit()
        {
            Sleeper.RandomSleep(0, 300);

            if (RequestCounter != null)
                RequestCounter.CheckRequestLimit();
        }        
    }
}
