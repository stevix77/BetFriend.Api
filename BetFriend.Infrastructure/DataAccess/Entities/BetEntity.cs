﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.Infrastructure.DataAccess.Entities
{
    [Table("bet")]
    public class BetEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

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
        [Column("member_id"), ForeignKey(nameof(MemberId))]
        public Guid MemberId { get; set; }
        [Required]
        [Column("creation_date")]
        public DateTime CreationDate { get; set; }
    }
}
