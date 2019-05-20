using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("BookingProduct")]
    public class BookingProduct : BaseEntity
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("status")]
        public int status { get; set; }
        [Column("appointment_id")]
        public int appointment_id { get; set; }
        [Column("product_id")]
        public int product_id { get; set; }
        [Column("quantity")]
        public int quantity { get; set; }
    }
}
