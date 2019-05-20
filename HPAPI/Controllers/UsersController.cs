using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entities;
using HPAPI.Models;
using HPAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repository;

namespace HPAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IConfiguration configuration;
        public UsersController(IConfiguration config)
        {
            configuration = config;
        }
        // login
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Login([FromBody] Entities.Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                string Messageage = string.Empty;


                string ecryptPass = UTIL.Encrypt(login.Password, true, configuration);
                User u = UserRepository.Login(login.Email, ecryptPass);


                if (u!=null)
                {
                    JWTAuthentication jWTAuthentication = new JWTAuthentication(configuration);
                    u.token = jWTAuthentication.GenerateJSONWebToken("UserId", u.email, u.id.ToString(), "", "");
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = u, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, Messageage) });

            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR USER LOIN");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });

            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Post([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

       
            user.create_date = DateTime.UtcNow;
            user.credit = 0;
            user.state_id = 1;
            user.city_id = 1;

            string ecryptPass = UTIL.Encrypt(user.password, true, configuration);
            user.password = ecryptPass;

            int? userId = UserRepository.Add(user);

            if (userId == 0)
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Dupllicate, CodeNumber = "409", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Dupllicate, "") });


            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = userId, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }

        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
            var currentMerchant = HttpContext.User;
            int UserId = 0;

            if (currentMerchant.HasClaim(c => c.Type == "merchantId"))
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

            if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out UserId);


            if (user.id != UserId)
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

            //string ecryptPass = UTIL.Encrypt(user.password, true, configuration);
            //user.password = ecryptPass;


            UserRepository.Update(user);
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Delete(int id)
        {
            UserRepository.Remove(id);
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }


        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Get()

        {
            try
            {
                var currentMerchant = HttpContext.User;
                if (currentMerchant.HasClaim(c => c.Type == "UserId") || currentMerchant.HasClaim(c => c.Type == "merchantId"))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

                IEnumerable<User> users = UserRepository.FindAll();

                if (!users.Any())
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }

                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = users, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error("Error: " + ex.ToString());
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Get(int id)
        {
            var CurrentUser = HttpContext.User;
            int ID = 0;
            if (CurrentUser.HasClaim(c => c.Type == "UserId"))
            {
                Int32.TryParse(CurrentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out ID);

                User user = UserRepository.FindByID(id);
                if (user == null)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                if (user.id != ID)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                }

                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = user, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });

            }
            else
            {
                if (CurrentUser.HasClaim(c => c.Type == "merchantId"))
                {

                    Int32.TryParse(CurrentUser.Claims.FirstOrDefault(c => c.Type == "merchantId").Value, out ID);

                    User user = UserRepository.FindByID(id);
                    if (user == null)
                    {
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                    }
                    if (ID == 0)
                    {
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                    }
                }

            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, "Request Fail") });

        }

        [HttpPost]
        [Route("ChangePassword")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult ChangePassword([FromBody] ChangePasswordValue item)
        {

            if (!ModelState.IsValid)
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

            var currentMerchant = HttpContext.User;
            int UserId = 0;
            if (currentMerchant.HasClaim(c => c.Type == "UserId"))
            {
                Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out UserId);

                User tempuser = UserRepository.FindByID(UserId);

                if (tempuser == null)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });

                }
                if (tempuser.password != UTIL.Encrypt(item.currentPassword, true, configuration))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, "Password incorrect!!") });

                tempuser.password = UTIL.Encrypt(item.newPassword, true, configuration);
                UserRepository.Update(tempuser);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
        }

        [Route("DeleteAll")]
        [HttpDelete]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult DeleteAll()
        {

            UserRepository.RemoveAll();
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });

        }


    }
}