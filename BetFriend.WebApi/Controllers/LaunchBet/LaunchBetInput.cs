namespace BetFriend.WebApi.Controllers.LaunchBet
{
    using System;
    using System.Collections.Generic;

    public class LaunchBetInput
    {
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public IReadOnlyCollection<Guid> Participants { get; set; }
    }
}