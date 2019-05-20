using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("BookingExtra")]
    public class BookingExtra : BaseEntity
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("status")]
        public int status { get; set; }
        [Column("extra_id")]
        public int extra_id { get; set; }
        [Column("appointment_id")]
        public int appointment_id { get; set; }



    }
}
