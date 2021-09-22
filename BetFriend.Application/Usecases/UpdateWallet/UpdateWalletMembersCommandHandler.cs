namespace BetFriend.Bet.Application.Usecases.UpdateWallet
{
    using BetFriend.Bet.Domain.Bets;
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Application.Abstractions.Command;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;


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
            var bet = await _betRepository.GetByIdAsync(new(request.BetId))
                    ?? throw new BetUnknownException($"This bet with id {request.BetId} is unknown");
            bet.UpdateWallets();
            var members = bet.GetAllMembers();
            await _memberRepository.SaveAsync(members).ConfigureAwait(false);

            return Unit.Value;
        }
    }
}
