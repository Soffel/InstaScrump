using InstagramApiSharp.Classes.Models;
using InstaScrump.Business.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstaScrump.Rules
{
    public class CosplayRule : ILikeRules
    {
        public CosplayRule(int maxLikes)
        {
            Count = maxLikes;
        }

        public string Name => "cosplay";

        public string MainSearch => "cosplay";

        public IRule[] DontLikeRules => new IRule[]
        {
            new LikesRule(40),
            new MediaTypeRule(new[]{InstaMediaType.Video}), 
            new VirusSpam(),
            new LikeFollowSpam(),
            new LocationSpam(),
            new ContainsCombo(new[]{
                new Combo(3, new[] { "joker", "robin", "flash", "catwoman", "spiderman", "wonderwoman"}), 
            }),
            new ContainsRule(new[] {"draw","tiktok", "#manga", "#fanart" })

        };

        public int LikeChance => 45;

        public int Count { get; }

        public bool DoLike(InstaMedia media)
        {
            if (media.Caption == null)
            {
                return false;
            }

            foreach (var dontRule in DontLikeRules)
            {
                if (dontRule.MatchRule(media))
                    return false;
            }

            return this.RandomLike();
        }
    
    }
}
