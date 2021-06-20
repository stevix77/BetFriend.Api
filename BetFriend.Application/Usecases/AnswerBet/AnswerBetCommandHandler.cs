using BetFriend.Application.Abstractions;
using BetFriend.Domain.Bets;
using BetFriend.Domain.Exceptions;
using BetFriend.Domain.Members;
using MediatR;
using System.Threading.Tasks;

namespace BetFriend.Application.Usecases.AnswerBet
{
    public sealed class AnswerBetCommandHandler
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IBetRepository _betRepository;

        public AnswerBetCommandHandler(IMemberRepository memberRepository, IBetRepository betRepository)
        {
            _memberRepository = memberRepository;
            _betRepository = betRepository;
        }

        public async Task<Unit> Handle(AnswerBetCommand request)
        {
            ValidateRequest(request);

            var member = await _memberRepository.GetByIdAsync(request.MemberId).ConfigureAwait(false) ??
                throw new MemberUnknownException("Member unknown");
            var bet = await _betRepository.GetByIdAsync(request.BetId).ConfigureAwait(false) ??
                throw new BetUnknownException($"Bet {request.BetId} is unknown");

            member.Answer(bet, request.IsAccepted, request.DateAnswer);
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
