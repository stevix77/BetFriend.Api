namespace BetFriend.Application.Usecases.LaunchBet
{
    using BetFriend.Application.Abstractions;
    using BetFriend.Application.Abstractions.Command;
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using MediatR;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class LaunchBetCommandHandler : ICommandHandler<LaunchBetCommand>
    {
        private readonly IBetRepository _betRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IDomainEventsListener _domainEventsListener;

        public LaunchBetCommandHandler(IBetRepository betRepository, IMemberRepository memberRepository, IDomainEventsListener domainEventsListener)
        {
            _betRepository = betRepository ?? throw new ArgumentNullException(nameof(betRepository), $"{nameof(betRepository)} cannot be null");
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository), $"{nameof(memberRepository)} cannot be null");
            _domainEventsListener = domainEventsListener ?? throw new ArgumentNullException(nameof(domainEventsListener), $"{nameof(domainEventsListener)} cannot be null");
        }

        public async Task<Unit> Handle(LaunchBetCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var member = await _memberRepository.GetByIdAsync(request.CreatorId).ConfigureAwait(false)
                            ?? throw new MemberUnknownException("Creator is unknown");

            var bet = member.CreateBet(new BetId(request.BetId),
                                        request.EndDate,
                                        request.Description,
                                        request.Coins);

            _domainEventsListener.AddDomainEvents(bet.DomainEvents);
            await _betRepository.SaveAsync(bet);
            
            return Unit.Value;
        }

        private static void ValidateRequest(LaunchBetCommand request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request), $"{nameof(request)} cannot be null");
        }
    }


}
