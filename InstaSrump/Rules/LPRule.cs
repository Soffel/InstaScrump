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

        public string MainSearch => "landscapephotography";

        public IRule[] DontLikeRules => new IRule[]
        {
            new LikesRule(40),
            new MediaTypeRule(new[]{InstaMediaType.Video, InstaMediaType.Carousel }),
            new ContainsSpam(),
            new ContainsRule(new[]{"fashion","outfit", "@", "fitness", "gay", "selfie", "drawing"}),
            new ContainsCombo(new[]{ 
                new Combo(2, new[] { "nikon", "canon", "mobile", "sony" }), 
                new Combo(2, new[] { "night", "day" }), 
                new Combo(3, new[] { "food", "landscape", "film", "wildlife", "newborn", "bnw", "macro", "toy", "book" }), 
                new Combo(2, new[] { "art", "landscape" }), 
                new Combo(2, new[] { "24mm", "35mm", "50mm", "75mm", "85mm", "120mm" }) 
            }),
        };

        public  int  LikeChance => 86;

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
