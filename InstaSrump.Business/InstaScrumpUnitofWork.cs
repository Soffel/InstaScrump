using InstaScrump.Business.Repository;
using InstaScrump.Common.Interfaces;
using InstaScrump.Database.Model;

namespace InstaScrump.Business
{
    public class InstaScrumpUnitOfWork
    {
        public InstaScrumpUnitOfWork(DbContext dbContext, IConfig config)
        { 
            FollowRepository = new FollowRepository(dbContext, config);
            AuthenticationRepository = new AuthenticationRepository(dbContext, config);
            LikeRepository = new LikeRepository(dbContext, config);
        }

        public  FollowRepository FollowRepository { get; private set; }
        public  AuthenticationRepository AuthenticationRepository { get; private set; }
        public LikeRepository LikeRepository { get; private set; }
    }
}
