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
            var bet = await _betRepository.GetByIdAsync(request.BetId).ConfigureAwait(false);
            if (!IsBetCreator(request.MemberId, bet.State.Creator))
                throw new MemberAuthorizationException($"Member {request.MemberId} is not creator of this bet");
            bet.Close(request.Success, _dateTimeProvider);
            await _betRepository.SaveAsync(bet);

            return Unit.Value;

            static bool IsBetCreator(Guid memberId, Member creator)
            {
                return memberId == creator.Id.Value;
            }
        }
    }
}
