using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("UserFavoriteStaff")]
    public class UserFavoriteStaff : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("staff_id")]
        public int StaffId { get; set; }
    }
}
