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
    public class AppointmentV2Respository 
    {

        private IConfiguration config;
    
        public AppointmentV2Respository(IConfiguration configuration)
        {
            config = configuration;
          
        }


        public int? Add(Appointment appointment, ref Exception exp)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                        //insert appointment and get appoinement id
                        appointment.PaidStatus = false;
                        appointment.Status = 1;
                        appointment.CreateDate = DateTime.Today;




                        int? appointmentID = Base.Connection.Insert<Appointment>(appointment);


                        // insert BookingService
                        if (appointment.BookingServices2 != null)
                        {
                            if (appointment.BookingServices2.Length > 0)
                            {
                                // List<Service> L_service = dbConnection.GetList<Service>("Where id IN " + ConvertArray//(appointment.BookingServices)).ToList();
                                List<BookingService> bookingServicesList = GetBookingServices(appointment.BookingServices2);
                                if (bookingServicesList != null && bookingServicesList.Count > 0)
                                {
                                    List<BookingService> Serviceresuilt = new List<BookingService>();
                                    foreach (BookingService ser in bookingServicesList)
                                    {

                                        BookingService BS = new BookingService();
                                        BS.status = 1;
                                        BS.appointment_id = (int)appointmentID;
                                        BS.service_id = ser.service_id;
                                        BS.staff_id = ser.staff_id;
                                        BS.duration = ser.duration;
                                        // BS.duration = 
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

                                List<Extra> L_Extra = Base.Connection.GetList<Extra>(" Where id IN " + ConvertArray(appointment.BookingExtras)).ToList();

                                foreach (Extra ex in L_Extra)
                                {
                                    BookingExtra BE = new BookingExtra();
                                    BE.status = 1;
                                    BE.appointment_id = (int)appointmentID;
                                    BE.extra_id = ex.Id;

                                    Extraresuilt.Add(BE);

                                }
                                var json = JsonConvert.SerializeObject(Extraresuilt);

                                IEnumerable<BookingProduct> resuilt = Base.Connection.Query<BookingProduct>("select\" InsertIntoBookingProduct\"('" + json + "')");

                            }
                        }

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



        public bool Update(Appointment appointment, ref Exception exp)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                        Remove(appointment.Id);

                        Appointment update_appointment = appointment;
                        update_appointment.CheckinStatus = update_appointment.CheckinStatus.ToLower();
                        update_appointment.User_id = appointment.User_id;
                    //update_appointment.


                    //update appointment
                    Base.Connection.Update<Appointment>(update_appointment);

                        // insert BookingService
                        if (appointment.BookingServices2 != null)
                        {
                            if (appointment.BookingServices2.Length > 0)
                            {
                                List<BookingService> bookingServicesList = GetBookingServices(appointment.BookingServices2);
                                if (bookingServicesList != null && bookingServicesList.Count > 0)
                                {
                                    List<BookingService> Serviceresuilt = new List<BookingService>();
                                    foreach (BookingService ser in bookingServicesList)
                                    {

                                        BookingService BS = new BookingService();
                                        BS.status = 1;
                                        BS.appointment_id = appointment.Id;
                                        BS.service_id = ser.service_id;
                                        BS.staff_id = ser.staff_id;
                                        BS.duration = ser.duration;
                                        // BS.duration = 
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

                        Notification noti = Base.Connection.Get<Notification>(8);
                        noti.Status = 1;

                        Base.Connection.Update<Notification>(noti);

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


        public bool UpdateStatus(Appointment appointment, ref Exception exp)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                        Appointment ud = Base.Connection.Get<Appointment>(appointment.Id);
                        ud.CheckinStatus = appointment.CheckinStatus.ToLower();


                    //update appointment
                    Base.Connection.Update<Appointment>(ud);


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


        public bool UpdateTimeStaff(string FromTime, int appointment_id, int staff_id, ref Exception exp)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                        Appointment ud = Base.Connection.Get<Appointment>(appointment_id);
                        if (ud.Staff_id == 0)
                            ud.CheckinStatus = "confirm";
                        ud.Staff_id = staff_id;
                        ud.FromTime = DateTime.Parse(FromTime);
                        //ud.ToTime = ud.FromTime.AddHours(1);
                        ud.CreateDate = DateTime.Parse(FromTime);

                    //update appointment
                    Base.Connection.Update<Appointment>(ud);


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

        public int? Add(Appointment item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Appointment> FindAll()
        {
            throw new NotImplementedException();
        }

        public Appointment FindByID(int id)
        {
            throw new NotImplementedException();
        }

        public UserCustomer FindUserByPhone(string _phone)
        {

            Base.Connection.Open();
                User u = Base.Connection.GetList<User>(new { phone = _phone }).FirstOrDefault();
                UserCustomer uc = new UserCustomer();
                if (u != null)
                {
                    uc.user_id = u.id;
                    uc.full_name = u.full_name;
                    uc.phone = u.phone;
                    uc.credit = u.credit;
                    uc.star = u.star;
                    uc.dob = Convert.ToDateTime(u.birthday);
                    uc.email = u.email;
                    uc.first_name = u.first_name;
                    uc.last_name = u.last_name;
                }
                return uc;
            
        }

        public List<Notification> FindNotifyByUser(int _id)
        {

            Base.Connection.Open();
                List<Notification> _list = Base.Connection.GetList<Notification>().Where(t => t.ReceiverId == _id || t.SenderId == _id).ToList();

            Base.Connection.Close();
                return _list;
            
        }

        public Notification FindNotifyById(int _id)
        {

            Base.Connection.Open();
                Notification _noti = Base.Connection.Get<Notification>(_id);


                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {
                        _noti.View = 1;

                    Base.Connection.Update<Notification>(_noti);


                        connect.Commit();

                    }
                    catch (Exception ex)
                    {

                        connect.Rollback();

                    }
                }
            Base.Connection.Close();
                return _noti;
            
        }

        public Notification UpdateNotify(Notification _noti)
        {

            Base.Connection.Open();



                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {

                    Base.Connection.Update<Notification>(_noti);


                        connect.Commit();

                    }
                    catch (Exception ex)
                    {

                        connect.Rollback();

                    }
                }
            Base.Connection.Close();
                return _noti;
            
        }

        public void UpdateUserCredit(int _uid, float money, string type)
        {

            Base.Connection.Open();



                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {

                        User u = Base.Connection.Get<User>(_uid);
                        if (type == "paid")
                            u.credit = u.credit - money;
                        if (type == "claim")
                            u.credit = u.credit + money;

                    Base.Connection.Update<User>(u);


                        connect.Commit();

                    }
                    catch (Exception ex)
                    {

                        connect.Rollback();

                    }
                }
            Base.Connection.Close();

            
        }

        public int? AddNotify(Notification notify, ref Exception exp)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {




                        int? notifyId = Base.Connection.Insert<Notification>(notify);




                        connect.Commit();
                        return (int)notifyId;
                    }
                    catch (Exception ex)
                    {
                        exp = ex;
                        connect.Rollback();
                        return 0;
                    }
               }
            
        }



        public UserCustomer FindUserById(int _id)
        {

            Base.Connection.Open();
                User u = Base.Connection.GetList<User>(new { id = _id }).FirstOrDefault();
                UserCustomer uc = new UserCustomer();
                if (u != null)
                {
                    uc.user_id = u.id;
                    uc.full_name = u.full_name;
                    uc.phone = u.phone;
                    uc.credit = u.credit;
                    uc.star = u.star;
                    uc.dob = Convert.ToDateTime(u.birthday);
                    uc.email = u.email;
                    if (u.first_name == null)
                        u.first_name = "Test King";
                    else
                        uc.first_name = u.first_name;
                    uc.last_name = u.last_name;
                }
                return uc;
            
        }

        public Appointment FindByID2(int id)
        {

            Base.Connection.Open();
                Appointment appointment = Base.Connection.Get<Appointment>(id);

                List<Service> allservices = Base.Connection.GetList<Service>().ToList();




                List<BookingService> servicelist = Base.Connection.GetList<BookingService>(new { status = 1, appointment_id = id }).ToList();

                User u = Base.Connection.Get<User>(appointment.User_id);
                appointment.CustomerId = u.id;
                appointment.PhoneNo = u.phone;
                // appointment.BookingServiceList = servicelist;
                string[] temp = new string[servicelist.Count];
                appointment.ListSer = new List<Service>();
                for (int i = 0; i < servicelist.Count; i++)
                {

                    List<Service> sList = new List<Service>();
                    for (int j = 0; j < allservices.Count; j++)
                    {
                        if (allservices[j].Id == servicelist[i].service_id)
                        {
                            allservices[j].adjust_duration = Convert.ToInt32(servicelist[i].duration);
                            allservices[j].adjust_staffid = Convert.ToInt32(servicelist[i].staff_id);
                            //appointment.ListSer.Add(allservices[j]);
                            if (servicelist[i].duration == 0)
                            {
                                temp[i] = servicelist[i].service_id + "@" + allservices[j].Duration + "@" + servicelist[i].staff_id; //+ "@" + allservices[j].Price; 
                            }
                            else
                            {
                                temp[i] = servicelist[i].service_id + "@" + servicelist[i].duration + "@" + servicelist[i].staff_id;// + "@" + allservices[j].Price;
                            }
                        }
                    }

                }
                appointment.BookingServices2 = temp;
                if (appointment != null)
                {
                    return appointment;
                }
            

            return null;
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

        public void Update(Appointment item)
        {
            throw new NotImplementedException();
        }

        // convert array to service id list
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

        private List<BookingService> GetBookingServices(string[] Array)
        {
            List<BookingService> list = new List<BookingService>();
            string[] tmp;
            for (int i = 0; i < Array.Length; i++)
            {
                tmp = Array[i].ToString().Split("@");
                BookingService bs = new BookingService();
                bs.service_id = Convert.ToInt32(tmp[0]);
                bs.duration = Convert.ToInt32(tmp[1]);
                bs.staff_id = Convert.ToInt32(tmp[2]);
                list.Add(bs);
            }


            return list;

        }


        public IEnumerable<AppointmentBooking> FindByDate(string date, int storeid, string status)
        {

            Base.Connection.Open();



                IEnumerable<Appointment> appointments;
                // all
                if (status == "all" || status == null)
                    appointments = Base.Connection.GetList<Appointment>("Where store_id=" + storeid + "   AND status=1 AND create_date = '" + date + "' and (checkin_status<>'waiting' or checkin_status <> 'waiting' )");
                else
                {
                    if (status == "waiting")
                    {
                        appointments = Base.Connection.GetList<Appointment>("Where store_id=" + storeid + "   AND status=1  and checkin_status='waiting'");
                    }
                    else
                        appointments = Base.Connection.GetList<Appointment>("Where store_id=" + storeid + "   AND status=1 AND create_date = '" + date + "' and checkin_status='" + status + "'");
                }
                //  IEnumerable<Appointment> appointments = dbConnection.GetList<Appointment>("Where staff_id IN " + ConvertArray(values.memberId) + " and create_date = @day and store_id =" + id.ToString(), new { day = DateTime.Parse(values.date) });

                List<Service> allService = Base.Connection.GetList<Service>().ToList();
                List<AppointmentBooking> ABL = new List<AppointmentBooking>();
                string aplist = "0";
                for (int i = 0; i < appointments.ToList().Count; i++)
                {
                    aplist += "," + appointments.ToList()[i].Id;
                }
                List<BookingService> allBS = Base.Connection.GetList<BookingService>("where status = 1 ANd appointment_id in(" + aplist + ")").ToList();


                if (appointments.ToList().Count > 0)
                {
                    foreach (Appointment appoint in appointments)
                    {
                        List<BookingService> LBS = allBS.Where(t => t.appointment_id == appoint.Id).ToList();


                        if (LBS != null && LBS.Count > 0)
                        {
                            appoint.ListSer = new List<Service>();
                            int[] _Array = new int[LBS.Count];
                            for (int i = 0; i < LBS.Count; i++)
                            {
                                // _Array[i] = LBS[i].service_id;
                                Service s = allService.First(t => t.Id == LBS[i].service_id);// .Get<Service>(LBS[i].service_id);
                                s.adjust_duration = Convert.ToInt32(LBS[i].duration);
                                s.adjust_staffid = LBS[i].staff_id;
                                s.Duration = Convert.ToInt32(LBS[i].duration);


                                appoint.ListSer.Add(s);
                            }
                            // appoint.ListSer = dbConnection.GetList<Service>(" Where id IN " + ConvertArray(_Array)).ToList();

                        }
                        /*  List<BookingProduct> LBP = dbConnection.GetList<BookingProduct>(new { appointment_id = appoint.Id }).ToList();
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

                          }*/



                        if (appoint.User_id != null)
                        {
                            ABL.Add(new AppointmentBooking(appoint, Base.Connection.Get<User>(appoint.User_id)));
                        }

                    }
                }
                return ABL;
            
        }


        public int? AddNewUser(UserCustomer uc, ref Exception exp)
        {
          
                Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {
                    try
                    {


                        User u = new User();
                        u.first_name = uc.first_name;
                        u.last_name = uc.last_name;
                        u.status = 1;
                        u.phone = uc.phone;
                        u.registration_status = 1;
                        u.full_name = uc.first_name + " " + uc.last_name;
                        u.create_date = DateTime.Today;
                        u.city_id = 1;
                        u.state_id = 1;

                        int? appointmentID = Base.Connection.Insert<User>(u);



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
    }
}
