using InstaScrump.Business.Repository;
using InstaScrump.Common.Utils;
using InstaScrump.Database.Model;

namespace InstaScrump.Business
{
    public class InstaScrumpUnitOfWork
    {
        public InstaScrumpUnitOfWork(DbContext dbContext, Config config)
        {
            FollowRepository = new FollowRepository(dbContext, config);
            AuthenticationRepository = new AuthenticationRepository(dbContext, config);
        }

        public static FollowRepository FollowRepository { get; private set; }
        public static AuthenticationRepository AuthenticationRepository { get; private set; }
    }
}
