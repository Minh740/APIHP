using System;

namespace Entities
{
    public class AppointmentDetail
    {

        public int Id { get; set; }

        public string CheckInStatus { get; set; }

        public string Store_phone { get; set; }

        public string Store_name { get; set; }
        public string Open_time { get; set; }
        public string Close_timre { get; set; }
        public string Store_Address { get; set; }

        public DateTime Begin_time { get; set; }

        public DateTime End_time { get; set; }

        public string Staff_name { get; set; }

        public AppointmentOption Item { get; set; }

        public float Total_Duration { get; set; }

        public double Total_money { get; set; }


        public AppointmentDetail(Appointment app, Store store, string staffname)
        {
            Item = new AppointmentOption();

            Id = app.Id;
            Store_name = store.StoreName;
            CheckInStatus = app.CheckinStatus;
            Store_phone = store.PhoneNumber;
            Store_Address = store.Address;
            Open_time = store.open_time;
            Close_timre = store.close_time;

            Begin_time = app.FromTime;

            End_time = app.ToTime;
            Staff_name = staffname;

            Item.Service = app.ListSer;

            Item.Extra = app.ListEx;

            Item.Product = app.ListPro;


            Total_Duration = app.duration;

            Total_money = app.total;
        }

    }
}
