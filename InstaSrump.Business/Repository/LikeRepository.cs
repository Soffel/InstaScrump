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
            var cursor = Console.CursorTop;
            do
            {
                CheckRequestLimit();
                var mediaList = await InstaApi.HashtagProcessor.GetRecentHashtagMediaListAsync(rules.MainSearch, PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(maxId));

                if(!mediaList.Succeeded)
                {
                    $"{mediaList.Info.ResponseType}".WriteLine(ConsoleColor.Red);    
                    return;  
                }

                maxId = mediaList.Value.NextMaxId;

                foreach(var media in mediaList.Value.Medias)
                {
                    if(!media.HasLiked && rules.DoLike(media))
                    {
                        CheckRequestLimit();
                        var like = await InstaApi.MediaProcessor.LikeMediaAsync(media.InstaIdentifier);

                        if (like.Succeeded && like.Value)
                        {
                            likes++;
                            "liked".DrawProgressBar(likes, rules.Count);

                            if(likes >= rules.Count)
                                return;
                        }   
                        else if(!like.Succeeded)
                        {
                            like.Info.Message.WriteLine(ConsoleColor.Red);
                            return;
                        }
                    }

                    Sleeper.Sleep();
                }
            } while (maxId != null && likes < rules.Count);
        }   
    }

}
