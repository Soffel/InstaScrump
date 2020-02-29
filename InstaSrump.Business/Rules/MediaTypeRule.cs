using InstagramApiSharp.Classes.Models;

namespace InstaScrump.Business.Rules
{
    public class MediaTypeRule : IRule
    {
        public MediaTypeRule(InstaMediaType[] _types)
        {
            types = _types;
        }

        private InstaMediaType[] types;
        public RuleType type => RuleType.MediaType;

        public bool MatchRule(InstaMedia media)
        {
            foreach(var typ in types)
            {
                if (media.MediaType == typ)
                    return true;
            }

            return false;
        }
    }
}
