using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class ServiceRespository : IRepository<Service>
    {
        private IConfiguration config;
        public ServiceRespository(IConfiguration configuration)
        {
            config = configuration;
          
        }

        public int? Add(Service item)
        {
            int? serviceId = null;
          
            
                Base.Connection.Open();
                serviceId = Base.Connection.Insert<Service>(item);
            
            return serviceId;
        }

        public bool AddServiceAndExtra(Service item, ref string message)
        {
            try
            {


                Base.Connection.Open();
                    int? service = Base.Connection.Insert<Service>(item);

                    for (int i = 0; i < item.ListExtra.Count; i++)
                    {
                        if (item.ListExtra[i].Id == 0)
                        {

                            int? extraid = Base.Connection.Insert<Extra>(item.ListExtra[i]);
                        Base.Connection.Insert<Service_Extra>(new Service_Extra { status = 1, ServiceId = service, ExtraId = extraid });
                        }
                        else
                        {

                        Base.Connection.Update<Extra>(item.ListExtra[i]);
                        }


                    }
                    message = "Success";
                    return true;
                
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return false;
            }

        }

        public IEnumerable<Service> FindbyArrayServiceId(int[] Array, ref string message)
        {

            try
            {





                Base.Connection.Open();

                    string array = "(";
                    for (int i = 0; i < Array.Length; i++)
                    {
                        array += Array[i].ToString();
                        if (i < Array.Length - 1)
                            array += ",";

                    }
                    array = array + ")";
                    IEnumerable<Service> services = Base.Connection.Query<Service>("select*from \"Service\" Where id IN" + array);

                    return services;
                


            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return null;

            }


        }

        public Service FindByID(int id)
        {
            try
            {

                Base.Connection.Open();
                return Base.Connection.Get<Service>(id);
                
            }
            catch (Exception ex)
            {
                Service d = new Service();
                d.Id = 0;
                d.Name = "Service Error";
                d.Description = ex.ToString();
                return d;
            }
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Service service = Base.Connection.Get<Service>(id);
            Base.Connection.Update<Service>(service);
            
        }

        public bool Update(Service item, ref string mess)
        {
            try
            {

                Base.Connection.Open();

                Base.Connection.Update<Service>(item);
                    mess = "Success";
                    return true;
                
            }
            catch (Exception ex)
            {
                mess = ex.ToString();
                return false;
            }
        }

        public bool UpdateServiceExtra(Service item, ref string message)
        {
            try
            {
                Base.Connection.Open();
                    int? serviceid = Base.Connection.Update<Service>(item);

                    IEnumerable<Service_Extra> SEL = Base.Connection.GetList<Service_Extra>(new { ServiceId = serviceid, status = 1 }).ToList();


                    message = "Success";
                    return true;
                
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return false;
            }
        }


        public bool UpdateArchive(Service item, ref string message)
        {
            try
            {


                Base.Connection.Update<Service>(item);
                    message = "Success";
                    return true;
                

            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return false;
            }
        }


        public IEnumerable<Service> FindAll()
        {

            Base.Connection.Open();
            return Base.Connection.GetList<Service>(new { status = 1 });
            
        }

        public IEnumerable<Service> InsertListService(string json)
        {
            IEnumerable<Service> resuilt;

            Base.Connection.Open();
                resuilt = Base.Connection.Query<Service>("select InsertIntoService (" + json + ")");
            
            return resuilt;
        }

        public void Update(Service item)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Service> FindServicebyName(string name)
        {

            Base.Connection.Open();
                IEnumerable<Service> servciceList = Base.Connection.GetList<Service>("where status=1 and name like  '%" + name + "%' ");
                List<Service_Extra> servcice_Extra = Base.Connection.GetList<Service_Extra>(new { status = 1 }).ToList();

                if (servciceList != null)
                {

                    foreach (Service sr in servciceList)
                    {
                        sr.ListExtra = new List<Extra>();
                        foreach (Service_Extra ex in servcice_Extra)
                        {
                            if (sr.Id == ex.ServiceId)
                            {
                                Extra extr = Base.Connection.Get<Extra>(ex.id);
                                sr.ListExtra.Add(extr);
                            }
                        }
                    }

                }
                return servciceList.ToList();
            
        }
        public IEnumerable<Service> FindByCategoryId(int categoryId)
        {

            Base.Connection.Open();
                IEnumerable<Service> servciceList = Base.Connection.GetList<Service>("where status = 1 and category_id = " + categoryId);
                List<Service_Extra> ser_extra = Base.Connection.GetList<Service_Extra>(new { status = 1 }).ToList();
                if (servciceList != null)
                {
                    foreach (Service ser in servciceList)
                    {
                        ser.ListExtra = new List<Extra>();
                        foreach (Service_Extra exS in ser_extra)
                        {
                            if (ser.Id == exS.ServiceId)
                            {
                                Extra ex = Base.Connection.Get<Extra>(exS.id);
                                ser.ListExtra.Add(ex);
                            }
                        }
                    }
                }

                return servciceList.ToList();
            
        }

        public IEnumerable<Extra> FindExtraByServiceId(int serviceid)
        {

            Base.Connection.Open();
                IEnumerable<Service_Extra> servciceList = Base.Connection.GetList<Service_Extra>("where status = 1 and service_id = " + serviceid);
                List<Extra> extraList = new List<Extra>();

                if (servciceList != null)
                {
                    foreach (Service_Extra ser in servciceList)
                    {
                        Extra ex = Base.Connection.Get<Extra>(ser.ExtraId);
                        extraList.Add(ex);
                    }
                }

                return extraList.ToList();
        }            
    }
}
