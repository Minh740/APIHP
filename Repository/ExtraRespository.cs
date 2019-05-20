using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class ExtraRespository : IRepository<Extra>
    {
        private IConfiguration config;
        public ExtraRespository(IConfiguration configuration)
        {
            config = configuration;
           
        }

        

        public int? Add(Extra item)
        {
            int? ExtraId = null;

            Base.Connection.Open();
                ExtraId = Base.Connection.Insert<Extra>(item);
            
            return ExtraId;
        }

        public IEnumerable<Extra> FindAll()
        {

            Base.Connection.Open();
            return Base.Connection.GetList<Extra>(new { status = 1 });
            
        }

        public Extra FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<Extra>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Extra Extra = Base.Connection.Get<Extra>(id);
                Extra.status = -1;
            Base.Connection.Update<Extra>(Extra);
            
        }

        public void Update(Extra item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Extra>(item);
            
        }
        public IEnumerable<Extra> InsertListExtra(string json)
        {
            IEnumerable<Extra> resuilt;
            
                Base.Connection.Open();
                resuilt = Base.Connection.Query<Extra>("select InsertIntoExtra (" + json + ")");
            
            return resuilt;
        }
    }
}
