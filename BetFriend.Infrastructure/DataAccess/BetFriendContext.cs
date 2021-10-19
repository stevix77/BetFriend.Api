using BetFriend.Bet.Infrastructure.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BetFriend.Bet.Infrastructure.DataAccess
{
    public class BetFriendContext : DbContext
    {
        public BetFriendContext([NotNull] DbContextOptions<BetFriendContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerEntity>()
                .HasKey(x => new
                {
                    x.BetId,
                    x.MemberId,
                    x.IsAccepted
                });


        }

        public DbSet<BetEntity> Bets { get; set; }
        public DbSet<MemberEntity> Members { get; set; }

    }
}
