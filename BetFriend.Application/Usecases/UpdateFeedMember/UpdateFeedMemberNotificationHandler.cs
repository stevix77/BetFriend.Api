using BetFriend.Application.Usecases.InsertBetQuerySide;
using BetFriend.Domain.Bets;
using BetFriend.Domain.Exceptions;
using BetFriend.Domain.Feeds;
using BetFriend.Domain.Members;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BetFriend.Application.Usecases.UpdateFeedMember
{
    public class UpdateFeedMemberNotificationHandler : INotificationHandler<InsertBetQuerySideNotification>
    {
        private IBetRepository _betRepository;
        private IMemberRepository _memberRepository;
        private IFeedRepository _feedRepository;

        public UpdateFeedMemberNotificationHandler(IBetRepository betRepository, IMemberRepository memberRepository, IFeedRepository feedRepository)
        {
            _betRepository = betRepository;
            _memberRepository = memberRepository;
            _feedRepository = feedRepository;
        }

        public async Task Handle(InsertBetQuerySideNotification notification, CancellationToken cancellationToken)
        {
            var bet = await _betRepository.GetByIdAsync(notification.BetId)
                        ?? throw new BetUnknownException($"Bet with Id {notification.BetId} does not exists");
            var member = await _memberRepository.GetByIdAsync(notification.MemberId);
            var feeds = await _feedRepository.GetByIdsAsync(member.Followers.Select(x => x.MemberId.Value));
            foreach(var feed in feeds)
                feed.AddBet(bet.State);
            await _feedRepository.SaveAsync(feeds);
        }
    }
}
