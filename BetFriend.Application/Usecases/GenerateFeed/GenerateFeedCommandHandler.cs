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
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class GenerateFeedCommandHandler : ICommandHandler<GenerateFeedCommand>
    {
        private readonly IFeedQueryRepository _feedRepository;
        private readonly IBetQueryRepository _betQueryRepository;
        private readonly IMemberRepository _memberRepository;

        public GenerateFeedCommandHandler(IFeedQueryRepository feedRepository, IBetQueryRepository betQueryRepository, IMemberRepository memberRepository)
        {
            _feedRepository = feedRepository;
            _betQueryRepository = betQueryRepository;
            _memberRepository = memberRepository;
        }

        public async Task<Unit> Handle(GenerateFeedCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(new(Guid.Parse(request.MemberId)))
                        ?? throw new MemberNotFoundException();
            var bets = await _betQueryRepository.GetBetsForFeedAsync();
            var feedDto = new FeedDto(member.Id.ToString(), bets);
            await _feedRepository.SaveAsync(feedDto);
            return Unit.Value;
        }
    }
}
