using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class AppointmentRespository 
    {
     
        private IConfiguration config;
       
        public AppointmentRespository(IConfiguration configuration)
        {
            config = configuration;
          
        }

        public void LogCheck(LogModel log)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                    Base.Connection.Insert<LogModel>(log);
                        connect.Commit();
                    }
                    catch (Exception ex)
                    {

                        connect.Rollback();

                    }
                }
            
        }
        public int? Add(Appointment appointment, ref Exception exp)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                        DateTime dt = DateTime.Now;
                        if (appointment.FromTime != null)
                            dt = new DateTime(appointment.FromTime.Year, appointment.FromTime.Month, appointment.FromTime.Day);

                        if (appointment.ToTime == null)
                            appointment.ToTime = dt.AddMinutes(30);

                        //insert appointment and get appoinement id
                        appointment.PaidStatus = false;
                        appointment.Status = 1;
                        appointment.CreateDate = dt;




                        int? appointmentID = Base.Connection.Insert<Appointment>(appointment);


                        // insert BookingService
                        if (appointment.BookingServices != null)
                        {
                            if (appointment.BookingServices.Length > 0)
                            {
                                List<Service> L_service = Base.Connection.GetList<Service>("Where id IN " + ConvertArray(appointment.BookingServices)).ToList();
                                if (L_service.Count > 0)
                                {
                                    List<BookingService> Serviceresuilt = new List<BookingService>();
                                    foreach (Service ser in L_service)
                                    {

                                        BookingService BS = new BookingService();
                                        BS.status = 1;
                                        BS.appointment_id = (int)appointmentID;
                                        BS.service_id = ser.Id;
                                        BS.staff_id = (int)appointment.Staff_id;
                                        Serviceresuilt.Add(BS);
                                    }
                                    var _json = JsonConvert.SerializeObject(Serviceresuilt);
                                    IEnumerable<BookingService> _resuilt = Base.Connection.Query<BookingService>("select \"InsertIntoBookingService\"('" + _json + "')");


                                }
                            }
                        }
                        // insert BookingProduct
                        if (appointment.BookingProducts != null)
                        {
                            if (appointment.BookingProducts.Length > 0)
                            {
                                List<BookingProduct> Productresuilt = new List<BookingProduct>();
                                List<Product> L_Product = Base.Connection.GetList<Product>("Where id IN " + ConvertArray(appointment.BookingProducts)).ToList();
                                foreach (Product pro in L_Product)
                                {
                                    BookingProduct BP = new BookingProduct();
                                    BP.status = 1;
                                    BP.appointment_id = (int)appointmentID;
                                    BP.product_id = pro.Id;
                                    BP.quantity = 1;
                                    Productresuilt.Add(BP);

                                }
                                var json = JsonConvert.SerializeObject(Productresuilt);

                                IEnumerable<BookingProduct> resuilt = Base.Connection.Query<BookingProduct>("select \"InsertIntoBookingProduct\"('" + json + "')");

                            }
                        }
                        // insert Extra
                        if (appointment.BookingExtras != null)
                        {
                            if (appointment.BookingExtras.Length > 0)
                            {
                                List<BookingExtra> Extraresuilt = new List<BookingExtra>();

                                List<Extra> L_Extra = Base.Connection.GetList<Extra>("Where id IN " + ConvertArray(appointment.BookingExtras)).ToList();

                                foreach (Extra ex in L_Extra)
                                {
                                    BookingExtra BE = new BookingExtra();
                                    BE.status = 1;
                                    BE.appointment_id = (int)appointmentID;
                                    BE.extra_id = ex.Id;

                                    Extraresuilt.Add(BE);

                                }
                                var json = JsonConvert.SerializeObject(Extraresuilt);

                                IEnumerable<BookingExtra> resuilt = Base.Connection.Query<BookingExtra>("select \"InsertIntoBookingExtra\"('" + json + "')");

                            }
                        }

                        FlagReposity FR = new FlagReposity(config);
                        FR.StartFlag(Convert.ToInt32(appointment.StoreId));

                        Notification noti = Base.Connection.Get<Notification>(8);
                        noti.Status = 1;

                    Base.Connection.Update<Notification>(noti);

                        connect.Commit();
                        return (int)appointmentID;
                    }
                    catch (Exception ex)
                    {
                        exp = ex;
                        connect.Rollback();
                        return 0;
                    }
                }
            
        }
        private string ConvertArray(int[] Array)
        {
            string array = "(";
            for (int i = 0; i < Array.Length; i++)
            {
                array += Array[i].ToString();
                if (i < Array.Length - 1)
                    array += ",";
            }
            array = array + ")";

            return array;

        }

        public AppointmentBooking FindByID(int id, ref int storeid, ref int userid)
        {

            Base.Connection.Open();

                Appointment appointment = Base.Connection.Get<Appointment>(id);
                Staff st = Base.Connection.Get<Staff>(appointment.Staff_id);
                appointment.staff_name = st.display_name;

                AppointmentBooking resuilt;
                if (appointment != null)
                {
                    storeid = (int)appointment.StoreId;
                    userid = (int)appointment.User_id;
                    List<BookingService> LBS = Base.Connection.GetList<BookingService>(new { appointment_id = appointment.Id }).ToList();
                    if (LBS.Count > 0)
                    {
                        int[] _Array = new int[LBS.Count];
                        for (int i = 0; i < LBS.Count; i++)
                        {
                            _Array[i] = LBS[i].service_id;
                        }

                        appointment.ListSer = Base.Connection.GetList<Service>("Where id IN " + ConvertArray(_Array)).ToList();
                    }

                    List<BookingProduct> LBP = Base.Connection.GetList<BookingProduct>(new { appointment_id = appointment.Id }).ToList();
                    if (LBP.Count > 0)
                    {
                        int[] _Array = new int[LBP.Count];
                        for (int i = 0; i < LBP.Count; i++)
                        {
                            _Array[i] = LBP[i].product_id;
                        }

                        appointment.ListPro = Base.Connection.GetList<Product>("Where id IN " + ConvertArray(_Array)).ToList();

                    }
                    List<BookingExtra> LBE = Base.Connection.GetList<BookingExtra>(new { appointment_id = appointment.Id }).ToList();
                    if (LBE.Count > 0)
                    {
                        int[] _Array = new int[LBE.Count];
                        for (int i = 0; i < LBE.Count; i++)
                        {
                            _Array[i] = LBE[i].extra_id;
                        }

                        appointment.ListEx = Base.Connection.GetList<Extra>("Where id IN " + ConvertArray(_Array)).ToList();

                    }

                    User temp = Base.Connection.Get<User>(appointment.User_id);

                    resuilt = new AppointmentBooking(appointment, temp);

                    return resuilt;
                }
                return null;

            
        }

        public IEnumerable<AppointmentBooking> FindByCheck_InStatus(int storeid)
        {



            Base.Connection.Open();

                List<Appointment> appointments = Base.Connection.GetList<Appointment>(new { checkin_status = "waiting", status = 1, store_id = storeid }).ToList();

                List<User> _User = Base.Connection.GetList<User>(new { status = 1 }).ToList();



                List<AppointmentBooking> ABL = new List<AppointmentBooking>();

                if (appointments.Count > 0)
                {
                    foreach (Appointment appoint in appointments)
                    {
                        List<BookingService> LBS = Base.Connection.GetList<BookingService>(new { appointment_id = appoint.Id }).ToList();

                        if (LBS != null && LBS.Count > 0)
                        {
                            int[] _Array = new int[LBS.Count];
                            for (int i = 0; i < LBS.Count; i++)
                            {
                                _Array[i] = LBS[i].service_id;
                            }
                            appoint.ListSer = Base.Connection.GetList<Service>("Where id IN " + ConvertArray(_Array)).ToList();

                        }
                        List<BookingProduct> LBP = Base.Connection.GetList<BookingProduct>(new { appointment_id = appoint.Id }).ToList();
                        if (LBP != null && LBP.Count > 0)
                        {

                            int[] _Array = new int[LBP.Count];
                            for (int i = 0; i < LBP.Count; i++)
                            {
                                _Array[i] = LBP[i].product_id;
                            }

                            appoint.ListPro = Base.Connection.GetList<Product>("Where id IN " + ConvertArray(_Array)).ToList();

                        }
                        List<BookingExtra> LBE = Base.Connection.GetList<BookingExtra>(new { appointment_id = appoint.Id }).ToList();
                        if (LBE != null && LBE.Count > 0)
                        {
                            int[] _Array = new int[LBE.Count];
                            for (int i = 0; i < LBE.Count; i++)
                            {
                                _Array[i] = LBE[i].extra_id;
                            }

                            appoint.ListEx = Base.Connection.GetList<Extra>("Where id IN " + ConvertArray(_Array)).ToList();

                        }



                        User user = _User.Find(c => c.id == appoint.User_id);

                        if (user != null)
                            ABL.Add(new AppointmentBooking(appoint, user));

                    }
                }
                return ABL;
            
        }

        public IEnumerable<AppointmentBooking> FindByMemberIdAnDate(MemberandDate values, int id)
        {

            Base.Connection.Open();

                List<Staff> Liststaff = Base.Connection.GetList<Staff>("Where id IN " + ConvertArray(values.memberId) + "and store_id = " + id.ToString()).ToList();
                if (Liststaff.Count != values.memberId.ToList().Count)
                    return null;

                IEnumerable<Appointment> appointments = Base.Connection.GetList<Appointment>("Where status=1 and checkin_status<>'cancel' and checkin_status<>'waiting' and staff_id IN " + ConvertArray(values.memberId) + " and create_date = @day and store_id =" + id.ToString() + " order by Id desc fetch first 10 rows only", new { day = DateTime.Parse(values.date) });

                List<User> _User = Base.Connection.GetList<User>(new { status = 1 }).ToList();
                List<AppointmentBooking> ABL = new List<AppointmentBooking>();

                List<BookingService> LBS = Base.Connection.GetList<BookingService>().ToList();
                List<Service> allservices = Base.Connection.GetList<Service>().ToList();
                List<Staff> allStaff = Base.Connection.GetList<Staff>().ToList();





                foreach (Appointment appoint in appointments)
                {
                    appoint.ListSer = new List<Service>();
                    User user = _User.Find(c => c.id == appoint.User_id);
                    List<BookingService> currentBookingServiceList = LBS.Where(t => t.appointment_id == appoint.Id && t.status == 1).ToList();
                    foreach (BookingService bsItem in currentBookingServiceList)
                    {
                        List<Service> sList = new List<Service>();
                        for (int i = 0; i < allservices.Count; i++)
                        {
                            if (allservices[i].Id == bsItem.service_id)
                            {

                                allservices[i].Duration = bsItem.duration;
                                appoint.ListSer.Add(allservices[i]);
                            }
                        }
                    }

                    if (user != null)
                        ABL.Add(new AppointmentBooking(appoint, user));
                }




            /*  if (appointments.ToList().Count > 0)
              {
                  foreach (Appointment appoint in appointments)
                  {
                      List<BookingService> LBS = dbConnection.GetList<BookingService>(new { appointment_id = appoint.Id }).ToList();

                      if (LBS != null && LBS.Count > 0)
                      {
                          int[] _Array = new int[LBS.Count];
                          for (int i = 0; i < LBS.Count; i++)
                          {
                              _Array[i] = LBS[i].service_id;
                          }
                          appoint.ListSer = dbConnection.GetList<Service>(" Where id IN " + ConvertArray(_Array)).ToList();

                      }
                      List<BookingProduct> LBP = dbConnection.GetList<BookingProduct>(new { appointment_id = appoint.Id }).ToList();
                      if (LBP != null && LBP.Count > 0)
                      {

                          int[] _Array = new int[LBP.Count];
                          for (int i = 0; i < LBP.Count; i++)
                          {
                              _Array[i] = LBP[i].product_id;
                          }

                          appoint.ListPro = dbConnection.GetList<Product>("Where id IN " + ConvertArray(_Array)).ToList();

                      }
                      List<BookingExtra> LBE = dbConnection.GetList<BookingExtra>(new { appointment_id = appoint.Id }).ToList();
                      if (LBE != null && LBE.Count > 0)
                      {
                          int[] _Array = new int[LBE.Count];
                          for (int i = 0; i < LBE.Count; i++)
                          {
                              _Array[i] = LBE[i].extra_id;
                          }

                          appoint.ListEx = dbConnection.GetList<Extra>("Where id IN " + ConvertArray(_Array)).ToList();

                      }

                      User user = _User.Find(c => c.id == appoint.User_id);
                      if(appoint.ListSer!=null)
                      foreach (Service ser in appoint.ListSer)
                      {
                          appoint.duration += (float)ser.Duration;

                      }
                      if(appoint.ListEx!=null)
                      foreach (Extra ex in appoint.ListEx)
                      {

                          appoint.duration += ex.duration;
                      }

                      if (user != null)
                          ABL.Add(new AppointmentBooking(appoint, user));

                  }
              }*/

            Base.Connection.Close();
                return ABL;
            
        }


        public void Remove(int id)
        {

            Base.Connection.Open();
                Appointment appointment = Base.Connection.Get<Appointment>(id);
                if (appointment != null)
                {
                    appointment.Status = -1;


                //delete BookingExtra

                Base.Connection.Query<BookingExtra>("Update \"BookingExtra\" set status=-1 Where appointment_id =@appointment_id", new { appointment_id = appointment.Id });

                //delete BookingService
                Base.Connection.Query<BookingService>("Update \"BookingService\" set status=-1 Where appointment_id =@appointmentid", new { appointmentid = appointment.Id });

                // delete BookingProduct
                Base.Connection.Query<BookingProduct>("Update \"BookingProduct\" set status=-1 Where appointment_id =@appointmentid", new { appointmentid = appointment.Id });

                //delete this Appointment
                Base.Connection.Update<Appointment>(appointment);
                }
            
        }

        public bool Update(Appointment appointment, ref Exception exp)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                        DateTime dt = new DateTime(appointment.FromTime.Year, appointment.FromTime.Month, appointment.FromTime.Day);
                        Remove(appointment.Id);
                        appointment.CreateDate = dt;

                    //update appointment
                    Base.Connection.Update<Appointment>(appointment);

                        // insert BookingService
                        if (appointment.BookingServices != null)
                        {
                            if (appointment.BookingServices.Length > 0)
                            {
                                List<Service> L_service = Base.Connection.GetList<Service>("Where id IN " + ConvertArray(appointment.BookingServices)).ToList();
                                if (L_service.Count > 0)
                                {
                                    List<BookingService> Serviceresuilt = new List<BookingService>();
                                    foreach (Service ser in L_service)
                                    {

                                        BookingService BS = new BookingService();
                                        BS.status = 1;
                                        BS.appointment_id = appointment.Id;
                                        BS.service_id = ser.Id;
                                        BS.staff_id = (int)appointment.Staff_id;
                                        Serviceresuilt.Add(BS);
                                    }
                                    var _json = JsonConvert.SerializeObject(Serviceresuilt);
                                    IEnumerable<BookingService> _resuilt = Base.Connection.Query<BookingService>("select \"InsertIntoBookingService\"('" + _json + "')");
                                }
                            }
                        }
                        // insert BookingProduct
                        if (appointment.BookingProducts != null)
                        {
                            if (appointment.BookingProducts.Length > 0)
                            {
                                List<BookingProduct> Productresuilt = new List<BookingProduct>();
                                List<Product> L_Product = Base.Connection.GetList<Product>("Where id IN " + ConvertArray(appointment.BookingProducts)).ToList();
                                foreach (Product pro in L_Product)
                                {
                                    BookingProduct BP = new BookingProduct();
                                    BP.status = 1;
                                    BP.appointment_id = appointment.Id;
                                    BP.product_id = pro.Id;
                                    BP.quantity = 1;
                                    Productresuilt.Add(BP);

                                }
                                var json = JsonConvert.SerializeObject(Productresuilt);

                                IEnumerable<BookingProduct> resuilt = Base.Connection.Query<BookingProduct>("select \"InsertIntoBookingProduct\"('" + json + "')");

                            }
                        }
                        // insert Extra
                        if (appointment.BookingExtras != null)
                        {
                            if (appointment.BookingExtras.Length > 0)
                            {
                                List<BookingExtra> Extraresuilt = new List<BookingExtra>();

                                List<Extra> L_Extra = Base.Connection.GetList<Extra>("Where id IN " + ConvertArray(appointment.BookingExtras)).ToList();

                                foreach (Extra ex in L_Extra)
                                {
                                    BookingExtra BE = new BookingExtra();
                                    BE.status = 1;
                                    BE.appointment_id = appointment.Id;
                                    BE.extra_id = ex.Id;

                                    Extraresuilt.Add(BE);

                                }
                                var json = JsonConvert.SerializeObject(Extraresuilt);

                                IEnumerable<BookingProduct> resuilt = Base.Connection.Query<BookingProduct>("select\" InsertIntoBookingProduct\"('" + json + "')");

                            }
                        }
                        connect.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        exp = ex;
                        connect.Rollback();
                        return false;
                    }
                }
            
        }


        public bool AsignAppointment(AssignAppointment Assign, ref Exception _ex, ref string Messageage)

        {

            Base.Connection.Open();
                try
                {

                    Appointment app = FindByID(Assign.appointmentid);
                    if (app.CheckinStatus == "waiting")
                    {
                        Messageage = "Assign appointment successfully!";
                        app.CheckinStatus = "confirm";
                    }
                    if (app.CheckinStatus == "confirm" || app.CheckinStatus == "unconfirm")
                        Messageage = "Move appointment successfully!";
                    app.Staff_id = Assign.memberid;
                    app.FromTime = Assign.Star;
                    app.ToTime = Assign.End;
                    int? appointment = Base.Connection.Update<Appointment>(app);

                    List<BookingService> LBS = Base.Connection.GetList<BookingService>(new { appointment_id = appointment }).ToList();
                    LBS.ForEach(ele =>
                    {
                        ele.staff_id = Assign.memberid;
                        Base.Connection.Update<BookingService>(ele);
                    });


                    return true;
                }

                catch (Exception ex)
                {
                    _ex = ex;
                    return false;
                }
            
        }

        public IEnumerable<Appointment> FindAll()
        {

            Base.Connection.Open();
                List<Appointment> appointments = Base.Connection.GetList<Appointment>(new { status = 1 }).ToList();
                appointments.ForEach(appointment =>
                {
                    List<BookingService> LBS = Base.Connection.GetList<BookingService>(new { appointment_id = appointment.Id }).ToList();
                    if (LBS.Count > 0)
                    {
                        int[] _Array = new int[LBS.Count];
                        for (int i = 0; i < LBS.Count; i++)
                        {
                            _Array[i] = LBS[i].service_id;
                        }

                        appointment.ListSer = Base.Connection.GetList<Service>("Where id IN " + ConvertArray(_Array)).ToList();
                    }

                    List<BookingProduct> LBP = Base.Connection.GetList<BookingProduct>(new { appointment_id = appointment.Id }).ToList();
                    if (LBP.Count > 0)
                    {
                        int[] _Array = new int[LBP.Count];
                        for (int i = 0; i < LBP.Count; i++)
                        {
                            _Array[i] = LBP[i].product_id;
                        }

                        appointment.ListPro = Base.Connection.GetList<Product>("Where id IN " + ConvertArray(_Array)).ToList();

                    }
                    List<BookingExtra> LBE = Base.Connection.GetList<BookingExtra>(new { appointment_id = appointment.Id }).ToList();
                    if (LBE.Count > 0)
                    {
                        int[] _Array = new int[LBE.Count];
                        for (int i = 0; i < LBE.Count; i++)
                        {
                            _Array[i] = LBE[i].extra_id;
                        }

                        appointment.ListEx = Base.Connection.GetList<Extra>("Where id IN " + ConvertArray(_Array)).ToList();

                    }
                });
                return appointments;
            
        }


        public bool PutBackAppointment(int appointment, ref Exception _ex)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                        Appointment app = FindByID(appointment);
                        app.CheckinStatus = "waiting";
                        app.Staff_id = -1;
                    Base.Connection.Update<Appointment>(app);
                        List<BookingService> LBS = Base.Connection.GetList<BookingService>(new { appointment_id = app.Id, status = 1 }).ToList();
                        LBS.ForEach(ele =>
                        {
                            ele.staff_id = -1;

                            Base.Connection.Update<BookingService>(ele);
                        });
                        connect.Commit();
                        return true;

                    }
                    catch (Exception ex)
                    {
                        _ex = ex;
                        connect.Rollback();
                        return false;
                    }
                }
            
        }

        public bool PaidForAppointment(Paid appointment, ref Exception _ex)

        {


            Base.Connection.Open();
                try
                {
                    Appointment app = FindByID(appointment.AppointmentId);
                    app.PaidStatus = true;
                Base.Connection.Update<Appointment>(app);
                    return true;

                }
                catch (Exception ex)
                {
                    _ex = ex;
                    return false;
                }
            
        }
        public int? Add(Appointment item)
        {
            throw new NotImplementedException();
        }

        public void Update(Appointment item)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<AppointmentDetail> FindAllByUserID(int id)
        {

            Base.Connection.Open();

                List<Appointment> appointments = Base.Connection.GetList<Appointment>("where status =1 and checkin_status<>'cancel' and user_id=" + id + " order by Id desc fetch first 20 rows only").ToList();
                List<AppointmentDetail> appdetaillist = new List<AppointmentDetail>();
                /* if ( appointments!=null )
                 {


                     foreach (Appointment appoint in appointments)
                     {
                         List<BookingService> LBS = dbConnection.GetList<BookingService>(new { appointment_id = appoint.Id }).ToList();

                         if (LBS != null && LBS.Count > 0)
                         {
                             int[] _Array = new int[LBS.Count];
                             for (int i = 0; i < LBS.Count; i++)
                             {
                                 _Array[i] = LBS[i].service_id;
                             }
                             appoint.ListSer = dbConnection.GetList<Service>(" Where id IN " + ConvertArray(_Array)).ToList();

                         }
                         List<BookingProduct> LBP = dbConnection.GetList<BookingProduct>(new { appointment_id = appoint.Id }).ToList();
                         if (LBP != null && LBP.Count > 0)
                         {

                             int[] _Array = new int[LBP.Count];
                             for (int i = 0; i < LBP.Count; i++)
                             {
                                 _Array[i] = LBP[i].product_id;
                             }

                             appoint.ListPro = dbConnection.GetList<Product>("Where id IN " + ConvertArray(_Array)).ToList();

                         }
                         List<BookingExtra> LBE = dbConnection.GetList<BookingExtra>(new { appointment_id = appoint.Id }).ToList();
                         if (LBE != null && LBE.Count > 0)
                         {
                             int[] _Array = new int[LBE.Count];
                             for (int i = 0; i < LBE.Count; i++)
                             {
                                 _Array[i] = LBE[i].extra_id;
                             }

                             appoint.ListEx = dbConnection.GetList<Extra>("Where id IN " + ConvertArray(_Array)).ToList();

                         }

                         Store store = new StoreRepository(config).FindByID((int)appoint.StoreId);

                         Staff staff = null;
                         if(appoint.Staff_id!=null)
                             staff = new StaffRespository(config).FindByID((int)appoint.Staff_id);

                         if (store != null&& staff!=null)
                             appdetaillist.Add(new AppointmentDetail(appoint, store, staff.first_name));
                         else
                             appdetaillist.Add(new AppointmentDetail(appoint, store,""));


                     }
                     dbConnection.Close();
                     return appdetaillist;
                 }*/

                if (appointments != null)

                {
                    Store store = new StoreRepository(config).FindByID((int)appointments[0].StoreId);


                    List<BookingService> LBS = Base.Connection.GetList<BookingService>().ToList();
                    List<Service> allservices = Base.Connection.GetList<Service>().ToList();
                    List<Staff> allStaff = Base.Connection.GetList<Staff>().ToList();





                    foreach (Appointment appoint in appointments)
                    {
                        appoint.ListSer = new List<Service>();
                        List<BookingService> currentBookingServiceList = LBS.Where(t => t.appointment_id == appoint.Id && t.status == 1).ToList();
                        foreach (BookingService bsItem in currentBookingServiceList)
                        {
                            List<Service> sList = new List<Service>();
                            for (int i = 0; i < allservices.Count; i++)
                            {
                                if (allservices[i].Id == bsItem.service_id)
                                {
                                    appoint.ListSer.Add(allservices[i]);
                                }
                            }
                        }

                        Staff s = new Staff();
                        try { s = allStaff.Where(t => t.Id == appoint.Staff_id).First(); s.display_name = s.first_name + " " + s.last_name; }
                        catch { s.display_name = "AnyStaff"; }

                        appoint.staff_name = s.display_name;
                        appdetaillist.Add(new AppointmentDetail(appoint, store, s.display_name));
                    }
                Base.Connection.Close();
                    return appdetaillist;
                }

                Base.Connection.Close();
                return null;
            
        }

        public Appointment FindByID(int id)
        {
            throw new NotImplementedException();
        }
    }
}
