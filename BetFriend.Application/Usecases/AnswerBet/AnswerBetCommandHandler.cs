﻿using BetFriend.Bet.Application.Abstractions.Command;
using BetFriend.Bet.Domain.Bets;
using BetFriend.Bet.Domain.Exceptions;
using BetFriend.Bet.Domain.Members;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BetFriend.Bet.Application.Usecases.AnswerBet
{
    public sealed class AnswerBetCommandHandler : ICommandHandler<AnswerBetCommand>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IBetRepository _betRepository;

        public AnswerBetCommandHandler(IMemberRepository memberRepository, IBetRepository betRepository)
        {
            _memberRepository = memberRepository;
            _betRepository = betRepository;
        }

        public async Task<Unit> Handle(AnswerBetCommand request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var member = await _memberRepository.GetByIdAsync(new(request.MemberId)).ConfigureAwait(false) ??
                throw new MemberUnknownException("Member unknown");
            var bet = await _betRepository.GetByIdAsync(new(request.BetId)).ConfigureAwait(false) ??
                throw new BetUnknownException($"Bet {request.BetId} is unknown");

            member.Answer(bet, request.IsAccepted, request.DateTimeProvider.Now);
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
