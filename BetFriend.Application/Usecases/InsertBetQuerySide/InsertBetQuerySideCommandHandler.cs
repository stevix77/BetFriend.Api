namespace BetFriend.Application.Usecases.InsertBetQuerySide
{
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Application.Abstractions.Repository;
    using BetFriend.Application.ViewModels;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class InsertBetQuerySideCommandHandler : ICommandHandler<InsertBetQuerySideCommand, Unit>
    {
        private readonly IBetRepository _betRepository;
        private readonly IBetQueryRepository _queryBetRepository;
        private readonly IMemberRepository _memberRepository;

        public InsertBetQuerySideCommandHandler(IBetRepository betRepository, IBetQueryRepository queryBetRepository, IMemberRepository memberRepository)
        {
            _betRepository = betRepository;
            _queryBetRepository = queryBetRepository;
            _memberRepository = memberRepository;
        }

        public async Task<Unit> Handle(InsertBetQuerySideCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var member = await _memberRepository.GetByIdAsync(request.MemberId).ConfigureAwait(false)
                        ?? throw new MemberUnknownException($"MemberId: {request.MemberId} is unknown");
            var bet = await _betRepository.GetByIdAsync(request.BetId).ConfigureAwait(false)
                    ?? throw new BetUnknownException($"BetId: {request.BetId} is unknwon");
            await _queryBetRepository.SaveAsync(new BetViewModel
            {
                Coins = bet.State.Coins,
                CreatorId = member.MemberId,
                CreatorUsername = member.MemberName,
                Description = bet.State.Description,
                EndDate = bet.State.EndDate,
                Id = bet.State.BetId
            });
            return Unit.Value;
        }

        private static void ValidateRequest(InsertBetQuerySideCommand request)
        {
            if (request is null)
                throw new ArgumentNullException();
        }
    }
}
