namespace InstaScrump.Business.Rules
{
    public class LikeFollowSpam : ContainsRule
    {
        public LikeFollowSpam() : base(new[] { "lfl", "fff", "f4f", "l4l", "follow", "like", "#lol", "c4c", "s4s", "#comment", "#youtube", "#twitch", "#facebook", "#vip", "suport", "support", "http", ".com", "mecca", "porn" }) { }
    }

    public class VirusSpam : ContainsRule
    {
        public VirusSpam() : base(new[] { "corona", "virus", "lockdown", "quarantine", "kill", "help" }) { }
    }

    public class LocationSpam : ContainsCombo
    {
        public LocationSpam() : base(new[] { 
            new Combo(3, new[]{"berlin", "münchen", "budapest", "stuttgart", "frankfurt", "london", "maiami", "amsterdam", "dubai" }),
            new Combo(3, new[]{"germany", "brasil", "usa", "mexico", "turkey", "france", "california" }),
        }) { }
    }

    public class CamSpam : ContainsCombo
    {
        public CamSpam() : base(new[]{
                new Combo(2, new[] { "nikon","nicon", "canon", "mobile", "sony", "gopro", "iphone", "drone" }),
                new Combo(2, new[] { "700d","600d", "500d", "200d" }),
                new Combo(3, new[] { "24mm", "35mm", "50mm", "75mm", "85mm", "120mm" })
        }){}
    }
}
