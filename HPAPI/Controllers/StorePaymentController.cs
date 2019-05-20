using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPAPI.Controllers
{
    [Route("api/[controller]")]
    public class StorePaymentController : Controller
    {
        private readonly StorePaymentRespository storePaymentRespository;
        public StorePaymentController(IConfiguration configuration)
        {
            storePaymentRespository = new StorePaymentRespository(configuration);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<StorePayment>> Get()
        {
            IEnumerable<StorePayment> stores = storePaymentRespository.FindAll();
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
        public ActionResult<StorePayment> Get(int id)
        {
            StorePayment item = storePaymentRespository.FindByID(id);
            if (item == null)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }


        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Post([FromBody]StorePayment item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                storePaymentRespository.Add(item);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR STORE PAYMENT POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }


        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]StorePayment item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                storePaymentRespository.Update(item);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR STORE PAYMENT PUT");
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
                storePaymentRespository.Remove(id);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR STORE PAYMENT DELETE");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }
    }
}
