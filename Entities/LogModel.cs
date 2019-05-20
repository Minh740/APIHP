using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Log")]
    public class LogModel : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("created_date")]
        public DateTime CreateDate { get; set; }
    }
}
