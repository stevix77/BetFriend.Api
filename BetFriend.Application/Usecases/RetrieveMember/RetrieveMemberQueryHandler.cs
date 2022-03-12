using BetFriend.Bet.Application.Abstractions.Repository;
using BetFriend.Bet.Application.Models;
using BetFriend.Shared.Application.Abstractions.Query;
using System.Threading;
using System.Threading.Tasks;

namespace BetFriend.Bet.Application.Usecases.RetrieveMember
{
    public class RetrieveMemberQueryHandler : IQueryHandler<RetrieveMemberQuery, MemberDto>
    {
        private readonly IQueryMemberRepository _memberRepository;

        public RetrieveMemberQueryHandler(IQueryMemberRepository queryMemberRepository)
        {
            _memberRepository = queryMemberRepository;
        }

        public async Task<MemberDto> Handle(RetrieveMemberQuery query, CancellationToken cancellationToken)
        {
            return await _memberRepository.GetByIdAsync(query.MemberId).ConfigureAwait(false);
        }
    }
}