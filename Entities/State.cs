using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("State")]
    public class State : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("country_id")]
        public int CountryId { get; set; }
        [Column("name")]
        public string Name { get; set; }

    }
}
