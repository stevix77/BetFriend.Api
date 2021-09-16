namespace BetFriend.Application.Usecases.CloseBet
{
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Domain;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CloseBetCommandHandler : ICommandHandler<CloseBetCommand>
    {
        private IBetRepository _betRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CloseBetCommandHandler(IBetRepository repository, IDateTimeProvider dateTimeProvider)
        {
            _betRepository = repository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(CloseBetCommand request, CancellationToken cancellationToken)
        {
            Bet bet = await GetBet(request).ConfigureAwait(false);
            if (!IsCreator(request.MemberId, bet.State.Creator))
                throw new MemberAuthorizationException($"Member {request.MemberId} is not creator of this bet");
            bet.Close(request.Success, _dateTimeProvider);
            await _betRepository.SaveAsync(bet);

            return Unit.Value;

            static bool IsCreator(Guid memberId, Member creator)
            {
                return memberId == creator.Id.Value;
            }

            async Task<Bet> GetBet(CloseBetCommand request)
            {
                return await _betRepository.GetByIdAsync(request.BetId).ConfigureAwait(false)
                            ?? throw new BetUnknownException($"This bet with id {request.BetId} is unknown");
            }
        }
    }
}
