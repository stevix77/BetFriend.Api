using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.Infrastructure.DataAccess.Entities
{
    [Table("bets")]
    public class Bet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Guid BetId { get; set; }
        [Required]
        public string Description { get; set; }
        public int Tokens { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
