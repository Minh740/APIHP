using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Store")]
    public class Store : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("merchantid")]
        public int merchantid { get; set; }
        [Column("name")]
        public string StoreName { get; set; }
        [Column("phone")]
        public string PhoneNumber { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("language")]
        public string Language { get; set; }
        [Column("about")]
        public string About { get; set; }
        [Column("rating")]
        public string Rating { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("city_id")]
        public int CityId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }
        [Column("latitude")]
        public double Lat { get; set; }
        [Column("longitude")]
        public double Long { get; set; }
        [Column("open_time")]
        public string open_time { get; set; }
        [Column("close_time")]
        public string close_time { get; set; }



    }
}
