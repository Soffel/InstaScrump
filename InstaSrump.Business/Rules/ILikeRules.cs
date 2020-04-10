using InstagramApiSharp.Classes.Models;
using System;

namespace InstaScrump.Business.Rules
{
    public interface ILikeRules
    {
        string Name { get; }
        string MainSearch { get;}
        IRule[] DontLikeRules { get; }

        bool DoLike(InstaMedia media);

        int LikeChance { get; }
        int Count { get; }
    }

    public  interface IRule
    {
        RuleType type { get;}
        bool MatchRule(InstaMedia media);
    }

    public enum RuleType
    {
        Contains,
        NoCaption,
        MediaType,
        ContainsCombo,
        MaxLikes,
    }

    public static class LikeRulesExtension
    {
        public static bool RandomLike(this ILikeRules rules)
        {
            if (rules.LikeChance == 100)
                return true;

            var time = DateTime.Now.Ticks;

            if (time % 100 <= rules.LikeChance)
                return true;

            return false;
        }
    }
}
