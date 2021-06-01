using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BetFriend.Infrastructure.DataAccess
{
    public class BetContext : DbContext
    {
        public BetContext([NotNull] DbContextOptions options) : base(options)
        {
            
        }
    }
}
