using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("Service_Extra")]
    public class Service_Extra : BaseEntity
    {

        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("status")]
        public int status { get; set; }
        [Column("service_id")]
        public int? ServiceId { get; set; }
        [Column("extra_id")]
        public int? ExtraId { get; set; }


    }
}
