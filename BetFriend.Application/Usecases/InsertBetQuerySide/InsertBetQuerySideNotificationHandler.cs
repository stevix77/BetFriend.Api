namespace BetFriend.Bet.Application.Usecases.InsertBetQuerySide
{
    using BetFriend.Bet.Application.Abstractions.Command;
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
        private readonly IMemberRepository _memberRepository;

        public InsertBetQuerySideNotificationHandler(IBetRepository betRepository,
                                                IBetQueryRepository queryBetRepository,
                                                IMemberRepository memberRepository)
        {
            _betRepository = betRepository;
            _queryBetRepository = queryBetRepository;
            _memberRepository = memberRepository;
        }

        public async Task Handle(InsertBetQuerySideNotification request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var member = await _memberRepository.GetByIdAsync(new(request.MemberId)).ConfigureAwait(false)
                        ?? throw new MemberUnknownException($"MemberId: {request.MemberId} is unknown");
            var bet = await _betRepository.GetByIdAsync(new(request.BetId)).ConfigureAwait(false)
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
