using System;
using System.Collections.Generic;
using System.Linq;

namespace BetFriend.Bet.Application.Models
{
    public class FeedDto
    {
        public FeedDto(string id, IEnumerable<BetDto> bets)
        {
            Id = id;
            Bets = bets.ToList();
        }

        public string Id { get; set; }
        public ICollection<BetDto> Bets { get; }
    }
}
