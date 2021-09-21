namespace BetFriend.Bet.Application.Usecases.LaunchBet
{
    using BetFriend.Bet.Application.Abstractions.Command;
    using BetFriend.Bet.Domain;
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class LaunchBetCommandHandler : ICommandHandler<LaunchBetCommand>
    {
        private readonly IBetRepository _betRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IAuthenticationGateway _authenticationGateway;

        public LaunchBetCommandHandler(IBetRepository betRepository, IMemberRepository memberRepository, IAuthenticationGateway authenticationGateway)
        {
            _betRepository = betRepository ?? throw new ArgumentNullException(nameof(betRepository), $"{nameof(betRepository)} cannot be null");
            _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository), $"{nameof(memberRepository)} cannot be null");
            _authenticationGateway = authenticationGateway;
        }

        public async Task<Unit> Handle(LaunchBetCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            if (!_authenticationGateway.IsAuthenticated(request.CreatorId))
                throw new NotAuthenticatedException();
            var member = await _memberRepository.GetByIdAsync(new(request.CreatorId)).ConfigureAwait(false)
                            ?? throw new MemberUnknownException("Creator is unknown");

            var bet = member.CreateBet(new BetId(request.BetId),
                                        request.EndDate,
                                        request.Description,
                                        request.Coins,
                                        request.CreationDate.Now);

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
