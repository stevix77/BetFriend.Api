namespace BetFriend.Bet.Application.Usecases.GenerateFeed
{
    using BetFriend.Bet.Application.Exceptions;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Feeds;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Application.Abstractions.Command;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class GenerateFeedCommandHandler : ICommandHandler<GenerateFeedCommand>
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IBetRepository _betRepository;
        private readonly IMemberRepository _memberRepository;

        public GenerateFeedCommandHandler(IFeedRepository feedRepository, IBetRepository betRepository, IMemberRepository memberRepository)
        {
            _feedRepository = feedRepository;
            _betRepository = betRepository;
            _memberRepository = memberRepository;
        }

        public async Task<Unit> Handle(GenerateFeedCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(new(Guid.Parse(request.MemberId)))
                        ?? throw new MemberNotFoundException();
            var bets = await _betRepository.GetBetsForFeedAsync();
            var feed = Feed.Create(member.Id.Value);
            foreach (var bet in bets)
                feed.AddBet(bet.State);
            await _feedRepository.SaveAsync(feed);
            return Unit.Value;
        }
    }
}
