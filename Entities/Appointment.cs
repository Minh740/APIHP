using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Appointment")]
    public class Appointment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("paid_status")]
        public bool PaidStatus { get; set; }
        [Column("checkin_status")]
        public string CheckinStatus { get; set; }
        [Column("store_id")]
        public int? StoreId { get; set; }
        [Column("customer_id")]
        public int? CustomerId { get; set; }
        [Column("user_id")]
        public int? User_id { get; set; }

        [Column("from_time")]
        public DateTime FromTime { get; set; }

        [Column("to_time")]
        public DateTime ToTime { get; set; }

        [Column("tip_percent")]
        public float TipPercent { get; set; }

        [Column("ip")]
        public string IP { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("code")]
        public string Code { get; set; }
        [Column("total")]
        public double total { get; set; }
        [Column("staff_id")]
        public int? Staff_id { get; set; }

        [Column("duration")]
        public float duration { get; set; }

        [Column("waitingtime")]
        public float waitingtime { get; set; }
        [NotMapped]
        public string staff_name { get; set; }



        public List<Service> ListSer { get; set; }

        public List<Product> ListPro { get; set; }
        public List<Extra> ListEx { get; set; }

        public List<BookingService> BookingServiceList { get; set; }

        public int[] BookingServices { get; set; }
        public int[] BookingProducts { get; set; }
        public int[] BookingExtras { get; set; }

        public string[] BookingServices2 { get; set; }
        [NotMapped]
        public string PhoneNo { get; set; }
    }
}
