using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Product")]
    public class Product : BaseEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("archive_status")]
        public int ArchiveStatus { get; set; }
        [Column("store_id")]
        public int StoreId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("sku")]
        public string SKU { get; set; }
        [Column("barcode")]
        public string BarCode { get; set; }
        [Column("price")]
        public double Price { get; set; }
        [Column("tax")]
        public double Tax { get; set; }
        [Column("discount")]
        public double Discount { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }


        public Store Store { get; set; }



    }
}
