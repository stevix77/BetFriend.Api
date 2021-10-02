using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetFriend.UserAccess.Infrastructure.DataAccess.Entities
{
    [Table("User")]
    public class UserEntity
    {
        [Required]
        [Key]
        [Column("user_id")]
        public string UserId { get; set; }
        [Column("password")] 
        public string Password { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("register_date")]
        public DateTime RegisterDate { get; set; }
    }
}
