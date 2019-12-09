using InstaScrump.Common.Interfaces;
using InstaScrump.Database.Model;

namespace InstaScrump.Business.Repository
{
    public class FollowRepository : BaseRepository
    {
        public FollowRepository(IDbContext<InstaScrumpDB> dbContext, IConfig config) : base(dbContext, config)
        {

        }
    }
}
