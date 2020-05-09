using InstagramApiSharp.Classes.Models;
using InstaScrump.Business.Rules;
//todo load from db

namespace InstaScrump.Rules
{
    public class LPRule : ILikeRules
    {
        public LPRule(int maxLikes)
        {
            Count = maxLikes;
        }

        public string Name => "lp";

        public string MainSearch {
            get 
            {
                return "landscapephotography";
            }
        }

        public IRule[] DontLikeRules => new IRule[]
        {
            new LikesRule(40),
            new MediaTypeRule(new[]{InstaMediaType.Video, InstaMediaType.Carousel }),
            new LikeFollowSpam(),
            new VirusSpam(),
            new LocationSpam(),
            new CamSpam(),
            new ContainsRule(new[]{"fashion","outfit", "@", "fitness", "gay", "selfie", "drawing", "india", "#itsme", "flower", "#love", "poetry", "#people"}),
            new ContainsCombo(new[]{ 
                new Combo(2, new[] { "night", "day" }), 
                new Combo(3, new[] { "food", "film", "wildlife", "newborn", "bnw", "macro", "toy", "book" }), 
                new Combo(2, new[] { "art", "landscape", "football", "ships" }), 
            }),
        };

        public  int  LikeChance => 40;

        public int Count { get; }

        public bool DoLike(InstaMedia media)
        {
            if (media.Caption == null)
            {
                return false;
            }

            if (!this.RandomLike())
                return false;

            foreach (var dontRule in DontLikeRules)
            {
                if (dontRule.MatchRule(media))
                    return false;
            }

            return true;
        }
    }
}
