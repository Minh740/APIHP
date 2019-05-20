using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("City")]
    public class City : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("country_id")]
        public int CountryId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("postCode")]
        public string PostCode { get; set; }

        public State State { get; set; }
    }
}
