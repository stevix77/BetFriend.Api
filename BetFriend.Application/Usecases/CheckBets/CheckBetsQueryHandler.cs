using BetFriend.Application.Abstractions.Command;
using BetFriend.Application.Abstractions.Query;
using BetFriend.Application.Abstractions.Repository;
using BetFriend.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BetFriend.Application.Usecases.CheckBets
{
    public sealed class CheckBetsQueryHandler 
        : IQueryHandler<CheckBetsQuery, IReadOnlyCollection<BetViewModel>>
    {
        private IBetQueryRepository _betRepository;

        public CheckBetsQueryHandler(IBetQueryRepository betRepository)
        {
            _betRepository = betRepository ?? throw new ArgumentNullException(nameof(betRepository));
        }

        public async Task<IReadOnlyCollection<BetViewModel>> Handle(CheckBetsQuery request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            return await _betRepository.GetBetsForMemberAsync(request.MemberId);
        }

        private static void ValidateRequest(CheckBetsQuery request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request), $"request cannot be null");
        }
    }
}
