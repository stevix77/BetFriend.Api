using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.Infrastructure.DataAccess.Entities
{
    [Table("answer")]
    public partial class AnswerEntity
    {
        [Column("bet_id")]
        [InverseProperty(nameof(BetEntity.Id))]
        [ForeignKey(nameof(BetEntity.Id))]
        [Key]
        public int BetId { get; set; }

        [Column("member_id")]
        [InverseProperty(nameof(MemberEntity.Id))]
        [ForeignKey(nameof(MemberEntity.Id))]
        [Key]
        public int MemberId { get; set; }

        [InverseProperty(nameof(MemberEntity.Id))]
        public MemberEntity Member { get; set; }

        [InverseProperty(nameof(BetEntity.Id))]
        public BetEntity Bet { get; set; }

        [Column("is_accepted")]
        [Key]
        public bool IsAccepted { get; set; }

        [Column("date_answer")]
        public DateTime DateAnswer { get; set; }
    }
}
