using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("Customer")]
    public class Customer : BaseEntity
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
        [Column("full_name")]
        public string FullName { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("phone")]
        public string Phone { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("city_id")]
        public int CityId { get; set; }
        [Column("state_id")]
        public int StateId { get; set; }


        public City City { get; set; }
        public State State { get; set; }
    }
}
