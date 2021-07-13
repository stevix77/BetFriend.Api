using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.Infrastructure.DataAccess.Entities
{
    [Table("user")]
    public class MemberEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Key, Required, Column("member_id")]
        public Guid MemberId { get; set; }
        [Required]
        [Column("wallet")]
        public int Wallet { get; set; }

        [Required, Column("member_username")]
        public string MemberName { get; set; }
    }
}
