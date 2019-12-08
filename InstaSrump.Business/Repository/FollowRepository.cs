using System;
using System.Collections.Generic;
using System.Text;
using InstaScrump.Common.Utils;
using InstaScrump.Database.Model;

namespace InstaScrump.Business.Repository
{
    public class FollowRepository : BaseRepository
    {
        public FollowRepository(DbContext dbContext, Config config) : base(dbContext, config)
        {

        }
    }
}
