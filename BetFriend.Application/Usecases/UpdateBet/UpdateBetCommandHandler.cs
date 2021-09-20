namespace BetFriend.Bet.Application.Usecases.UpdateBet
{
    using BetFriend.Bet.Application.Abstractions.Command;
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;


    public class UpdateBetCommandHandler : ICommandHandler<UpdateBetCommand>
    {
        private IBetRepository _betRepository;
        private IBetQueryRepository _betQueryRepository;

        public UpdateBetCommandHandler(IBetRepository betRepository, IBetQueryRepository betQueryRepository)
        {
            _betRepository = betRepository;
            _betQueryRepository = betQueryRepository;
        }

        public async Task<Unit> Handle(UpdateBetCommand request, CancellationToken cancellationToken)
        {
            var bet = await _betRepository.GetByIdAsync(new(request.BetId))
                        ?? throw new BetUnknownException($"This bet with Id {request.BetId} does not exist");

            var dto = new BetDto(bet.State);
            await _betQueryRepository.SaveAsync(dto);
            return Unit.Value;
        }
    }
}
