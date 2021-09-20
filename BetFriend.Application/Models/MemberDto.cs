namespace BetFriend.Bet.Application.Models
{
    using BetFriend.Bet.Domain.Members;
    using System;
    public class MemberDto
    {
        public MemberDto()
        {
        }

        public MemberDto(Member creator)
        {
            Id = creator.Id.Value;
            Username = creator.Name;
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}
