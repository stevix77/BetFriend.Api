namespace BetFriend.WebApi.Controllers.LaunchBet
{
    using System;

    public class LaunchBetInput
    {
        public Guid BetId { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public int Coins { get; set; }
    }
}