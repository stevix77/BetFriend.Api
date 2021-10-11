namespace BetFriend.Bet.Application.Usecases.CloseBet
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Domain;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class CloseBetCommandHandler : ICommandHandler<CloseBetCommand>
    {
        private readonly IBetRepository _betRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IAuthenticationGateway _authenticationGateway;

        public CloseBetCommandHandler(IBetRepository repository,
                                      IDateTimeProvider dateTimeProvider,
                                      IAuthenticationGateway authenticationGateway)
        {
            _betRepository = repository;
            _dateTimeProvider = dateTimeProvider;
            _authenticationGateway = authenticationGateway;
        }

        public async Task<Unit> Handle(CloseBetCommand request, CancellationToken cancellationToken)
        {
            if (!_authenticationGateway.IsAuthenticated())
                throw new NotAuthenticatedException();
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
