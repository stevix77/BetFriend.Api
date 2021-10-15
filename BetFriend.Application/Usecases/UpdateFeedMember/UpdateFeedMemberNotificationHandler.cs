namespace BetFriend.Bet.Application.Usecases.UpdateFeedMember
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Usecases.InsertBetQuerySide;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using MediatR;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;


    public class UpdateFeedMemberNotificationHandler : INotificationHandler<InsertBetQuerySideNotification>
    {
        private readonly IBetRepository _betRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IFeedRepository _feedRepository;

        public UpdateFeedMemberNotificationHandler(IBetRepository betRepository, IMemberRepository memberRepository, IFeedRepository feedRepository)
        {
            _betRepository = betRepository;
            _memberRepository = memberRepository;
            _feedRepository = feedRepository;
        }

        public async Task Handle(InsertBetQuerySideNotification notification, CancellationToken cancellationToken)
        {
            var bet = await _betRepository.GetByIdAsync(new BetId(notification.BetId))
                        ?? throw new BetUnknownException($"Bet with Id {notification.BetId} does not exists");
            var member = await _memberRepository.GetByIdAsync(new MemberId(notification.MemberId));
            var feeds = await _feedRepository.GetByIdsAsync(member.Subscriptions.Select(x => x.MemberId.Value));
            foreach (var feed in feeds)
                feed.Bets.Add(new Models.BetDto(bet.State));
            await _feedRepository.SaveAsync(feeds);
        }
    }
}
