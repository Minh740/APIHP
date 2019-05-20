using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class PrincipalRespository : IRepository<Principal>
    {

        public PrincipalRespository(IConfiguration configuration)
        {
           
        }
       


        public int? Add(Principal item)
        {
            int? giftId = null;

            Base.Connection.Open();
                giftId = Base.Connection.Insert<Principal>(item);
            
            return giftId;
        }


        public Principal FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<Principal>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Principal Principal = Base.Connection.Get<Principal>(id);
            Base.Connection.Update<Principal>(Principal);
            
        }

        public void Update(Principal item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Principal>(item);
            
        }



        public IEnumerable<Principal> FindAll()
        {
           
                Base.Connection.Open();
            return Base.Connection.GetList<Principal>(new { status = 1 });
            
        }
    }
}
