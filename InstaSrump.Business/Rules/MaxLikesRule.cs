using InstagramApiSharp.Classes.Models;

namespace InstaScrump.Business.Rules
{
    public class LikesRule : IRule
    {
        public LikesRule(int likes)
        {
            maxLikes = likes;
        }
        public RuleType type => RuleType.MaxLikes;

        private int maxLikes;
        public bool MatchRule(InstaMedia media)
        {
            return media.LikesCount >= maxLikes;
        }
    }
}
