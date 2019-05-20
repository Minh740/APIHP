using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class AssignAppointment
    {
        public int memberid { get; set; }
        public int appointmentid { get; set; }

        public DateTime Star { get; set; }

        public DateTime End { get; set; }
    }
}
