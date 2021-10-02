namespace BetFriend.UserAccess.Infrastructure.DataAccess
{
    using BetFriend.UserAccess.Infrastructure.DataAccess.Entities;
    using Microsoft.EntityFrameworkCore;
    using System.Diagnostics.CodeAnalysis;

    public sealed class UserAccessContext : DbContext
    {
        public UserAccessContext([NotNull] DbContextOptions<UserAccessContext> options) : base(options)
        {

        }

        public DbSet<UserEntity> Users { get; set; }
    }
}
