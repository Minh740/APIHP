using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{

    [Table("GiftCard")]
    public class GiftCard : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("sending_status")]
        public int SendingStatus { get; set; }
        [Column("sender_id")]
        public int SẻnderId { get; set; }
        [Column("receiver_id")]
        public int ReceiveUserId { get; set; }
        [Column("gift_Code")]
        public string GiftCode { get; set; }
        [Column("note")]
        public string Note { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("fee_percent")]
        public double FeePercent { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("sent_date")]
        public DateTime SendDate { get; set; }
        [Column("ip")]
        public string IP { get; set; }

    }
}
