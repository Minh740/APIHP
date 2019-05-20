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
    public class StoresController : Controller
    {
        private readonly StoreRepository storeRepository;
        public StoresController(IConfiguration configuration)
        {
            storeRepository = new StoreRepository(configuration);
        }
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Store>> Get()
        {
            var currentMerchant = HttpContext.User;
            if (currentMerchant.HasClaim(c => c.Type == "merchantId"))
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

            IEnumerable<Store> stores = storeRepository.FindAll();
            if (!stores.Any())
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = stores, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Store> Get(int id)
        {
            var currentMerchant = HttpContext.User;
            int ID = 0;
            if (currentMerchant.HasClaim(c => c.Type == "merchantId"))
            {

                Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "merchantId").Value, out ID);

                Store store = storeRepository.FindByID(id);

                if (store == null)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                if (store.merchantid != ID)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = store, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });

            }
            else
            {
                if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "UserId").Value, out ID);

                    Store store = storeRepository.FindByID(id);
                    if (store == null)
                    {
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                    }
                    if (ID == 0)
                    {
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                    }
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = store, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                }

            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, "Request Fail") });

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Add")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Post([FromBody]Store store)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                string mesage = string.Empty;
                int? storeId = storeRepository.Add(store, ref mesage);

                if (storeId == -1)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Dupllicate, CodeNumber = "409", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Dupllicate, mesage) });
                if (storeId == -2)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, mesage) });


                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = storeId, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });

            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR STORE POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }

        // PUT api/stores/5
        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]Store store)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

                var currentMerchant = HttpContext.User;
                int ID = 0;
                if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

                if (currentMerchant.HasClaim(c => c.Type == "merchantId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "merchantId").Value, out ID);
                }
                if (ID != store.merchantid)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });
                }
                storeRepository.Update(store);
                return Ok();
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR STORE POST");
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
                storeRepository.Remove(id);
                return Ok();
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR STORE DELETE");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }


        // find nearest store
        [Route("findNearest")]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult FindNearest([FromBody]Coordinate coordinate)
        {
            try
            {
                List<Store> stores = storeRepository.FindNest(coordinate);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = stores, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR STORE POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }

        // find nearest store
        [Route("findByMerchant")]
        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult FindByMerchant()
        {
            try
            {
                var currentMerchant = HttpContext.User;
                int merchantId = 0;
                if (currentMerchant.HasClaim(c => c.Type == "merchantId"))
                {
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "merchantId").Value, out merchantId);
                }
                Store store = storeRepository.FindByMerchantId(merchantId);
                if (store != null)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = store, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                return BadRequest();
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR STORE POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }

    }
}
