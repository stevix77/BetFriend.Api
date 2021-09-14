namespace BetFriend.Application.Usecases.UpdateBet
{
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Models;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
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
            var bet = await _betRepository.GetByIdAsync(request.BetId)
                        ?? throw new BetUnknownException($"This bet with Id {request.BetId} does not exist");

            var dto = new BetDto(bet.State, null);
            await _betQueryRepository.SaveAsync(dto);
            return Unit.Value;
        }
    }
}
