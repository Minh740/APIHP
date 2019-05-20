using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class WorkingDay
    {

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Day Wday { get; set; }

    }

    public enum Day
    {
        None,
        MonDay,
        TuesDay,
        WednesDay,
        ThursDay,
        FriDay,
        StaturDay,
        Sunday

    }
}
