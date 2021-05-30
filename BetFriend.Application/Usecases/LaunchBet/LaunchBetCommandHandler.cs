namespace BetFriend.Application.Usecases.LaunchBet
{
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class LaunchBetCommandHandler
    {
        private readonly IBetRepository _betRepository;
        private readonly IMemberRepository _memberRepository;

        public LaunchBetCommandHandler(IBetRepository betRepository, IMemberRepository memberRepository)
        {
            _betRepository = betRepository ?? throw new ArgumentNullException(nameof(betRepository), $"{nameof(betRepository)} cannot be null");
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository), $"{nameof(memberRepository)} cannot be null");
        }

        public async Task Handle(LaunchBetCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            if (!await _memberRepository.ExistsAllAsync(new List<Guid>(request.Participants) { request.CreatorId }.ToArray()))
                throw new MemberUnknownException("Some members are unknown");

            var bet = Bet.Create(new BetId(request.BetId),
                                 new MemberId(request.CreatorId),
                                 request.EndDate,
                                 request.Description,
                                 request.Participants
                                        .Select(x => new MemberId(x))
                                        .ToArray());

            await _betRepository.AddAsync(bet);
        }

        private static void ValidateRequest(LaunchBetCommand request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request), $"{nameof(request)} cannot be null");
        }
    }


}
