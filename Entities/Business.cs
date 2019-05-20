using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    [Table("Business")]
    public class Business : BaseEntity
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("status")]
        public int Status { get; set; }
        [Column("answer_1")]
        public bool answer_1 { get; set; }
        [Column("answer_2")]
        public bool answer_2 { get; set; }
        [Column("answer_3")]
        public bool answer_3 { get; set; }
        [Column("answer_4")]
        public bool answer_4 { get; set; }
        [Column("answer_5")]
        public bool answer_5 { get; set; }
        [Column("detail_answer_1")]
        public string detail_answer_1 { get; set; }
        [Column("detail_answer_2")]
        public string detail_answer_2 { get; set; }
        [Column("detail_answer_3")]
        public string detail_answer_3 { get; set; }
        [Column("detail_answer_4")]
        public string detail_answer_4 { get; set; }
        [Column("general_id")]
        public int general_id { get; set; }
    }
}
