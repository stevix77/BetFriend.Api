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
        private readonly IBetRepository _betRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CloseBetCommandHandler(IBetRepository repository, IDateTimeProvider dateTimeProvider)
        {
            _betRepository = repository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(CloseBetCommand request, CancellationToken cancellationToken)
        {
            Bet bet = await GetBet(request).ConfigureAwait(false);
            bet.Close(new MemberId(request.MemberId), request.Success, _dateTimeProvider);
            await _betRepository.SaveAsync(bet);

            return Unit.Value;

            async Task<Bet> GetBet(CloseBetCommand request)
            {
                return await _betRepository.GetByIdAsync(new(request.BetId)).ConfigureAwait(false)
                            ?? throw new BetUnknownException($"This bet with id {request.BetId} is unknown");
            }
        }
    }
}
