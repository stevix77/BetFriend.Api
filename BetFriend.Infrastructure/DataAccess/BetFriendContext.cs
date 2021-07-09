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
