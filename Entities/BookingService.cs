using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("BookingService")]
    public class BookingService : BaseEntity
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("status")]
        public int status { get; set; }
        [Column("staff_id")]
        public int staff_id { get; set; }
        [Column("service_id")]
        public int service_id { get; set; }
        [Column("appointment_id")]
        public int appointment_id { get; set; }
        [Column("duration")]
        public float duration { get; set; }



        public List<BookingExtra> BookingExtras { get; set; }
    }
}
