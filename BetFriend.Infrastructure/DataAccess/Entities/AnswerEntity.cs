using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.Bet.Infrastructure.DataAccess.Entities
{
    [Table("answer")]
    public partial class AnswerEntity
    {
        [Column("bet_id")]
        [ForeignKey(nameof(BetEntity.BetId))]
        [Key]
        public Guid BetId { get; set; }

        [Column("member_id")]
        [ForeignKey(nameof(MemberEntity.MemberId))]
        [Key]
        public Guid MemberId { get; set; }

        public MemberEntity Member { get; set; }

        public BetEntity Bet { get; set; }

        [Column("is_accepted")]
        [Key]
        public bool IsAccepted { get; set; }

        [Column("date_answer")]
        public DateTime DateAnswer { get; set; }
    }
}
