namespace BetFriend.Application.Models
{
    using System;
    using System.Collections.Generic;

    public class BetDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid CreatorId { get; set; }
        public IReadOnlyCollection<MemberDto> Participants { get; set; } = new List<MemberDto>();
        public int Coins { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatorUsername { get; set; }
    }
}
