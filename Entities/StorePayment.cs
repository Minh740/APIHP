using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("StorePayment")]
    public class StorePayment : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("store_id")]
        public int StoreId { get; set; }
        [Column("payment_id")]
        public int PaymentId { get; set; }

        public Store Store { get; set; }
        public Payment Payment { get; set; }
    }
}
