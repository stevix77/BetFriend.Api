namespace BetFriend.Application.Usecases.RetrieveBet
{
    using BetFriend.Application.Abstractions.Query;
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Models;
    using BetFriend.Domain.Exceptions;
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
                            ?? throw new BetUnknownException($"This bet with id: {request.BetId} does not exist");
        }
    }
}
