using BetFriend.Application.Abstractions.Command;
using BetFriend.Domain.Bets;
using BetFriend.Domain.Exceptions;
using BetFriend.Domain.Members;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BetFriend.Application.Usecases.UpdateWallet
{
    public class UpdateWalletMembersCommandHandler : ICommandHandler<UpdateWalletMembersCommand>
    {
        private readonly IBetRepository _betRepository;
        private readonly IMemberRepository _memberRepository;

        public UpdateWalletMembersCommandHandler(IBetRepository betRepository, IMemberRepository memberRepository)
        {
            _betRepository = betRepository;
            _memberRepository = memberRepository;
        }

        public async Task<Unit> Handle(UpdateWalletMembersCommand request, CancellationToken cancellationToken)
        {
            var bet = await _betRepository.GetByIdAsync(request.BetId)
                    ?? throw new BetUnknownException($"This bet with id {request.BetId} is unknown");
            var creator = await _memberRepository.GetByIdAsync(bet.State.Creator.Id.Value).ConfigureAwait(false);
            creator.WonBet(bet);

            foreach(var answer in bet.State.Answers)
            {
                var member = await _memberRepository.GetByIdAsync(answer.MemberId).ConfigureAwait(false);
                member.LostBet(bet);
            }

            return Unit.Value;
        }
    }
}
