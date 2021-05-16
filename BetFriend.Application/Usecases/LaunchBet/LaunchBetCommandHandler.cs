namespace BetFriend.Application.Usecases.LaunchBet
{
    using BetFriend.Domain.Bets;
    using BetFriend.Domain.Exceptions;
    using BetFriend.Domain.Members;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class LaunchBetCommandHandler
    {
        private readonly IBetRepository _betRepository;
        private readonly IMemberRepository _memberRepository;

        public LaunchBetCommandHandler(IBetRepository betRepository, IMemberRepository memberRepository)
        {
            _betRepository = betRepository;
            _memberRepository = memberRepository;
        }

        public async Task Handle(LaunchBetCommand request, CancellationToken cancellationToken)
        {
            if (request.EndDate <= DateTime.UtcNow)
                throw new EndDateNotValidException("The end date is before the current date");

            
            if (!await _memberRepository.ExistsAllAsync(request.Participants))
                throw new MemberUnknownException("Some member are unknown");

            await _betRepository.AddAsync(Bet.Create(request.BetId, request.MemberId, request.EndDate, request.Participants));
        }
    }

    public class InMemoryMemberRepository : IMemberRepository
    {
        private readonly List<MemberId> _memberIds;
        public InMemoryMemberRepository(List<MemberId> memberIds = null)
        {
            _memberIds = memberIds ?? new List<MemberId>();
        }

        public Task<bool> ExistsAllAsync(MemberId[] participants)
        {
            return Task.FromResult(_memberIds.Exists(x => participants.Contains(x)));
        }
    }

    
}
