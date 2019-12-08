using System;
using System.Collections.Generic;
using System.Text;
using InstagramApiSharp.API;
using InstaScrump.Common.Utils;
using InstaScrump.Database.Model;

namespace InstaScrump.Business.Repository
{
    public class BaseRepository
    {
        public BaseRepository(DbContext dbContext, Config config)
        {
            DbContext = dbContext;
            Config = config;
        }

        protected static Config Config { get; private set; }
        protected static IInstaApi InstaApi { get; private set; }
        protected DbContext DbContext { get; private set; }
    }
}
