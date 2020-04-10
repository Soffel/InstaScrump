using InstaScrump.Common.Interfaces;
using InstaScrump.Database.Model;
using InstaScrump.Common.Extension;
using Extension;
using System;
using InstaScrump.Business.Rules;
using System.Text;
using System.Threading.Tasks;
using InstagramApiSharp;
using InstagramApiSharp.Classes;
using InstaScrump.Common;
using System.Linq;
using InstaScrump.Common.Utils;

namespace InstaScrump.Business.Repository
{
    public class LikeRepository : BaseRepository
    {
        public LikeRepository(IDbContext<InstaScrumpDB> dbContext, IConfig config) : base(dbContext, config)
        {
        }

        public async Task LikeHashTagbyRule(ILikeRules rules)
        {
            if (!InstaApi.IsUserAuthenticated)
            {
                "Please log in first!".WriteLine(ConsoleColor.Red);
                return;
            }

            var maxId = "";
            var likes = 0;
            do
            {
                CheckRequestLimit();
                var mediaList = await InstaApi.HashtagProcessor.GetRecentHashtagMediaListAsync(rules.MainSearch, PaginationParameters.MaxPagesToLoad((int)(DateTime.Now.Ticks % (5 - 1)) + 1).StartFromMaxId(maxId));

                if(!mediaList.Succeeded)
                {
                    $"{mediaList.Info.ResponseType}".WriteLine(ConsoleColor.Red);    
                    return;  
                }

                maxId = mediaList.Value.NextMaxId;

                Sleeper.RandomSleep(500, 900);

                foreach (var media in mediaList.Value.Medias)
                {
                    if(!media.HasLiked && rules.DoLike(media))
                    {
                        Sleeper.RandomSleep(300, 900);

                        CheckRequestLimit();
                        var like = await InstaApi.MediaProcessor.LikeMediaAsync(media.InstaIdentifier);

                        if (like.Succeeded && like.Value)
                        {
                            likes++;
                            $"liked".DrawProgressBar(likes, rules.Count);   
                        }   
                        else if(!like.Succeeded)
                        {
                            "".WriteLine();
                            like.Info.Message.WriteLine(ConsoleColor.Red);
                            return;
                        }

                        if (likes >= rules.Count)
                        {
                            "ready".WriteLine(ConsoleColor.Green);
                            return;
                        }

                        Sleeper.RandomSleep(200, 500);
                    }

                    Sleeper.RandomSleep(300, 1000);
                }

                Sleeper.RandomSleep(500, 1000);

            } while (maxId != null && likes < rules.Count);
            "ready".WriteLine(ConsoleColor.Green);
        }   
    }

}
