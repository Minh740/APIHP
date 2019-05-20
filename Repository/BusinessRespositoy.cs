using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class BusinessRespositoy : IRepository<Business>
    {
        public BusinessRespositoy(IConfiguration configuration)
        {
          
        }

        public int? Add(Business item)
        {
            int? giftId = null;

            Base.Connection.Open();
                giftId = Base.Connection.Insert<Business>(item);
            
            return giftId;
        }


        public Business FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<Business>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Business Business = Base.Connection.Get<Business>(id);
            Base.Connection.Update<Business>(Business);
            
        }

        public void Update(Business item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Business>(item);
            
        }



        public IEnumerable<Business> FindAll()
        {
           
                Base.Connection.Open();
                return Base.Connection.GetList<Business>(new { status = 1 });
            
        }
    }
}
