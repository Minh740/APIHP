using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class Service_ExtraRespository : IRepository<Service_Extra>
    {


        private IConfiguration config;
        public Service_ExtraRespository(IConfiguration configuration)
        {
            config = configuration;

        }

        public int? Add(Service_Extra item)
        {
            int? Service_ExtraId = null;
            
                Base.Connection.Open();
                item.status = 1;
                Service_ExtraId = Base.Connection.Insert<Service_Extra>(item);
            
            return Service_ExtraId;
        }

        public IEnumerable<Service_Extra> FindAll()
        {

            Base.Connection.Open();
                return Base.Connection.GetList<Service_Extra>(new { status = 1 });
            
        }

        public Service_Extra FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<Service_Extra>(id);
            
        }

        public void Remove(int id)
        {


            Base.Connection.Open();
                Service_Extra item = Base.Connection.Get<Service_Extra>(id);
                item.status = -1;
            Base.Connection.Update<Service_Extra>(item);
            
        }

        public void Update(Service_Extra item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Service_Extra>(item);
            
        }
    }
}
