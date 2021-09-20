namespace BetFriend.Bet.Application.Usecases.RetrieveBet
{
    using BetFriend.Bet.Application.Abstractions.Query;
    using BetFriend.Bet.Application.Abstractions.Repository;
    using BetFriend.Bet.Application.Exceptions;
    using BetFriend.Bet.Application.Models;
    using BetFriend.Bet.Domain.Exceptions;
    using System.Threading;
    using System.Threading.Tasks;

    public class RetrieveBetQueryHandler : IQueryHandler<RetrieveBetQuery, BetDto>
    {
        private readonly IBetQueryRepository _betQueryRepository;

        public RetrieveBetQueryHandler(IBetQueryRepository betQueryRepository)
        {
            _betQueryRepository = betQueryRepository;
        }

        public async Task<BetDto> Handle(RetrieveBetQuery request, CancellationToken cancellationToken)
        {
            return await _betQueryRepository.GetByIdAsync(request.BetId).ConfigureAwait(false)
                            ?? throw new BetNotFoundException($"This bet with id: {request.BetId} is not found");
        }
    }
}
