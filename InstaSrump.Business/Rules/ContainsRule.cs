using InstagramApiSharp.Classes.Models;
using Extensions;

namespace InstaScrump.Business.Rules
{
    public class ContainsRule : IRule
    {
        public ContainsRule(string[] _values)
        {
            values = _values;
        }

        public RuleType type => RuleType.Contains;
        public string[] values { get; set; }

        public bool MatchRule(InstaMedia media)
        {
            if (media.Caption?.Text.IsNullOrWhiteSpace() == null)
                return false;

            foreach(var value in values)
            {
                if (media.Caption.Text.Contains(value, System.StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
