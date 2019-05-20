using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("status")]
        public int status { get; set; }
        [Column("registration_status")]
        public int registration_status { get; set; }
        [Column("full_name")]
        public string full_name { get; set; }
        [Column("first_name")]
        public string first_name { get; set; }
        [Column("last_name")]
        public string last_name { get; set; }
        [Column("email")]
        public string email { get; set; }
        [Column("phone")]
        public string phone { get; set; }
        [Column("password")]
        public string password { get; set; }
        [Column("credit")]
        public float credit { get; set; }
        [Column("star")]
        public int star { get; set; }
        [Column("birthday")]
        public DateTime? birthday { get; set; }
        [Column("address")]
        public string address { get; set; }
        [Column("city_id")]
        public int city_id { get; set; }
        [Column("state_id")]
        public int state_id { get; set; }
        [Column("create_date")]
        public DateTime? create_date { get; set; }
        [Column("last_login_time")]
        public DateTime? last_login_time { get; set; }
        [Column("biomaticcode")]
        public string biomaticcode { get; set; }
        [Column("token")]
        public string token { get; set; }
        [Column("pin")]
        public string pin { get; set; }
    }
}
