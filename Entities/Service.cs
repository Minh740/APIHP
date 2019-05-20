using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("Service")]
    public class Service : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("archive_status")]
        public int ArchiveStatus { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("duration")]
        public double Duration { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("tax")]
        public double Tax { get; set; }
        [Column("discount")]
        public double Discount { get; set; }
        [NotMapped]
        public int adjust_duration { get; set; }
        [NotMapped]
        public int adjust_staffid { get; set; }

        public List<Extra> ListExtra { get; set; }

    }
}
