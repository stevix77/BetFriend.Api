namespace BetFriend.Application.Usecases.RetrieveBets
{
    using BetFriend.Application.Abstractions.Query;
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;


    public sealed class RetrieveBetsQueryHandler
        : IQueryHandler<RetrieveBetsQuery, IReadOnlyCollection<BetDto>>
    {
        private IBetQueryRepository _betRepository;

        public RetrieveBetsQueryHandler(IBetQueryRepository betRepository)
        {
            _betRepository = betRepository ?? throw new ArgumentNullException(nameof(betRepository));
        }

        public async Task<IReadOnlyCollection<BetDto>> Handle(RetrieveBetsQuery request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            return await _betRepository.GetBetsForMemberAsync(request.MemberId);
        }

        private static void ValidateRequest(RetrieveBetsQuery request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request), $"request cannot be null");
        }
    }
}
