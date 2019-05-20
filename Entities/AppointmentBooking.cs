using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class AppointmentBooking
    {


        public int? Id { get; set; }
        public string UserFullName { get; set; }
        public string PhoneNumber { get; set; }
        public AppointmentOption Options { get; set; }
        public string Status { get; set; }
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int StaffId { get; set; }

        public float Duration { get; set; }

        public Appointment app { get; set; }
        public string staff_name { get; set; }

        public AppointmentBooking(Appointment app, User user)
        {
            Options = new AppointmentOption();

            Id = app.Id;
            if (user.full_name != null)
                UserFullName = user.full_name;

            if (user.phone != null)
                PhoneNumber = user.phone;

            if (app.ListEx != null)
                Options.Extra = app.ListEx;



            if (app.ListSer != null)
                Options.Service = app.ListSer;
            if (app.ListPro != null)
                Options.Product = app.ListPro;

            if (app.CheckinStatus != null)
                Status = app.CheckinStatus;

            StaffId = (int)app.Staff_id;
            Start = DateTimeOffset.Parse(app.FromTime.ToString(), null).DateTime;
            End = DateTimeOffset.Parse(app.ToTime.ToString(), null).DateTime;
            this.Duration = app.duration;

            this.app = app;
            this.staff_name = app.staff_name;

        }
    }
}
