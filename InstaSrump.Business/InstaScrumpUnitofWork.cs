using System;
using System.Collections.Generic;
using System.Text;
using InstaScrump.Business.Repository;

namespace InstaScrump.Business
{
    public class InstaScrumpUnitOfWork
    {
        public InstaScrumpUnitOfWork()
        {
            FollowRepository = new FollowRepository();
            AuthenticationRepository = new AuthenticationRepository();
        }

        public static FollowRepository FollowRepository { get; private set; }
        public static AuthenticationRepository AuthenticationRepository { get; private set; }
    }
}
