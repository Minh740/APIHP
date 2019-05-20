using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class Test
    {
        public int Id { get; set; }
        [Column("name_test")]
        public string name { get; set; }
    }
}
