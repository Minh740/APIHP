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
    public class AppointmentsController : Controller
    {
        private readonly AppointmentRespository appointmentRespository;
        private IConfiguration config;
        public AppointmentsController(IConfiguration configuration)
        {
            config = configuration;
            appointmentRespository = new AppointmentRespository(configuration);
        }
        // Get All Appointment
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Appointment>> Get()
        {
            var currentMerchant = HttpContext.User;
            if (currentMerchant.HasClaim(c => c.Type == "UserId") || currentMerchant.HasClaim(c => c.Type == "merchantId"))
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

            IEnumerable<Appointment> stores = appointmentRespository.FindAll();
            if (!stores.Any())
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = stores, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }

        //Get Appointment By ID
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<AppointmentBooking> Get(int id)
        {
            int storeID = 0;
            int UserId = 0;
            AppointmentBooking item = appointmentRespository.FindByID(id, ref storeID, ref UserId);
            var currentMerchant = HttpContext.User;
            int ID = 0;
            if (currentMerchant.HasClaim(c => c.Type == "UserId"))
            {
                Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out ID);
                if (ID != UserId)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
            }
            if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
            {
                Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out ID);
                if (ID != storeID)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
            }

            if (item == null)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }
        //Get Appointment By userID
        [HttpPost("ByUser/{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<AppointmentDetail>> GetByUser(int id)
        {

            var currentMerchant = HttpContext.User;
            int ID = 0;
            if (currentMerchant.HasClaim(c => c.Type == "UserId"))
            {
                Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out ID);
                if (ID != id)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
            }
            List<AppointmentDetail> List = appointmentRespository.FindAllByUserID(id).ToList();

            if (List.Count <= 0)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = List, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });





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
                // LogModel d = new LogModel();
                // d.UserId = 0;//Convert.ToInt32(item.User_id);
                // d.Message = "check";// Newtonsoft.Json.JsonConvert.SerializeObject(item);
                // d.Title = "User-Add-Appoinment";
                //  d.CreateDate = DateTime.UtcNow;

                // appointmentRespository.LogCheck(d);

                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

                var currentMerchant = HttpContext.User;

                if (item.Staff_id == null || item.Staff_id <= 0)
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


                int? appointmentId = appointmentRespository.Add(item, ref ex);
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
        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]Appointment item)
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
                if (appointmentRespository.Update(item, ref ex))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item.Id, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR ADVERTISEMENTS PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }

        // DELETE api/appointments/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Delete(int id)
        {
            try
            {
                appointmentRespository.Remove(id);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR ADVERTISEMENTS DELETE");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }
        [Route("Waiting")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<AppointmentBooking>> GetWaiting()
        {
            int ID = 0;
            var currentMerchant = HttpContext.User;
            if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });


            if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
                Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out ID);


            IEnumerable<AppointmentBooking> appointments = appointmentRespository.FindByCheck_InStatus(ID);

            if (appointments.ToList().Count <= 0)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = appointments, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }
        [Route("Member")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<AppointmentBooking>> GetByMemberdate([FromBody]MemberandDate values)
        {
            try
            {
                int ID = 0;
                var currentMerchant = HttpContext.User;
                if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

                if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out ID);
                string Message = string.Empty;
                IEnumerable<AppointmentBooking> appointments = appointmentRespository.FindByMemberIdAnDate(values, ID);

                if (appointments == null)
                {
                    Appointment p = new Appointment();
                    p.StoreId = ID;
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = p, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = appointments, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR Get Appointment By Member Array!!");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });

            }

        }

        [Route("Assign")]
        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]AssignAppointment item)
        {
            try
            {
                var currentMerchant = HttpContext.User;
                if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });


                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                Exception ex = new Exception();
                string tempMessage = string.Empty;
                if (appointmentRespository.AsignAppointment(item, ref ex, ref tempMessage))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item.appointmentid, Message = tempMessage });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });

            }
            catch (Exception ex)
            {

                //Log.Error(ex, "ERROR AsignAppointment PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });


            }

        }

        [HttpPut("Back/{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put(int id)
        {

            try
            {
                var currentMerchant = HttpContext.User;
                if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

                Exception ex = new Exception();

                if (appointmentRespository.PutBackAppointment(id, ref ex))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = id, Message = " Move appointment successfully!" });

                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });

            }
            catch (Exception ex)
            {

                //Log.Error(ex, "ERROR AsignAppointment PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });


            }

        }
        [Route("Status")]
        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]Paid PaidRequest)
        {

            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                Exception ex = new Exception();
                if (appointmentRespository.PaidForAppointment(PaidRequest, ref ex))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = PaidRequest.AppointmentId, Message = "Update status appointment successfully!" });
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });

            }
            catch (Exception ex)
            {

                //Log.Error(ex, "ERROR AsignAppointment PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });


            }
        }


    }
}
