using InstagramApiSharp.API;
using InstaScrump.Common.Interfaces;
using InstaScrump.Database.Model;

namespace InstaScrump.Business.Repository
{
    public class BaseRepository
    {
        public BaseRepository(IDbContext<InstaScrumpDB> dbContext, IConfig config)
        {
            DbContext = dbContext;
            Config = config;
        }

        protected static IConfig Config { get; private set; }
        protected static IInstaApi InstaApi { get; set; }
        protected IDbContext<InstaScrumpDB> DbContext { get; private set; }
    }
}
