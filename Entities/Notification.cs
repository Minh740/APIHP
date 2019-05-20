using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Notification")]
    public class Notification : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("confirm_status")]
        public int ConfirmStatus { get; set; }
        [Column("store_id")]
        public int StoreId { get; set; }


        [Column("receiver_id")]
        public int ReceiverId { get; set; }
        [Column("sender_id")]
        public int SenderId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("type")]
        public string Type { get; set; }


        [Column("money")]
        public float Money { get; set; }

        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Column("send_date")]
        public DateTime SendDate { get; set; }

        [Column("receive_date")]
        public DateTime ReceiveDate { get; set; }

        [Column("view")]
        public int View { get; set; }
        [Column("notify_status")]
        public string NotifyStatus { get; set; }
    }
}
