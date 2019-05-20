using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{

    [Table("Device")]
    public class Device : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("confirm_status")]
        public int ConfirmStatus { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("device_id")]
        public string DeviceId { get; set; }
        [Column("register_id")]
        public string Token { get; set; }
        [Column("platform")]
        public string Platform { get; set; }
        [Column("version")]
        public string Version { get; set; }
        [Column("manufacture")]
        public string Manufacture { get; set; }
        [Column("is_virtual")]
        public string IsVitural { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("ip")]
        public string IP { get; set; }

        public User User { get; set; }

    }
}
