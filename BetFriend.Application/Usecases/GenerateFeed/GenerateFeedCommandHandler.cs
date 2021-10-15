namespace BetFriend.Bet.Application.Usecases.GenerateFeed
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Exceptions;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Application.Abstractions.Command;
    using MediatR;
    using System;
    using System.Linq;
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
            var member = await _memberRepository.GetByIdAsync(new MemberId(Guid.Parse(request.MemberId)))
                        ?? throw new MemberNotFoundException();
            var bets = await _betRepository.GetBetsForFeedAsync();
            var feedDto = new FeedDto(member.Id.ToString(), bets.Select(x => new BetDto(x.State)));
            await _feedRepository.SaveAsync(feedDto);
            return Unit.Value;
        }
    }
}
