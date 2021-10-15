using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.Bet.Infrastructure.DataAccess.Entities
{
    [Table("user")]
    public class MemberEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Key, Required, Column("user_id")]
        public string MemberId { get; set; }
        [Required]
        [Column("wallet")]
        public int Wallet { get; set; }

        [Required, Column("username")]
        public string MemberName { get; set; }
    }
}
