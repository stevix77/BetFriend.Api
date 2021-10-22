using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.Bet.Infrastructure.DataAccess.Entities
{
    [Table("bet")]
    public class BetEntity
    {
        public BetEntity()
        {
            Answers = new HashSet<AnswerEntity>();
        }

        [Required]
        [Key]
        [Column("bet_id")]
        public Guid BetId { get; set; }
        
        [Required]
        [Column("description", TypeName = "varchar(max)")]
        public string Description { get; set; }
        
        [Column("coins")]
        public int Coins { get; set; }
        
        [Required]
        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Column("member_id"), ForeignKey(nameof(CreatorId))]
        public Guid CreatorId { get; set; }
        public MemberEntity Creator { get; set; }


        [Required]
        [Column("creation_date")]
        public DateTime CreationDate { get; set; }
        [Column("close_date")]
        public DateTime? CloseDate { get; set; }
        [Column("is_success")]
        public bool? IsSuccess { get; set; }

        public virtual ICollection<AnswerEntity> Answers { get; set; }
        public bool IsClosed { get => CloseDate.HasValue; }
    }
}
