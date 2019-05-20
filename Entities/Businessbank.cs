using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("Businessbank")]
    public class Businessbank : BaseEntity
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("routing_number")]
        public int routing_number { get; set; }
        [Column("dda")]
        public int dda { get; set; }
        [Column("imageurl")]
        public int imageurl { get; set; }
        [Column("general_id")]
        public int general_id { get; set; }


    }
}
