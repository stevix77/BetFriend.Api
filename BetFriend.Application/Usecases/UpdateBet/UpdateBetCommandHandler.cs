namespace BetFriend.Application.Usecases.UpdateBet
{
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;


    public class UpdateBetCommandHandler : ICommandHandler<UpdateBetCommand>
    {
        private IBetRepository betRepository;
        private IBetQueryRepository betQueryRepository;

        public UpdateBetCommandHandler(IBetRepository betRepository, IBetQueryRepository betQueryRepository)
        {
            this.betRepository = betRepository;
            this.betQueryRepository = betQueryRepository;
        }

        public async Task<Unit> Handle(UpdateBetCommand request, CancellationToken cancellationToken)
        {
            var bet = await betRepository.GetByIdAsync(request.BetId)
                        ?? throw new BetUnknownException($"This bet with Id {request.BetId} does not exist");
            await betQueryRepository.SaveAsync(bet.State, null);
            return Unit.Value;
        }
    }
}
