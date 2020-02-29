using InstagramApiSharp.Classes.Models;

namespace InstaScrump.Business.Rules
{
    public class Combo
    {
        public Combo(int c, string[] v)
        {
            count = c;
            content = v;
        }

        public int count { get; }
        public string[] content { get; }
    }

    public class ContainsCombo : IRule
    {
        public ContainsCombo(Combo[] _combos)
        {
            combos = _combos; 
        }

        public RuleType type => RuleType.ContainsCombo;

        public Combo[] combos;

        public bool MatchRule(InstaMedia media)
        {
            foreach(var combo in combos)
            {
                var c = 0;

                foreach(var v in combo.content)
                {
                    if (media.Caption.Text.Contains(v))
                        c++;
                }

                if (c >= combo.count)
                    return true;
            }
            return false;
        }
    }
}
