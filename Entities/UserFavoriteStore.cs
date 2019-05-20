using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("UserFavoriteStore")]
    public class UserFavoriteStore : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("store_id")]
        public int StoreId { get; set; }
    }
}
