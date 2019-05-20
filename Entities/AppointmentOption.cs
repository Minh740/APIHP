using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{

    public class AppointmentOption
    {

        public List<Service> Service { get; set; }

        public List<Extra> Extra { get; set; }

        public List<Product> Product { get; set; }

        public AppointmentOption()
        {
            Extra = new List<Extra>();
            Service = new List<Service>();
            Product = new List<Product>();
        }

    }
}
