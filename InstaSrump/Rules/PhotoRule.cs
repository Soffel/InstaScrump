using InstagramApiSharp.Classes.Models;
using InstaScrump.Business.Rules;
//todo load from db

namespace InstaScrump.Rules
{
    public class PhotoRule : ILikeRules
    {
        public PhotoRule(int maxLikes)
        {
            Count = maxLikes;
        }

        public string Name => "foto";

        public string MainSearch => "photograhy";

        public IRule[] DontLikeRules => new IRule[]
        {
            new LikesRule(40),
            new MediaTypeRule(new[]{InstaMediaType.Video, InstaMediaType.Carousel }),
            new LikeFollowSpam(),
            new VirusSpam(),
            new LocationSpam(),
            new CamSpam(),
            new ContainsRule(new[]{"fashion","outfit", "@", "fitness", "gay", "selfie", "drawing", "gat", "handcraft", "my", "kill"}),
            new ContainsCombo(new[]{ 
                new Combo(3, new[] { "food", "landscape", "film", "wildlife", "newborn", "bnw", "macro", "toy", "book" }), 
                new Combo(2, new[] { "cat", "dog", "bunny", "monkey" }) 
            }),
            
        };

        public  int  LikeChance => 45;

        public int Count { get; }

        public bool DoLike(InstaMedia media)
        {
            if (media.Caption == null || !media.Caption.Text.Contains(MainSearch)) //gesuchter hashtag in Kommentaren
            {
                return false;
            }

            foreach(var dontRule in DontLikeRules)
            {
                if (dontRule.MatchRule(media))
                    return false;
            }

            return this.RandomLike();
        }
    }
}
