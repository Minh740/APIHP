using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("Principal")]
    public class Principal : BaseEntity
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("status")]
        public int status { get; set; }
        [Column("first_name")]
        public string first_name { get; set; }
        [Column("last_name")]
        public string last_name { get; set; }
        [Column("title")]
        public string title { get; set; }
        [Column("owner_ship")]
        public string owner_ship { get; set; }
        [Column("home_phone")]
        public string home_phone { get; set; }
        [Column("mobile_phone")]
        public string mobile_phone { get; set; }
        [Column("year_address")]
        public string year_address { get; set; }
        [Column("ssn")]
        public string ssn { get; set; }
        [Column("birthday")]
        public string birthday { get; set; }
        [Column("driver_number")]
        public string driver_number { get; set; }
        [Column("driver_imageurl")]
        public string driver_imageurl { get; set; }
        [Column("general_id")]
        public int general_id { get; set; }
        [Column("city_id")]
        public int city_id { get; set; }
        [Column("state_id")]
        public int state_id { get; set; }
        [Column("address")]
        public string address { get; set; }
        [Column("zip")]
        public string zip { get; set; }

    }
}
