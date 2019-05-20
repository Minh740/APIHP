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
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductRespository productRespository;
        public ProductController(IConfiguration configuration)
        {
            productRespository = new ProductRespository(configuration);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Product>> Get()
        {
            IEnumerable<Product> stores = productRespository.FindAll();
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
        public ActionResult<Product> Get(int id)
        {
            Product item = productRespository.FindByID(id);
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
        public ActionResult Post([FromBody]Product item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });

                productRespository.Add(item);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR PRODUC POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }
        [HttpPost]
        [Route("searchByCategoryId/{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Product>> GetByCategoryId(int id)
        {

            int ID = 0;
            var currentMerchant = HttpContext.User;
            if (currentMerchant.HasClaim(c => c.Type == "UserId"))
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

            if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
                Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out ID);
            IEnumerable<Product> stores = productRespository.FindCatgoryAndStoreId(id, ID);
            if (!stores.Any())
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = stores, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }


        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]Product item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                productRespository.Update(item);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR PRODUC PUT");
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
                productRespository.Remove(id);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR PRODUC DELETE");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }
        [HttpPost]
        [Route("searchByName-BySku")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Product>> GetByNameBySku([FromBody]Key_Name_Sku item)

        {
            try
            {
                List<Product> Products = productRespository.FindByNameOrSku(item).ToList();
                if (Products != null && Products.Count > 0)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = Products, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                else
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });

            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR PRODUC GetByNameBySku");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });

            }

        }

        [HttpPost]
        [Route("SearchByNameSku")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Product>> ListNameSku([FromBody]Product item)
        {
            try
            {
                List<Product> Products = productRespository.SearchNameSku(item.Name, item.SKU).ToList();
                if (Products != null && Products.Count > 0)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = Products, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                else
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });

            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR PRODUC ListNameSku");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });

            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("ListByCategory/{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Product>> GetListByCategory(int id)
        {
            try
            {
                List<Product> Products = productRespository.ListByCategory(id).ToList();
                if (Products != null && Products.Count > 0)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = Products, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                else
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });

            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR PRODUC ListByCategory");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });

            }
        }

    }
}
