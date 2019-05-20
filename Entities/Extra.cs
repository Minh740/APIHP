using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("Extra")]
    public class Extra : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int status { get; set; }
        [Column("archive_status")]
        public int ArchiveStatus { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("tax")]
        public double Tax { get; set; }
        [Column("discount")]
        public double Discount { get; set; }

        [Column("store_id")]
        public int StoreId { get; set; }

        [Column("duration")]
        public float duration { get; set; }

    }
}
