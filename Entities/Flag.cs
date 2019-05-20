using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("Flag")]
    public class Flag : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("storeid")]
        public int StoreId { get; set; }
        [Column("flag")]
        public int flag { get; set; }
        [Column("token")]
        public string Token { get; set; }

    }
}
