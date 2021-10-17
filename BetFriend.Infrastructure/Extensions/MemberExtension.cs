namespace BetFriend.Bet.Infrastructure.Extensions
{
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Bet.Infrastructure.DataAccess.Entities;


    internal static class MemberExtension
    {
        internal static Member ToMember(this MemberEntity memberEntity)
        {
            return new Member(new MemberId(memberEntity.MemberId),
                              memberEntity.MemberName,
                              memberEntity.Wallet);
        }
    }
}
