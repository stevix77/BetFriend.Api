namespace BetFriend.Bet.Application.Usecases.InsertBetQuerySide
{
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class InsertBetQuerySideNotificationHandler : INotificationHandler<InsertBetQuerySideNotification>
    {
        private readonly IBetRepository _betRepository;
        private readonly IBetQueryRepository _queryBetRepository;

        public InsertBetQuerySideNotificationHandler(IBetRepository betRepository,
                                                IBetQueryRepository queryBetRepository)
        {
            _betRepository = betRepository;
            _queryBetRepository = queryBetRepository;
        }

        public async Task Handle(InsertBetQuerySideNotification request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var bet = await _betRepository.GetByIdAsync(new BetId(request.BetId)).ConfigureAwait(false)
                    ?? throw new BetUnknownException($"BetId: {request.BetId} is unknwon");
            var betDto = new BetDto(bet.State);
            await _queryBetRepository.SaveAsync(betDto);
        }

        private static void ValidateRequest(InsertBetQuerySideNotification request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));
        }
    }
}
