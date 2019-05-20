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
    public class ServiceController : Controller
    {
        private readonly ServiceRespository serviceRespository;
        private IConfiguration config;
        public ServiceController(IConfiguration configuration)
        {
            config = configuration;
            serviceRespository = new ServiceRespository(configuration);
        }

        [HttpPost]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<IEnumerable<Service>> Get()
        {
            IEnumerable<Service> stores = serviceRespository.FindAll();
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
        public ActionResult<Service> Get(int id)
        {
            Service item = serviceRespository.FindByID(id);
            if (item == null)
            {
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
            }
            return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = item, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
        }
        [HttpPost]
        [Route("Array")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Service> Get([FromBody]ArrayServiceId Array)
        {
            try
            {
                if (Array == null)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, "Request Fail") });

                string message = string.Empty;
                List<Service> listitem = serviceRespository.FindbyArrayServiceId(Array.ArrayId, ref message).ToList();
                if (listitem == null)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = listitem, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error("Error" + ex.ToString());
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });


            }
        }

        [HttpPost]
        [Route("Add")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Post([FromBody]Service item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                item.Status = 1;
                item.ArchiveStatus = 1;
                serviceRespository.Add(item);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR SERVICE POST");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }


        [HttpPut]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult Put([FromBody]Service item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                if (item.CategoryId == 0)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = "This Service don't have CatgoryID " });

                var currentMerchant = HttpContext.User;
                if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
                {
                    int Store_id = 0;
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out Store_id);
                    Category cat = new CategoryRespository(config).FindByID(item.CategoryId);

                    if (cat == null)
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = "Your Category don't exits! " });

                    if (cat.StoreId != Store_id)
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

                    Service temp = new ServiceRespository(config).FindByID(item.Id);
                    string message = string.Empty;
                    item.ArchiveStatus = temp.ArchiveStatus;
                    item.Status = temp.Status;
                    if (serviceRespository.Update(item, ref message))
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                    else
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = message });

                }

                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR SERVICE PUT");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }

        }


        [HttpPut]
        [Route("Archive")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult PutArchive([FromBody]Service item)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ModelState.ToString()) });
                if (item.CategoryId == 0)
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = "This Service don't have CatgoryID " });

                var currentMerchant = HttpContext.User;
                if (currentMerchant.HasClaim(c => c.Type == "StoreId"))
                {
                    int Store_id = 0;
                    Int32.TryParse(currentMerchant.Claims.FirstOrDefault(c => c.Type == "StoreId").Value, out Store_id);
                    Category cat = new CategoryRespository(config).FindByID(item.CategoryId);

                    if (cat == null)
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = "Your Category don't exits! " });

                    if (cat.StoreId != Store_id)
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

                    Service temp = new ServiceRespository(config).FindByID(item.Id);
                    string message = string.Empty;
                    temp.ArchiveStatus = item.ArchiveStatus;
                    temp.Status = item.Status;
                    if (serviceRespository.UpdateArchive(temp, ref message))
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
                    else
                        return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = message });


                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Unauthorized, CodeNumber = "401", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Unauthorized, "") });

            }
            catch (Exception ex)
            {

                //Log.Error(ex, "ERROR CATEGORY PUT");
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
                serviceRespository.Remove(id);
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error(ex, "ERROR SERVICE DELETE");
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }
        [HttpPost]
        [Route("ByName/{name}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Service> FindServiceByName(string name)
        {
            try
            {
                string message = string.Empty;
                List<Service> listitem = serviceRespository.FindServicebyName(name).ToList();
                if (listitem == null)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = listitem, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error("Error" + ex.ToString());
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });


            }
        }

        [HttpPost]
        [Route("ByCategory/{id}")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult<Service> searchByCategoryId(int id)
        {
            try
            {
                string message = string.Empty;
                List<Service> listitem = serviceRespository.FindByCategoryId(id).ToList();
                if (listitem == null || listitem.Count == 0)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = listitem, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error("Error" + ex.ToString());
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }

        [HttpPost]
        [Route("GetExtraService")]
        [ProducesResponseType(typeof(NewHttpRespone), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public ActionResult GetExtraServiceByServiceId(int serviceid)
        {
            try
            {
                string message = string.Empty;
                List<Extra> listitem = serviceRespository.FindExtraByServiceId(serviceid).ToList();
                if (listitem == null || listitem.Count == 0)
                {
                    return Ok(new NewHttpRespone { CodeStatus = ResponeCode.InvaliedData, CodeNumber = "404", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.InvaliedData, "") });
                }
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Success, CodeNumber = "200", Data = listitem, Message = new NewHttpRespone().setData(ResponeCode.Success, "") });
            }
            catch (Exception ex)
            {
                //Log.Error("Error" + ex.ToString());
                return Ok(new NewHttpRespone { CodeStatus = ResponeCode.Error, CodeNumber = "400", Data = { }, Message = new NewHttpRespone().setData(ResponeCode.Error, ex.ToString()) });
            }
        }
    }
}
