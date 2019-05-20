using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("General")]
    public class General : BaseEntity
    {

        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("status")]
        public int status { get; set; }
        [Column("legal_business_name")]
        public string legal_business_name { get; set; }
        [Column("do_business_name")]
        public string do_business_name { get; set; }
        [Column("tax")]
        public string tax { get; set; }
        [Column("address")]
        public string address { get; set; }
        [Column("city_id")]
        public int city_id { get; set; }
        [Column("state_id")]
        public int state_id { get; set; }
        [Column("zip")]
        public string zip { get; set; }
        [Column("phone_business")]
        public string phone_business { get; set; }
        [Column("emai_contact")]
        public string emai_contact { get; set; }
        [Column("first_name")]
        public string first_name { get; set; }
        [Column("last_name")]
        public string last_name { get; set; }
        [Column("tittle")]
        public string tittle { get; set; }
        [Column("phone_contact")]
        public string phone_contact { get; set; }

    }
}
