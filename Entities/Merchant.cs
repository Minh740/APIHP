using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    [Table("Merchant")]
    public class Merchant : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("approve_status")]
        public int ApproveStatus { get; set; }

        [Column("business_name")]
        public string BusinessName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("business_hours")]
        public string BusinessHour { get; set; }

        [Column("tax_id")]
        public string TaxId { get; set; }

        [Column("driver_license")]
        public string DriverLicense { get; set; }

        [Column("pin")]
        public string Pin { get; set; }

        [Column("address")]
        public string Address { get; set; }
        [Column("city_id")]
        public int? CityId { get; set; }
        [Column("state_id")]
        public int? StateId { get; set; }
        [Column("store_id")]
        public int? Storeid { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("phone")]
        public string phone { get; set; }

        [Column("zip")]
        public string zip { get; set; }
        [Column("bankname")]
        public string bankname { get; set; }
        [Column("account_number")]
        public string account_number { get; set; }
        [Column("routing_number")]
        public string routing_number { get; set; }

        [Column("ein")]
        public string ein { get; set; }

        [Column("voidcheck")]
        public string void_check { get; set; }




    }
}
