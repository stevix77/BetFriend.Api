namespace BetFriend.Bet.Application.Usecases.CreateMember
{
    using BetFriend.Bet.Domain.Exceptions;
    using BetFriend.Bet.Domain.Members;
    using BetFriend.Shared.Application.Abstractions.Command;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;


    public sealed class CreateMemberCommandHandler : ICommandHandler<CreateMemberCommand>
    {
        private const int INIT_WALLET = 1000;
        private readonly IMemberRepository _memberRepository;

        public CreateMemberCommandHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Unit> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            if (await _memberRepository.GetByIdAsync(new MemberId(request.MemberId)) != null)
                throw new MemberAlreadyExistsException();
            var member = Member.Create(new MemberId(request.MemberId), request.MemberName, INIT_WALLET);
            await _memberRepository.SaveAsync(member);
            return Unit.Value;
        }
    }
}
