namespace BetFriend.UserAccess.Infrastructure.Configurations
{
    public class AuthenticationConfiguration
    {
        public string Secret { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
