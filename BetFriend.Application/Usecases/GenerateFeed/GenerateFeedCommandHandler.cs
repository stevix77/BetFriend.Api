namespace BetFriend.Bet.Application.Usecases.GenerateFeed
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Domain.Feeds;
    using BetFriend.Shared.Application.Abstractions.Command;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class GenerateFeedCommandHandler : ICommandHandler<GenerateFeedCommand>
    {
        private readonly IFeedRepository _feedRepository;
        private readonly IBetQueryRepository _betQueryRepository;

        public GenerateFeedCommandHandler(IFeedRepository feedRepo, IBetQueryRepository betQueryRepository)
        {
            _feedRepository = feedRepo;
            _betQueryRepository = betQueryRepository;
        }

        public async Task<Unit> Handle(GenerateFeedCommand request, CancellationToken cancellationToken)
        {
            var feed = Feed.Create(Guid.Parse(request.FeedId));
            await _feedRepository.SaveAsync(new List<Feed>() { feed });

            return Unit.Value;
        }
    }
}
