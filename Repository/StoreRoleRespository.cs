using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class StoreRoleRespository : IRepository<StoreRole>
    {
        public StoreRoleRespository(IConfiguration configuration)
        {
            
        }

        public int? Add(StoreRole item)
        {
            int? roleId = null;
           
                Base.Connection.Open();
                roleId = Base.Connection.Insert<StoreRole>(item);
            
            return roleId;
        }


        public StoreRole FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<StoreRole>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                StoreRole storeRole = Base.Connection.Get<StoreRole>(id);
                storeRole.Status = -1;
            Base.Connection.Update<StoreRole>(storeRole);
            
        }

        public void Update(StoreRole item)
        {

            Base.Connection.Open();
            Base.Connection.Update<StoreRole>(item);
            
        }



        public IEnumerable<StoreRole> FindAll()
        {

            Base.Connection.Open();

                return Base.Connection.GetList<StoreRole>(new { status = 1 });
            
        }
    }
}
