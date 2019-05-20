using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AppointmentV2Controller : Controller
    {

        private readonly AppointmentV2Respository appointmentV2Respository;
        private IConfiguration config;
        public AppointmentV2Controller(IConfiguration configuration)
        {
            config = configuration;
            appointmentV2Respository = new AppointmentV2Respository(configuration);
        }

        // Add Appointment
        [Route("Add")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Post([FromBody]Appointment item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

                var currentMerchant = HttpContext.User;

                if (item.Staff_id <= 0)
                    item.CheckinStatus = "waiting";
                else
                    item.CheckinStatus = "unconfirm";
                item.CreateDate = DateTime.UtcNow;
                Exception ex = new Exception();
                int ID = 0;
                if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out ID);
                    item.User_id = ID;
                }
                if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out ID);
                    item.StoreId = ID;
                }


                int? appointmentId = appointmentV2Respository.Add(item, ref ex);
                if (appointmentId == 0)

                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = appointmentId, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR APPOINTMENT POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }


        //Update Appointment
        [Route("Update")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult PostUpdate([FromBody]Appointment item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

                var currentMerchant = HttpContext.User;
                int ID = 0;
                if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out ID);
                    if (ID != item.User_id)
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                }
                if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out ID);
                    if (ID != item.StoreId)
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                }

                Exception ex = new Exception();
                if (appointmentV2Respository.Update(item, ref ex))
                {
                    // UpdateCalendar();
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item.Id, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });


            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR APPOINTMENT PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }


        //UpdateStatus Appointment
        [Route("UpdateStatus")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult PostUpdateStatus([FromBody]Appointment item)
        {
            try
            {
                //if (!ModelState.IsValid)
                //   return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData//(ResponeCode.Error, ModelState.ToString()) });

                var currentMerchant = HttpContext.User;
                int ID = 0;
                if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out ID);
                    if (ID != item.User_id)
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                }
                if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out ID);
                    if (ID != item.StoreId)
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                }

                Exception ex = new Exception();
                if (appointmentV2Respository.UpdateStatus(item, ref ex))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item.Id, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }


            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR APPOINTMENT PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }


        //UpdateStatus Appointment
        [Route("DragDrop")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult DragDrop(string FromTime, int appointment_id, int staff_id)
        {
            try
            {


                var currentMerchant = HttpContext.User;


                Exception ex = new Exception();
                if (appointmentV2Respository.UpdateTimeStaff(FromTime, appointment_id, staff_id, ref ex))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = "OK", Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }


            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR DRAG PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }

        //Get FindUserById By ID
        [Route("FindUserByPhone")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<UserCustomer> FindCustomer(string phone)
        {
            UserCustomer u = appointmentV2Respository.FindUserByPhone(phone);


            var currentMerchant = HttpContext.User;

            if (u == null)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            if (u.user_id == 0)
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = "{}", Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = u, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }

        //Get FindUserById By ID
        [Route("FindUserByID")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<UserCustomer> FindUserById(int id)
        {
            UserCustomer u = appointmentV2Respository.FindUserById(id);


            var currentMerchant = HttpContext.User;

            if (u == null)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            if (u.user_id == 0)
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = "{}", Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = u, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }


        //Get Appointment By ID
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Appointment> Get(int id)
        {

            Appointment item = appointmentV2Respository.FindByID2(id);
            var currentMerchant = HttpContext.User;

            /* if (currentMerchant.HasClaim(c => c.Type == "UserId"))
             {
                 Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out ID);
                 if (ID != UserId)
                     return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "402", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "" + ID) });
             }
             if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
             {
                 Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out ID);
                 if (ID != storeID)
                     return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized , "") });
             }*/

            if (item == null)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }



        //Get Appointment By Date
        [Route("FindByDate")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Appointment> FindByDate(string date, int storeid, string status)
        {

            try
            {
                int ID = 0;
                var currentMerchant = HttpContext.User;

                string Message = string.Empty;
                IEnumerable<AppointmentBooking> appointments = appointmentV2Respository.FindByDate(date, storeid, status.ToLower());
                if (appointments == null)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = appointments, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR Get Appointment By Member Array!!");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = ex.ToString(), Message = new NewHttpRespone().setData(ResponeCode.Error, "[]") });

            }
        }


        //Get Appointment By Date
        [Route("GetNotify")]
        [HttpGet]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Notification> FindNotifyByUserId(int userid)
        {

            try
            {
                int ID = 0;
                var currentMerchant = HttpContext.User;

                string Message = string.Empty;
                IEnumerable<Notification> _list = appointmentV2Respository.FindNotifyByUser(userid);
                if (_list == null)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = _list, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR Get Notify");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, "[]") });

            }
        }

        //Get Appointment By Date
        [Route("GetInbox")]
        [HttpGet]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Notification> GetInbox(int userid)
        {

            try
            {
                int ID = 0;
                var currentMerchant = HttpContext.User;

                string Message = string.Empty;
                IEnumerable<Notification> _list = appointmentV2Respository.FindNotifyByUser(userid).Where(t => t.View == 0);
                if (_list == null)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = _list, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR Get Notify");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, "[]") });

            }
        }

        // Add Appointment
        [Route("AddNotify")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult AddNotify([FromBody]Notification item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

                item.View = 0;

                var currentMerchant = HttpContext.User;


                item.CreateDate = DateTime.UtcNow;
                Exception ex = new Exception();

                if (item.NotifyStatus == "paid")
                {
                    appointmentV2Respository.UpdateUserCredit(item.SenderId, item.Money, "paid");
                }
                if (item.NotifyStatus == "claim")
                {
                    appointmentV2Respository.UpdateUserCredit(item.SenderId, item.Money, "calm");
                }


                int? nofifyId = appointmentV2Respository.AddNotify(item, ref ex);
                if (nofifyId == 0)

                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = nofifyId, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR NOTIFICATION POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }

        // Add Appointment
        [Route("UpdateNotify")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult UpdateNotify([FromBody]Notification item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });



                var currentMerchant = HttpContext.User;


                item.CreateDate = DateTime.UtcNow;
                Exception ex = new Exception();


                int? nofifyId = appointmentV2Respository.UpdateNotify(item).Id;
                if (nofifyId == 0)

                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = nofifyId, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR NOTIFICATION POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }

        // Add Appointment
        [Route("UpdateCalendar")]
        [HttpGet]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Notification> UpdateCalendar()
        {
            try
            {


                Notification noti = appointmentV2Respository.FindNotifyById(8);
                if (noti.Id == 8 && noti.Status == 1)
                {
                    noti.Status = 0;
                    UpdateNotify(noti);

                }

                Exception ex = new Exception();


                if (noti != null)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = noti, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                return null;
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR NOTIFICATION POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = 0, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }


        [Route("ViewNotify")]
        [HttpGet]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Notification> ViewNotify(int notifyid)
        {
            try
            {


                Notification noti = appointmentV2Respository.FindNotifyById(notifyid);


                Exception ex = new Exception();


                if (noti != null)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = noti, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                return null;
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR NOTIFICATION POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = notifyid, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }






        // Add Appointment
        [Route("AddNewUser")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult AddCustomer([FromBody]UserCustomer uc)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

                var currentMerchant = HttpContext.User;


                Exception ex = new Exception();
                int ID = 0;



                int? appointmentId = appointmentV2Respository.AddNewUser(uc, ref ex);
                if (appointmentId == 0)

                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = appointmentId, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR APPOINTMENT POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }
    }
}
