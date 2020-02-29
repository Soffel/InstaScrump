namespace InstaScrump.Business.Rules
{
    public class ContainsSpam : ContainsRule
    {
        public ContainsSpam() : base(new[] { "f4f", "l4l", "follow", "like", "#lol", "c4c", "#comment", "#youtube", "#vip" }) { }
    }
}
