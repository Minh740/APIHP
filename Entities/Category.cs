using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("Category")]
    public class Category : BaseEntity
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
        public string CategoryName { get; set; }

        [Column("category_style")]
        public string Categorystyle { get; set; }

        public List<Product> Products { get; set; }
        public List<Service> Service { get; set; }

    }
}
