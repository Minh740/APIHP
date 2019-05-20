using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("Staff")]
    public class Staff : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("working_status")]
        public int WorkingStatus { get; set; }

        [Column("roleid")]
        public int RoleId { get; set; }

        [Column("store_id")]
        public int StoreId { get; set; }

        [Column("first_name")]
        public string first_name { get; set; }

        [Column("last_name")]
        public string last_name { get; set; }

        [Column("display_name")]
        public string display_name { get; set; }

        [Column("address")]
        public string address { get; set; }
        [Column("city_id")]
        public int city_id { get; set; }
        [Column("state_id")]
        public int state_id { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("pin")]
        public string PIN { get; set; }

        [Column("salary_perhour")]
        public string salary_perhour { get; set; }

        [Column("salary_commission")]
        public string salary_commission { get; set; }

        [Column("tip_fee_percent")]
        public float tip_fee_percent { get; set; }

        [Column("tip_fee_fixed")]
        public float tip_fee_fixed { get; set; }

        [Column("driver_license")]
        public string DriverLicense { get; set; }

        [Column("professional_license")]
        public string ProfessionalLicense { get; set; }

        [Column("social_security_number")]
        public string SocialSecurityNumber { get; set; }


        [Column("longitude")]
        public double longitude { get; set; }


        [Column("latitude")]
        public double latitude { get; set; }

        [Column("workingtime")]
        public string WorkingTime { get; set; }

        [Column("imageurl")]
        public string imageurl { get; set; }

        [Column("ordernumber")]
        public int orderNumber { get; set; }


        [Column("activestatus")]
        public int activestatus { get; set; }


        [NotMapped]
        public string RoleName { get; set; }



    }
}
