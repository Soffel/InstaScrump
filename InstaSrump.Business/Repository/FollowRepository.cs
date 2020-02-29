using InstaScrump.Common.Interfaces;
using InstaScrump.Database.Model;
using System.Threading.Tasks;
using Extension;
using System;
using LinqToDB;
using InstaScrump.Common.Exceptions;

namespace InstaScrump.Business.Repository
{
    public class FollowRepository : BaseRepository
    {
        public FollowRepository(IDbContext<InstaScrumpDB> dbContext, IConfig config) : base(dbContext, config)
        {
         
        }

        public async Task FollowUser(string username, bool favorit = false)
        {
            if (username.IsNullOrWhiteSpace())
            {
                "Can't follow user without username ;)".WriteLine(ConsoleColor.Red);
                return;
            }

            using (var db = DbContext.Create())
            {
                if((await db.Follows.FindAsync(username)) != default)
                {
                    $"User {username} is already being followed".WriteLine(ConsoleColor.Yellow);
                    return;
                }

                if(!InstaApi.IsUserAuthenticated)
                {
                    "Please log in first!".WriteLine(ConsoleColor.Red);
                    return;
                }

                CheckRequestLimit();
                var user = await InstaApi.UserProcessor.GetUserAsync(username);

                if(user.Succeeded)
                {
                    if(await db.InsertAsync(new Follow
                    {
                        UserName = user.Value.UserName,
                        InstaPk = user.Value.Pk,
                        FullName = user.Value.FullName,
                        Favorit = favorit,
                        FailedUpdate = 0,
                        LastUpdate = DateTime.MinValue
                    }) != 1)
                        throw new DatabaseException("Follow insert failed!");

                    await db.CommitTransactionAsync();
                    return;
                }
                
                user.Info.Message.WriteLine(ConsoleColor.Red);       
            }
        }
    }
}
