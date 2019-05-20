using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("StaffService")]
    public class StaffService : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("staff_id")]
        public int StaffId { get; set; }
        [Column("service_id")]
        public int ServiceId { get; set; }

        public Staff Staff { get; set; }
        public Service Service { get; set; }
    }
}
