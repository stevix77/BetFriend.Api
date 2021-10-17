using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.Bet.Infrastructure.DataAccess.Entities
{
    [Table("Member")]
    public class MemberEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Key, Required, Column("Member_id")]
        public Guid MemberId { get; set; }
        [Required]
        [Column("Wallet")]
        public int Wallet { get; set; }

        [Required, Column("Member_name")]
        public string MemberName { get; set; }
    }
}
