using Entities;
using HPAPI.Utils;
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
    public class MerchantController : Controller
    {
        private readonly MerchantRespository merchantRespository;
        private IConfiguration configuration;
        public MerchantController(IConfiguration config)
        {
            configuration = config;
            merchantRespository = new MerchantRespository(configuration);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Merchant>> Get()
        {
            var currentMerchant = HttpContext.User;
            if (currentMerchant.HasClaim(c => c.Type == "UserId") || currentMerchant.HasClaim(c => c.Type == "merchantId"))
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });


            IEnumerable<Merchant> merchants = merchantRespository.FindAll();
            if (!merchants.Any())
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = merchants, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });

        }


        [HttpPost("{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Merchant> Get(int id)
        {
            Merchant item = merchantRespository.FindByID(id);
            if (item == null)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }

        // POST api/merchant
        // register 
        [AllowAnonymous]
        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Post([FromBody]Merchant item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                string Messageage = string.Empty;
                item.Password = UTIL.Encrypt(item.Password, true, configuration);
                int? merchantId = merchantRespository.Add(item, ref Messageage);

                if (merchantId == 0)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Dupllicate, CodeNumber = "409", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Dupllicate, Messageage) });

                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR MERCHANT POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }
        // login
        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Login([FromBody]Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                string Messageage = string.Empty;
                Merchant merchant = new Merchant();

                string ecryptPass = UTIL.Encrypt(login.Password, true, configuration);

                if (merchantRespository.Login(login.Email, ecryptPass, ref merchant, ref Messageage))
                {
                    JWTAuthentication jWTAuthentication = new JWTAuthentication(configuration);
                    Store store = new StoreRepository(configuration).FindByMerchantId(merchant.Id);

                    string token;
                    if (store != null)
                    {
                        merchant.Storeid = store.Id;
                        token = jWTAuthentication.GenerateJSONWebToken("merchantId", merchant.Email, merchant.Id.ToString(), "StoreId", store.Id.ToString());

                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = new { Merchant = merchant, Token = token }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                    }
                    else
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = "Merchant don't have Store" });


                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, Messageage) });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR MERCHANT LOIN");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }

        [AllowAnonymous]
        [Route("UpdatePlayerId")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult UpdatePlayerId([FromBody] Newtonsoft.Json.Linq.JObject datos)
        {
            try
            {
                FlagReposity d = new FlagReposity(configuration);
                Flag f = new Flag();
                f.StoreId = Convert.ToInt32(datos["storeid"].ToString());
                f.Token = datos["playerid"].ToString();
                d.AddFlag(f);

                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = "OK " });

            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR MERCHANT LOIN");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = ex.ToString(), Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]Merchant item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                merchantRespository.Update(item);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR MERCHANT PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Delete(int id)
        {
            try
            {
                merchantRespository.Remove(id);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR MERCHANT DELETE");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }

        [Route("CheckFlag")]
        [HttpGet]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public int CheckFlag(string playerid)
        {
            try
            {
                FlagReposity FR = new FlagReposity(configuration);
                Flag flag = FR.FindByToken(playerid);
                if (flag != null)
                    return flag.flag;
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [Route("DoneFlag")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public void DoneFlag(string playerid)
        {
            try
            {
                FlagReposity FR = new FlagReposity(configuration);
                FR.DoneFlag(playerid);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
