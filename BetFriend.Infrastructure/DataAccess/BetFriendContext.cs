using BetFriend.Infrastructure.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BetFriend.Infrastructure.DataAccess
{
    public class BetFriendContext : DbContext
    {
        public BetFriendContext([NotNull] DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<BetEntity> Bets { get; set; }
        public DbSet<MemberEntity> Members { get; set; }

    }
}
