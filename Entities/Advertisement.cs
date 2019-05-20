using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities
{
    [Table("Advertisement")]
    public class Advertisement
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("store_id")]
        public int StoreId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("create_date")]
        public DateTime CreateDate { get; set; }
        [Column("end_date")]
        public DateTime EndDate { get; set; }
        [Column("created_ip")]
        public string CreateIP { get; set; }
    }
}
