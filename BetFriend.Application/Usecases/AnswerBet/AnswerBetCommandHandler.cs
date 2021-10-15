namespace BetFriend.Bet.Application.Usecases.AnswerBet
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Application.Abstractions.Command;
    using BetFriend.Shared.Domain;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;


    public sealed class AnswerBetCommandHandler : ICommandHandler<AnswerBetCommand>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IBetRepository _betRepository;
        private readonly IAuthenticationGateway _authentificationGateway;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AnswerBetCommandHandler(IMemberRepository memberRepository, IBetRepository betRepository, IAuthenticationGateway authentificationGateway, IDateTimeProvider dateTimeProvider)
        {
            _memberRepository = memberRepository;
            _betRepository = betRepository;
            _authentificationGateway = authentificationGateway;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(AnswerBetCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            if (!_authentificationGateway.IsAuthenticated())
                throw new NotAuthenticatedException();

            var member = await _memberRepository.GetByIdAsync(new MemberId(_authentificationGateway.UserId)).ConfigureAwait(false) ??
                throw new MemberUnknownException("Member unknown");
            var bet = await _betRepository.GetByIdAsync(new BetId(request.BetId)).ConfigureAwait(false) ??
                throw new BetUnknownException($"Bet {request.BetId} is unknown");

            member.Answer(bet, request.IsAccepted, _dateTimeProvider.Now);
            await _betRepository.SaveAsync(bet).ConfigureAwait(false);

            return Unit.Value;
        }

        private static void ValidateRequest(AnswerBetCommand request)
        {
            if (request is null)
                throw new System.ArgumentNullException(nameof(request), $"{nameof(request)} cannot be null");
        }
    }
}
